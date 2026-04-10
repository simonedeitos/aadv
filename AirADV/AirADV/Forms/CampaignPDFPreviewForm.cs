using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class CampaignPDFPreviewForm : Form
    {
        private DbcManager.Campaign _campaign;
        private List<ScheduleEngine.DailySchedule> _dailySchedules;
        private List<DbcManager.Spot> _spots;
        private DbcManager.Client _client;
        private DbcManager.Station _station;

        private PrintDocument _printDocument;
        private int _currentPage = 0;
        private const int ITEMS_PER_PAGE = 25;

        private Dictionary<int, string> _spotLetters;

        public CampaignPDFPreviewForm(
            DbcManager.Campaign campaign,
            List<ScheduleEngine.DailySchedule> dailySchedules,
            List<DbcManager.Spot> spots,
            DbcManager.Client client)
        {
            InitializeComponent();
            _campaign = campaign;
            _dailySchedules = dailySchedules;
            _spots = spots;
            _client = client;

            LoadStationData();
            GenerateSpotLetters();
            InitializePrintDocument();
            this.Load += CampaignPDFPreviewForm_Load;
        }

        private void LoadStationData()
        {
            try
            {
                var stations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc");
                _station = stations.FirstOrDefault(s => s.StationID == _campaign.StationID);

                if (_station == null)
                {
                    _station = new DbcManager.Station
                    {
                        StationName = LanguageManager.Get("PDFPreview.UnknownStation", "Emittente Sconosciuta"),
                        Frequency = "N/A",
                        LogoPath = ""
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDFPreview] Errore caricamento stazione: {ex.Message}");
                _station = new DbcManager.Station
                {
                    StationName = LanguageManager.Get("PDFPreview.UnknownStation", "Emittente Sconosciuta"),
                    Frequency = "N/A",
                    LogoPath = ""
                };
            }
        }

        private void GenerateSpotLetters()
        {
            _spotLetters = new Dictionary<int, string>();

            for (int i = 0; i < _spots.Count; i++)
            {
                string letter = GetLetterForIndex(i);
                _spotLetters[_spots[i].ID] = letter;

                Console.WriteLine($"[PDFPreview] Spot '{_spots[i].SpotTitle}' → Lettera '{letter}'");
            }
        }

        private string GetLetterForIndex(int index)
        {
            string result = "";
            while (index >= 0)
            {
                result = (char)('A' + (index % 26)) + result;
                index = (index / 26) - 1;
            }
            return result;
        }

        private string GetLetterForSlot(int dayIndex, int slotIndex)
        {
            if (_spots.Count == 0) return "? ";

            int totalSlotsBefore = 0;
            for (int d = 0; d < dayIndex; d++)
            {
                if (d < _dailySchedules.Count)
                    totalSlotsBefore += _dailySchedules[d].TimeSlots.Count;
            }

            int globalSlotIndex = totalSlotsBefore + slotIndex;
            int spotIndex = globalSlotIndex % _spots.Count;

            return _spotLetters[_spots[spotIndex].ID];
        }

        private void InitializePrintDocument()
        {
            _printDocument = new PrintDocument();
            _printDocument.DefaultPageSettings.Landscape = false;
            _printDocument.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void CampaignPDFPreviewForm_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyLanguage();

                _previewControl.Document = _printDocument;
                _previewControl.Zoom = 1.0;
                _previewControl.Refresh();

                LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("PDFPreview.LoadError", "Errore caricamento anteprima")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            Console.WriteLine("[PDFPreview] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[PDFPreview] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                // ✅ Titolo finestra
                this.Text = string.Format(
                    LanguageManager.Get("PDFPreview.WindowTitle", "📄 Anteprima Programmazione - {0}"),
                    _campaign.CampaignName
                );

                // ✅ NUOVO: Titolo principale del form (Label o GroupBox)
                // Cerca per nome controllo
                var lblMainTitleArray = this.Controls.Find("lblMainTitle", true);
                if (lblMainTitleArray.Length > 0 && lblMainTitleArray[0] is Label lblMainTitle)
                {
                    lblMainTitle.Text = LanguageManager.Get("PDFPreview.MainTitle", "📄 Anteprima Programmazione Campagna Pubblicitaria");
                }

                // Oppure cerca per GroupBox
                var grpMainArray = this.Controls.Find("grpMain", true);
                if (grpMainArray.Length > 0 && grpMainArray[0] is GroupBox grpMain)
                {
                    grpMain.Text = LanguageManager.Get("PDFPreview.MainTitle", "📄 Anteprima Programmazione Campagna Pubblicitaria");
                }

                // ✅ ALTERNATIVA: Cerca dinamicamente per testo
                TranslateControlByText(this,
                    "Anteprima Programmazione Campagna Pubblicitaria",
                    "Campaign Schedule Preview",
                    LanguageManager.Get("PDFPreview.MainTitle", "📄 Anteprima Programmazione Campagna Pubblicitaria"));

                // ✅ Bottoni
                btnSavePDF.Text = LanguageManager.Get("PDFPreview.BtnSavePDF", "💾 Salva PDF");
                btnPrint.Text = LanguageManager.Get("PDFPreview.BtnPrint", "🖨️ Stampa");
                btnZoomIn.Text = LanguageManager.Get("PDFPreview.BtnZoomIn", "🔍+");
                btnZoomOut.Text = LanguageManager.Get("PDFPreview.BtnZoomOut", "🔍-");
                btnClose.Text = LanguageManager.Get("Common.Close", "✖ Chiudi");

                // ✅ Refresh anteprima per riapplicare lingua nel documento
                if (_previewControl.Document != null)
                {
                    _currentPage = 0;
                    _previewControl.InvalidatePreview();
                }

                Console.WriteLine("[PDFPreview] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDFPreview] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        // ✅ Metodo helper per tradurre controllo per testo
        private void TranslateControlByText(Control parent, string italianText, string englishText, string newText)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (!string.IsNullOrEmpty(ctrl.Text) &&
                    (ctrl.Text.Contains(italianText) || ctrl.Text.Contains(englishText)))
                {
                    ctrl.Text = newText;
                    Console.WriteLine($"[PDFPreview] ✅ Tradotto {ctrl.GetType().Name}: {ctrl.Name}");
                    return;
                }

                if (ctrl.HasChildren)
                {
                    TranslateControlByText(ctrl, italianText, englishText, newText);
                }
            }
        }

        private string FormatTimeSlot(string timeSlot)
        {
            if (string.IsNullOrEmpty(timeSlot))
                return timeSlot;

            if (timeSlot.Length <= 5)
                return timeSlot;

            if (timeSlot.Length >= 8 && timeSlot.Contains(":"))
            {
                var parts = timeSlot.Split(':');
                if (parts.Length >= 2)
                {
                    return $"{parts[0]}:{parts[1]}";
                }
            }

            return timeSlot;
        }

        private string GetShortDayName(DateTime date)
        {
            // ✅ Usa cultura corrente per ottenere nome giorno
            string fullDay = date.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);

            // ✅ Mappa abbreviazioni (IT e EN)
            var dayMap = new Dictionary<string, string>
            {
                { "lunedì", LanguageManager.Get("Common.DayMondayShort", "Lun.") },
                { "martedì", LanguageManager.Get("Common.DayTuesdayShort", "Mar.") },
                { "mercoledì", LanguageManager.Get("Common.DayWednesdayShort", "Mer.") },
                { "giovedì", LanguageManager.Get("Common.DayThursdayShort", "Gio.") },
                { "venerdì", LanguageManager.Get("Common.DayFridayShort", "Ven.") },
                { "sabato", LanguageManager.Get("Common.DaySaturdayShort", "Sab.") },
                { "domenica", LanguageManager.Get("Common.DaySundayShort", "Dom.") },
                { "monday", LanguageManager.Get("Common.DayMondayShort", "Mon.") },
                { "tuesday", LanguageManager.Get("Common.DayTuesdayShort", "Tue.") },
                { "wednesday", LanguageManager.Get("Common.DayWednesdayShort", "Wed.") },
                { "thursday", LanguageManager.Get("Common.DayThursdayShort", "Thu.") },
                { "friday", LanguageManager.Get("Common.DayFridayShort", "Fri.") },
                { "saturday", LanguageManager.Get("Common.DaySaturdayShort", "Sat.") },
                { "sunday", LanguageManager.Get("Common.DaySundayShort", "Sun.") }
            };

            string lowerDay = fullDay.ToLower();
            return dayMap.ContainsKey(lowerDay) ? dayMap[lowerDay] : fullDay.Substring(0, Math.Min(3, fullDay.Length)) + ".";
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font fontTitle = new Font("Segoe UI", 16, FontStyle.Bold);
                Font fontHeader = new Font("Segoe UI", 12, FontStyle.Bold);
                Font fontNormal = new Font("Segoe UI", 10, FontStyle.Regular);
                Font fontSmall = new Font("Segoe UI", 9, FontStyle.Regular);
                Font fontBold = new Font("Segoe UI", 10, FontStyle.Bold);

                Brush brushBlack = Brushes.Black;
                Brush brushGray = Brushes.Gray;
                Brush brushBlue = new SolidBrush(Color.FromArgb(0, 123, 255));

                Pen penLine = new Pen(Color.LightGray, 1);
                Pen penThick = new Pen(Color.FromArgb(0, 123, 255), 2);

                float yPos = e.MarginBounds.Top;
                float leftMargin = e.MarginBounds.Left;
                float rightMargin = e.MarginBounds.Right;
                float pageWidth = e.MarginBounds.Width;

                // ═══════════════════════════════════════════════════════════
                // 1.LOGO E NOME EMITTENTE
                // ═══════════════════════════════════════════════════════════

                if (!string.IsNullOrEmpty(_station.LogoPath) && File.Exists(_station.LogoPath))
                {
                    try
                    {
                        Image logo = Image.FromFile(_station.LogoPath);
                        float logoHeight = 60;
                        float logoWidth = (logo.Width * logoHeight) / logo.Height;
                        g.DrawImage(logo, leftMargin, yPos, logoWidth, logoHeight);
                        yPos += logoHeight + 10;
                    }
                    catch { }
                }

                g.DrawString(_station.StationName, fontTitle, brushBlue, leftMargin, yPos);
                yPos += 25;

                if (!string.IsNullOrEmpty(_station.Frequency))
                {
                    g.DrawString(
                        string.Format(
                            LanguageManager.Get("PDFPreview.Frequency", "Frequenza: {0}"),
                            _station.Frequency
                        ),
                        fontSmall,
                        brushGray,
                        leftMargin,
                        yPos
                    );
                    yPos += 20;
                }

                yPos += 10;
                g.DrawLine(penThick, leftMargin, yPos, rightMargin, yPos);
                yPos += 20;

                // ═══════════════════════════════════════════════════════════
                // 2.DATI CLIENTE
                // ═══════════════════════════════════════════════════════════

                g.DrawString(
                    LanguageManager.Get("PDFPreview.ClientDataTitle", "DATI CLIENTE"),
                    fontHeader,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                yPos += 25;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.Client", "Cliente: "),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString(_client.ClientName, fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                if (!string.IsNullOrEmpty(_client.CompanyName))
                {
                    g.DrawString(
                        LanguageManager.Get("PDFPreview.CompanyName", "Ragione Sociale:"),
                        fontBold,
                        brushBlack,
                        leftMargin,
                        yPos
                    );
                    g.DrawString(_client.CompanyName, fontNormal, brushBlack, leftMargin + 150, yPos);
                    yPos += 20;
                }

                if (!string.IsNullOrEmpty(_client.Email))
                {
                    g.DrawString(
                        LanguageManager.Get("PDFPreview.Email", "Email:"),
                        fontBold,
                        brushBlack,
                        leftMargin,
                        yPos
                    );
                    g.DrawString(_client.Email, fontNormal, brushBlack, leftMargin + 150, yPos);
                    yPos += 20;
                }

                if (!string.IsNullOrEmpty(_client.Phone))
                {
                    g.DrawString(
                        LanguageManager.Get("PDFPreview.Phone", "Telefono:"),
                        fontBold,
                        brushBlack,
                        leftMargin,
                        yPos
                    );
                    g.DrawString(_client.Phone, fontNormal, brushBlack, leftMargin + 150, yPos);
                    yPos += 20;
                }

                yPos += 10;
                g.DrawLine(penLine, leftMargin, yPos, rightMargin, yPos);
                yPos += 20;

                // ═══════════════════════════════════════════════════════════
                // 3.DATI CAMPAGNA
                // ═══════════════════════════════════════════════════════════

                g.DrawString(
                    LanguageManager.Get("PDFPreview.CampaignDataTitle", "DATI CAMPAGNA PUBBLICITARIA"),
                    fontHeader,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                yPos += 25;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.CampaignName", "Nome Campagna:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString(_campaign.CampaignName, fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.CampaignCode", "Codice Campagna:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString(_campaign.CampaignCode, fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.Period", "Periodo:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString(
                    $"{_campaign.StartDate:dd/MM/yyyy} - {_campaign.EndDate:dd/MM/yyyy}",
                    fontNormal,
                    brushBlack,
                    leftMargin + 150,
                    yPos
                );
                yPos += 20;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.ScheduledDays", "Giorni schedulati:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString($"{_dailySchedules.Count}", fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                g.DrawString(
                    LanguageManager.Get("PDFPreview.DailyPasses", "Passaggi al giorno:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString($"{_campaign.DailyPasses}", fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                int totalPasses = _dailySchedules.Sum(d => d.TimeSlots.Count);
                g.DrawString(
                    LanguageManager.Get("PDFPreview.TotalPasses", "Totale passaggi:"),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                g.DrawString($"{totalPasses}", fontNormal, brushBlack, leftMargin + 150, yPos);
                yPos += 20;

                // ✅ Spot in rotazione CON LETTERE
                g.DrawString(
                    LanguageManager.Get("PDFPreview.RotatingSpots", "Spot in rotazione: "),
                    fontBold,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                yPos += 20;

                foreach (var spot in _spots)
                {
                    string letter = _spotLetters[spot.ID];
                    g.DrawString($"   [{letter}] {spot.SpotTitle} ({spot.Duration}s)", fontSmall, brushBlack, leftMargin + 20, yPos);
                    yPos += 18;
                }

                yPos += 10;
                g.DrawLine(penLine, leftMargin, yPos, rightMargin, yPos);
                yPos += 20;

                // ═══════════════════════════════════════════════════════════
                // 4.PROGRAMMAZIONE GIORNALIERA
                // ═══════════════════════════════════════════════════════════

                g.DrawString(
                    LanguageManager.Get("PDFPreview.ScheduleTitle", "PROGRAMMAZIONE PUBBLICITARIA"),
                    fontHeader,
                    brushBlack,
                    leftMargin,
                    yPos
                );
                yPos += 30;

                // Header tabella
                float colDateWidth = 140;
                float colSlotsWidth = pageWidth - colDateWidth - 80;
                float colPassesWidth = 80;

                RectangleF headerRect = new RectangleF(leftMargin, yPos, pageWidth, 25);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 123, 255)), headerRect);

                g.DrawString(
                    LanguageManager.Get("PDFPreview.ColDate", "Data"),
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    Brushes.White,
                    leftMargin + 5,
                    yPos + 5
                );
                g.DrawString(
                    LanguageManager.Get("PDFPreview.ColScheduledTimes", "Orari Programmati"),
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    Brushes.White,
                    leftMargin + colDateWidth + 5,
                    yPos + 5
                );
                g.DrawString(
                    LanguageManager.Get("PDFPreview.ColPasses", "Pass."),
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    Brushes.White,
                    rightMargin - 70,
                    yPos + 5
                );

                yPos += 30;

                int startIndex = _currentPage * ITEMS_PER_PAGE;
                int endIndex = Math.Min(startIndex + ITEMS_PER_PAGE, _dailySchedules.Count);

                bool alternate = false;
                for (int i = startIndex; i < endIndex; i++)
                {
                    var day = _dailySchedules[i];

                    if (yPos > e.MarginBounds.Bottom - 60)
                        break;

                    if (alternate)
                    {
                        RectangleF rowRect = new RectangleF(leftMargin, yPos, pageWidth, 35);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), rowRect);
                    }

                    // Data + Giorno abbreviato
                    string dateWithDay = $"{day.Date:dd/MM/yyyy} {GetShortDayName(day.Date)}";
                    g.DrawString(dateWithDay, fontSmall, brushBlack, leftMargin + 5, yPos + 5);

                    // ✅ Orari CON LETTERE (08:30 A, 10:00 B, 14:00 C...)
                    var formattedSlotsWithLetters = new List<string>();
                    for (int slotIdx = 0; slotIdx < day.TimeSlots.Count; slotIdx++)
                    {
                        string time = FormatTimeSlot(day.TimeSlots[slotIdx]);
                        string letter = GetLetterForSlot(i, slotIdx);
                        formattedSlotsWithLetters.Add($"{time} {letter}");
                    }

                    string timeSlotsStr = string.Join(", ", formattedSlotsWithLetters);
                    SizeF textSize = g.MeasureString(timeSlotsStr, fontSmall);

                    if (textSize.Width > colSlotsWidth - 10)
                    {
                        // Dividi su 2 righe
                        int halfCount = formattedSlotsWithLetters.Count / 2;
                        string line1 = string.Join(", ", formattedSlotsWithLetters.Take(halfCount));
                        string line2 = string.Join(", ", formattedSlotsWithLetters.Skip(halfCount));

                        g.DrawString(line1, fontSmall, brushBlack, leftMargin + colDateWidth + 5, yPos + 2);
                        g.DrawString(line2, fontSmall, brushBlack, leftMargin + colDateWidth + 5, yPos + 17);
                    }
                    else
                    {
                        g.DrawString(timeSlotsStr, fontSmall, brushBlack, leftMargin + colDateWidth + 5, yPos + 10);
                    }

                    // Numero passaggi
                    string passCountStr = day.TimeSlots.Count.ToString();
                    g.DrawString(passCountStr, fontBold, brushBlack, rightMargin - 60, yPos + 10);

                    yPos += 35;
                    g.DrawLine(penLine, leftMargin, yPos, rightMargin, yPos);

                    alternate = !alternate;
                }

                // ═══════════════════════════════════════════════════════════
                // FOOTER
                // ═══════════════════════════════════════════════════════════

                yPos = e.MarginBounds.Bottom + 20;
                string footerText = string.Format(
                    LanguageManager.Get("PDFPreview.FooterText", "Documento generato il {0} - Pagina {1}"),
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    _currentPage + 1
                );
                SizeF footerSize = g.MeasureString(footerText, fontSmall);
                g.DrawString(footerText, fontSmall, brushGray, (pageWidth - footerSize.Width) / 2 + leftMargin, yPos);

                _currentPage++;
                e.HasMorePages = (_currentPage * ITEMS_PER_PAGE < _dailySchedules.Count);

                if (!e.HasMorePages)
                    _currentPage = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDFPreview] Errore stampa pagina: {ex.Message}");
            }
        }

        private void btnSavePDF_Click(object sender, EventArgs e)
        {
            try
            {
                string suggestedFileName = $"{_client.ClientName} - {_campaign.CampaignName} - {_campaign.StartDate:yyyy-MM-dd}.pdf";

                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    suggestedFileName = suggestedFileName.Replace(c, '_');
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = LanguageManager.Get("PDFPreview.PDFFilter", "PDF Files|*.pdf"),
                    Title = LanguageManager.Get("PDFPreview.SaveTitle", "Salva Programmazione Campagna"),
                    FileName = suggestedFileName,
                    DefaultExt = "pdf",
                    AddExtension = true
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PrintDialog printDialog = new PrintDialog
                    {
                        Document = _printDocument,
                        UseEXDialog = true
                    };

                    bool pdfPrinterFound = false;
                    foreach (string printer in PrinterSettings.InstalledPrinters)
                    {
                        if (printer.Contains("PDF") || printer.Contains("pdf") || printer.Contains("Microsoft Print to PDF"))
                        {
                            _printDocument.PrinterSettings.PrinterName = printer;
                            _printDocument.PrinterSettings.PrintToFile = true;
                            _printDocument.PrinterSettings.PrintFileName = saveDialog.FileName;
                            pdfPrinterFound = true;
                            break;
                        }
                    }

                    if (!pdfPrinterFound)
                    {
                        MessageBox.Show(
                            LanguageManager.Get("PDFPreview.NoPDFPrinter", "Nessuna stampante PDF trovata!\n\nInstalla 'Microsoft Print to PDF' o una stampante PDF virtuale."),
                            LanguageManager.Get("Common.Warning", "Attenzione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    try
                    {
                        _currentPage = 0;
                        _printDocument.Print();

                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("PDFPreview.SaveSuccess", "✅ PDF salvato con successo!\n\nPercorso:\n{0}"),
                                saveDialog.FileName
                            ),
                            LanguageManager.Get("Common.Success", "Successo"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        var openResult = MessageBox.Show(
                            LanguageManager.Get("PDFPreview.OpenPDF", "Vuoi aprire il PDF appena creato?"),
                            LanguageManager.Get("PDFPreview.OpenPDFTitle", "Apri PDF"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (openResult == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveDialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"{LanguageManager.Get("PDFPreview.SaveError", "Errore durante il salvataggio")}:\n{ex.Message}\n\n{LanguageManager.Get("PDFPreview.SaveErrorHint", "Assicurati di avere una stampante PDF installata.")}",
                            LanguageManager.Get("Common.Error", "Errore"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("PDFPreview.SavePDFError", "Errore salvataggio PDF")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog
                {
                    Document = _printDocument
                };

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentPage = 0;
                    _printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("PDFPreview.PrintError", "Errore stampa")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_previewControl.Zoom < 2.0)
            {
                _previewControl.Zoom += 0.25;
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_previewControl.Zoom > 0.25)
            {
                _previewControl.Zoom -= 0.25;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}