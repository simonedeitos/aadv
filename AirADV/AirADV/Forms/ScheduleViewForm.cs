using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class ScheduleViewForm : Form
    {
        private DateTime _currentDate = DateTime.Now.Date;
        private int _stationID = 0;
        private Dictionary<string, List<DbcManager.Schedule>> _slotSchedules = new Dictionary<string, List<DbcManager.Schedule>>();
        private bool _isDirty = false;
        private bool _showClientName = true;

        // Cache dati
        private List<DbcManager.Client> _clients;
        private List<DbcManager.Spot> _spots;
        private List<DbcManager.Campaign> _campaigns;
        private List<DbcManager.Category> _categories;
        private List<DbcManager.TimeSlot> _timeSlots;

        // Calendario popup
        private MonthCalendar monthCalendar;
        private Form calendarPopup;

        // Drag & Drop
        private DataGridViewCell _dragSourceCell;

        public ScheduleViewForm()
        {
            InitializeComponent();
            this.Load += ScheduleViewForm_Load;
        }

        public ScheduleViewForm(int stationID, DateTime date) : this()
        {
            _stationID = stationID;
            _currentDate = date;
        }

        private void ScheduleViewForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_stationID == 0)
                    _stationID = ConfigManager.CurrentStationID;

                if (_stationID == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ScheduleView.NoStationSelected", "Seleziona prima un'emittente! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    this.Close();
                    return;
                }

                LoadCacheData();
                InitializeCalendarPopup();
                SetupGrid();
                ApplyLanguage();
                LoadSchedule(_currentDate);

                LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.InitializationError", "Errore inizializzazione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            Console.WriteLine("[ScheduleView] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
        }

        /// <summary>
        /// Inizializza calendario popup
        /// </summary>
        private void InitializeCalendarPopup()
        {
            monthCalendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                ShowToday = true,
                ShowTodayCircle = true
            };

            monthCalendar.DateSelected += MonthCalendar_DateSelected;

            calendarPopup = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Size = monthCalendar.Size,
                ShowInTaskbar = false,
                TopMost = true
            };

            calendarPopup.Controls.Add(monthCalendar);
            calendarPopup.Deactivate += (s, ev) => calendarPopup.Hide();
        }

        /// <summary>
        /// Mostra calendario al click sulla data
        /// </summary>
        private void lblDate_Click(object sender, EventArgs e)
        {
            Point lblLocation = lblDate.PointToScreen(Point.Empty);
            calendarPopup.Location = new Point(lblLocation.X, lblLocation.Y + lblDate.Height + 5);

            monthCalendar.SetDate(_currentDate);

            calendarPopup.Show();
            calendarPopup.BringToFront();
        }

        private void LoadCacheData()
        {
            _clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
            _spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
            _campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
            _categories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc");
            _timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                .Where(t => t.StationID == _stationID && t.IsActive)
                .OrderBy(t => t.SlotTime)
                .ToList();
        }

        /// <summary>
        /// ✅ AGGIORNATO: Setup grid senza colonne spot fisse
        /// </summary>
        private void SetupGrid()
        {
            dgvSchedule.AllowUserToAddRows = false;
            dgvSchedule.AllowUserToDeleteRows = false;
            dgvSchedule.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSchedule.RowHeadersWidth = 10;
            dgvSchedule.ColumnHeadersHeight = 35;
            dgvSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSchedule.AllowDrop = true;
            dgvSchedule.ReadOnly = false;

            dgvSchedule.Columns.Clear();

            // ✅ Colonna Ore (bloccata durante scroll)
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOre",
                HeaderText = LanguageManager.Get("ScheduleView.ColTime", "Ore"),
                Width = 80,
                ReadOnly = true,
                Frozen = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    BackColor = Color.FromArgb(240, 240, 240),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // Colonna N.(bloccata)
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colN",
                HeaderText = LanguageManager.Get("ScheduleView.ColN", "N."),
                Width = 40,
                ReadOnly = true,
                Frozen = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(240, 240, 240)
                }
            });

            // Colonna Durata (bloccata)
            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDurata",
                HeaderText = LanguageManager.Get("ScheduleView.ColDuration", "Durata"),
                Width = 90,
                ReadOnly = true,
                Frozen = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(240, 240, 240),
                    Font = new Font("Consolas", 9F)
                }
            });

            // ❌ NON creare colonne spot qui - le creiamo dinamicamente in PopulateGrid()

            dgvSchedule.CellFormatting += DgvSchedule_CellFormatting;
            dgvSchedule.CellMouseDown += DgvSchedule_CellMouseDown;
            dgvSchedule.DragOver += DgvSchedule_DragOver;
            dgvSchedule.DragDrop += DgvSchedule_DragDrop;
        }

        /// <summary>
        /// ✅ NUOVO: Crea colonne spot dinamiche in base al numero massimo necessario
        /// </summary>
        private void EnsureSpotColumns(int maxSpotsNeeded)
        {
            // Conta colonne spot esistenti (dopo le 3 fisse)
            int currentSpotColumns = dgvSchedule.Columns.Count - 3;

            // Se abbiamo già abbastanza colonne, esci
            if (currentSpotColumns >= maxSpotsNeeded)
                return;

            // Aggiungi colonne mancanti
            for (int i = currentSpotColumns + 1; i <= maxSpotsNeeded; i++)
            {
                dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = $"colSpot{i}",
                    HeaderText = i.ToString(),
                    Width = 180,
                    ReadOnly = false
                });

                Console.WriteLine($"[ScheduleView] Aggiunta colonna spot {i}");
            }
        }

        private void LoadSchedule(DateTime date)
        {
            try
            {
                _currentDate = date;

                // ✅ Formato data localizzato
                lblDate.Text = $"📅 {_currentDate: dddd dd MMMM yyyy}";

                var scheduleEngine = new ScheduleEngine();
                var schedules = scheduleEngine.LoadSchedule(_stationID, _currentDate);

                var spotSchedules = schedules.Where(s => s.FileType == "SPOT").ToList();

                _slotSchedules.Clear();
                var slotGroups = spotSchedules.GroupBy(s => s.SlotTime);

                foreach (var group in slotGroups)
                {
                    _slotSchedules[group.Key] = group.OrderBy(s => s.SequenceOrder).ToList();
                }

                PopulateGrid();
                UpdateStatus();
                _isDirty = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.LoadError", "Errore caricamento")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// ✅ AGGIORNATO:  Popola grid con colonne dinamiche
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                // ✅ STEP 1: Calcola numero massimo di spot in un singolo slot
                int maxSpotsInAnySlot = 0;

                foreach (var timeSlot in _timeSlots)
                {
                    var slotsSpots = _slotSchedules.ContainsKey(timeSlot.SlotTime)
                        ? _slotSchedules[timeSlot.SlotTime]
                        : new List<DbcManager.Schedule>();

                    if (slotsSpots.Count > maxSpotsInAnySlot)
                    {
                        maxSpotsInAnySlot = slotsSpots.Count;
                    }
                }

                // ✅ STEP 2: Assicura minimo 6 colonne, massimo illimitato
                int columnsNeeded = Math.Max(6, maxSpotsInAnySlot);
                EnsureSpotColumns(columnsNeeded);

                Console.WriteLine($"[ScheduleView] Colonne spot:  {columnsNeeded} (max slot: {maxSpotsInAnySlot})");

                // ✅ STEP 3: Popola grid
                dgvSchedule.Rows.Clear();

                foreach (var timeSlot in _timeSlots)
                {
                    int rowIndex = dgvSchedule.Rows.Add();
                    var row = dgvSchedule.Rows[rowIndex];

                    // Ora - Formato HH:mm
                    row.Cells["colOre"].Value = timeSlot.SlotTime.Substring(0, 5);

                    var slotsSpots = _slotSchedules.ContainsKey(timeSlot.SlotTime)
                        ? _slotSchedules[timeSlot.SlotTime]
                        : new List<DbcManager.Schedule>();

                    // N.spot
                    row.Cells["colN"].Value = slotsSpots.Count;

                    // Durata - Formato mm:ss
                    int totalDuration = slotsSpots.Sum(s => s.Duration);
                    row.Cells["colDurata"].Value = FormatDuration(totalDuration);

                    // ✅ Popola TUTTE le colonne spot disponibili
                    int spotColumnCount = dgvSchedule.Columns.Count - 3; // Escludi le 3 fisse

                    for (int i = 0; i < spotColumnCount; i++)
                    {
                        string colName = $"colSpot{i + 1}";

                        if (i < slotsSpots.Count)
                        {
                            var schedule = slotsSpots[i];
                            row.Cells[colName].Value = GetSpotDisplayText(schedule);
                            row.Cells[colName].Tag = schedule;
                        }
                        else
                        {
                            row.Cells[colName].Value = "";
                            row.Cells[colName].Tag = null;
                        }
                    }

                    row.Tag = timeSlot;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleView] ❌ Errore PopulateGrid: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.LoadError", "Errore caricamento")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string GetSpotDisplayText(DbcManager.Schedule schedule)
        {
            var campaign = _campaigns.FirstOrDefault(c => c.ID == schedule.CampaignID);
            if (campaign == null) return "N/A";

            if (_showClientName)
            {
                var client = _clients.FirstOrDefault(c => c.ID == schedule.ClientID);
                return client?.ClientName ?? "N/A";
            }
            else
            {
                var spot = _spots.FirstOrDefault(s => s.ID == schedule.SpotID);
                return spot?.SpotTitle ?? "N/A";
            }
        }

        /// <summary>
        /// ✅ AGGIORNATO:  Colonne spot iniziano dalla 4° (indice 3)
        /// </summary>
        private void DgvSchedule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 3 && e.RowIndex >= 0)
            {
                var cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Tag is DbcManager.Schedule schedule)
                {
                    var campaign = _campaigns.FirstOrDefault(c => c.ID == schedule.CampaignID);
                    if (campaign != null)
                    {
                        var category = _categories.FirstOrDefault(cat => cat.ID == campaign.CategoryID);
                        if (category != null)
                        {
                            try
                            {
                                Color bgColor = ColorTranslator.FromHtml(category.Color);
                                e.CellStyle.BackColor = bgColor;
                                e.CellStyle.SelectionBackColor = ControlPaint.Dark(bgColor, 0.2f);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        // DRAG & DROP
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// ✅ AGGIORNATO:  Colonne spot iniziano dalla 4° (indice 3)
        /// </summary>
        private void DgvSchedule_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0 && e.ColumnIndex >= 3)
            {
                var cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Tag is DbcManager.Schedule)
                {
                    _dragSourceCell = cell;
                    dgvSchedule.DoDragDrop(cell.Tag, DragDropEffects.Move);
                }
            }
        }

        private void DgvSchedule_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DbcManager.Schedule)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void DgvSchedule_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DbcManager.Schedule)))
            {
                var schedule = e.Data.GetData(typeof(DbcManager.Schedule)) as DbcManager.Schedule;

                Point clientPoint = dgvSchedule.PointToClient(new Point(e.X, e.Y));
                var hitTest = dgvSchedule.HitTest(clientPoint.X, clientPoint.Y);

                if (hitTest.RowIndex >= 0 && hitTest.ColumnIndex >= 3)
                {
                    var targetRow = dgvSchedule.Rows[hitTest.RowIndex];
                    var targetTimeSlot = targetRow.Tag as DbcManager.TimeSlot;

                    if (targetTimeSlot != null)
                    {
                        string oldSlot = schedule.SlotTime;
                        string newSlot = targetTimeSlot.SlotTime;

                        if (_slotSchedules.ContainsKey(oldSlot))
                        {
                            _slotSchedules[oldSlot].Remove(schedule);
                        }

                        schedule.SlotTime = newSlot;

                        if (!_slotSchedules.ContainsKey(newSlot))
                        {
                            _slotSchedules[newSlot] = new List<DbcManager.Schedule>();
                        }

                        _slotSchedules[newSlot].Add(schedule);

                        // Riordina sequenza
                        for (int i = 0; i < _slotSchedules[newSlot].Count; i++)
                        {
                            _slotSchedules[newSlot][i].SequenceOrder = i + 1;
                        }

                        _isDirty = true;
                        PopulateGrid();

                        Console.WriteLine($"[ScheduleView] Spot spostato da {oldSlot} a {newSlot}");
                    }
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        // EVENTI BOTTONI
        // ═══════════════════════════════════════════════════════════

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            LoadSchedule(e.Start);
            calendarPopup.Hide();
        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            LoadSchedule(_currentDate.AddDays(-1));
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            LoadSchedule(_currentDate.AddDays(1));
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            LoadSchedule(DateTime.Now.Date);
        }

        private void btnToggleView_Click(object sender, EventArgs e)
        {
            _showClientName = !_showClientName;

            // ✅ Aggiorna testo bottone tradotto
            btnToggleView.Text = _showClientName
                ? LanguageManager.Get("ScheduleView.BtnToggleClient", "👤 Cliente")
                : LanguageManager.Get("ScheduleView.BtnToggleSpot", "🎬 Spot");

            PopulateGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isDirty)
                {
                    MessageBox.Show(
                        LanguageManager.Get("Messages.NoChanges", "Nessuna modifica da salvare."),
                        LanguageManager.Get("Common.Information", "Info"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");

                allSchedules.RemoveAll(s =>
                    s.StationID == _stationID &&
                    s.ScheduleDate.Date == _currentDate.Date &&
                    s.FileType == "SPOT");

                foreach (var kvp in _slotSchedules)
                {
                    allSchedules.AddRange(kvp.Value);
                }

                bool success = DbcManager.Save("ADV_Schedule.dbc", allSchedules);

                if (success)
                {
                    try
                    {
                        bool exported = AirDirectorExportService.ExportFullSchedule(
                            _stationID,
                            _currentDate,
                            _currentDate.AddDays(1)
                        );

                        if (exported)
                        {
                            Console.WriteLine("[ScheduleView] ✅ Export AirDirector aggiornato");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ScheduleView] ⚠️ Errore export: {ex.Message}");
                    }

                    _isDirty = false;
                    MessageBox.Show(
                        LanguageManager.Get("ScheduleView.SaveSuccess", "✅ Palinsesto salvato! "),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.SaveError", "Errore salvataggio")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSchedule(_currentDate);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Get("ScheduleView.ExportPdfWip", "Funzione export PDF in sviluppo"),
                LanguageManager.Get("Common.Information", "Info"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ═══════════════════════════════════════════════════════════
        // HELPER
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Formatta durata come mm:ss
        /// </summary>
        private string FormatDuration(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;
            return $"{mins: 00}:{secs:00}";
        }

        private void UpdateStatus()
        {
            int totalSlots = _slotSchedules.Count;
            int totalSpots = _slotSchedules.Values.Sum(list => list.Count);

            lblStatus.Text = string.Format(
                LanguageManager.Get("ScheduleView.StatusLabel", "Slot attivi: {0} | Spot totali: {1}"),
                totalSlots,
                totalSpots
            );
        }

        /// <summary>
        /// ✅ COMPLETO: Tutte le traduzioni
        /// </summary>
        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[ScheduleView] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                // ✅ Titolo
                this.Text = LanguageManager.Get("ScheduleView.WindowTitle", "📅 Palinsesto Giornaliero");
                lblTitle.Text = LanguageManager.Get("ScheduleView.Title", "📅 PALINSESTO GIORNALIERO");

                // ✅ Label data (aggiorna con data corrente)
                lblDate.Text = $"📅 {_currentDate: dddd dd MMMM yyyy}";

                // ✅ Bottoni navigazione
                btnPrevDay.Text = LanguageManager.Get("ScheduleView.BtnPrevDay", "◀");
                btnNextDay.Text = LanguageManager.Get("ScheduleView.BtnNextDay", "▶");
                btnToday.Text = LanguageManager.Get("ScheduleView.BtnToday", "📍 Oggi");

                // ✅ Bottone toggle vista
                btnToggleView.Text = _showClientName
                    ? LanguageManager.Get("ScheduleView.BtnToggleClient", "👤 Cliente")
                    : LanguageManager.Get("ScheduleView.BtnToggleSpot", "🎬 Spot");

                // ✅ Bottoni azioni
                btnSave.Text = LanguageManager.Get("Common.Save", "💾 Salva");
                btnRefresh.Text = LanguageManager.Get("Common.Refresh", "🔄 Aggiorna");
                btnExportPdf.Text = LanguageManager.Get("ScheduleView.BtnExportPdf", "📄 Esporta PDF");

                // ✅ Colonne DataGridView (solo le 3 fisse)
                if (dgvSchedule.Columns.Contains("colOre"))
                    dgvSchedule.Columns["colOre"].HeaderText = LanguageManager.Get("ScheduleView.ColTime", "Ore");

                if (dgvSchedule.Columns.Contains("colN"))
                    dgvSchedule.Columns["colN"].HeaderText = LanguageManager.Get("ScheduleView.ColN", "N.");

                if (dgvSchedule.Columns.Contains("colDurata"))
                    dgvSchedule.Columns["colDurata"].HeaderText = LanguageManager.Get("ScheduleView.ColDuration", "Durata");

                // ✅ Refresh status
                UpdateStatus();

                Console.WriteLine("[ScheduleView] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleView] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_isDirty)
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.UnsavedChangesSave", "Ci sono modifiche non salvate.Vuoi salvarle?"),
                    LanguageManager.Get("Messages.UnsavedChangesTitle", "Modifiche Non Salvate"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    btnSave_Click(null, null);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            // ✅ Cleanup
            calendarPopup?.Close();
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}