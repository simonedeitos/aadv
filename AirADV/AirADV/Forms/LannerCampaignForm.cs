using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class LannerCampaignForm : Form
    {
        private int _stationID;
        private List<LannerCampaign> _campaigns = new List<LannerCampaign>();
        private LannerCampaign _selectedCampaign = null;
        private bool _isEditing = false;
        private string _resolvedXmlPath = null;

        // ═══════════════════════════════════════════════════════════
        // DATA MODEL
        // ═══════════════════════════════════════════════════════════

        public enum CampaignStatus
        {
            Past,
            Active,
            Future
        }

        [Serializable]
        public class LannerCampaign
        {
            public int ID { get; set; }
            public string ClientName { get; set; } = "";
            public string CampaignName { get; set; } = "";
            public DateTime StartDate { get; set; } = DateTime.Now.Date;
            public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(30);
            public int DailySlots { get; set; } = 1;
            public List<string> SlotTimes { get; set; } = new List<string>();
            public int DurationMinutes { get; set; } = 5;
            public string ImagePath { get; set; } = "";
            public DateTime CreatedDate { get; set; } = DateTime.Now;

            [XmlIgnore]
            public CampaignStatus Status
            {
                get
                {
                    DateTime today = DateTime.Now.Date;
                    if (EndDate.Date < today) return CampaignStatus.Past;
                    if (StartDate.Date <= today && EndDate.Date >= today) return CampaignStatus.Active;
                    return CampaignStatus.Future;
                }
            }

            [XmlIgnore]
            public string StatusText
            {
                get
                {
                    switch (Status)
                    {
                        case CampaignStatus.Past: return "PASSATA";
                        case CampaignStatus.Active: return "IN CORSO";
                        case CampaignStatus.Future: return "FUTURA";
                        default: return "";
                    }
                }
            }

            [XmlIgnore]
            public string SlotTimesDisplay => string.Join(", ", SlotTimes);
        }

        [Serializable]
        [XmlRoot("LannerTVCampaigns")]
        public class LannerCampaignData
        {
            public List<LannerCampaign> Campaigns { get; set; } = new List<LannerCampaign>();
        }

        // ═══════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ═══════════════════════════════════════════════════════════

        public LannerCampaignForm(int stationID)
        {
            InitializeComponent();
            _stationID = stationID;
            this.Load += LannerCampaignForm_Load;
        }

        private void LannerCampaignForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ PRIMA DI TUTTO: risolvi il path XML
                ResolveXmlPath();

                SetupCampaignsGrid();
                SetupContextMenu();
                EnsureXmlExists();
                LoadCampaigns();
                ClearEditPanel();
                SetEditMode(false);
                ApplyLanguage();

                LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;

                Console.WriteLine("[LannerTV] ✅ Form caricato correttamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore Load: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show(
                    $"Errore inizializzazione:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        // ═══════════════════════════════════════════════════════════
        // XML PATH — Risolto una volta sola all'avvio
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Calcola e salva il percorso XML definitivo.
        /// Chiamato una volta sola in Load.
        /// </summary>
        private void ResolveXmlPath()
        {
            Console.WriteLine($"[LannerTV] 🔍 ResolveXmlPath per stationID={_stationID}");

            try
            {
                var allStations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc");
                Console.WriteLine($"[LannerTV] 📋 Stazioni nel config: {allStations.Count}");

                foreach (var s in allStations)
                {
                    Console.WriteLine($"[LannerTV]   → ID={s.StationID} | {s.StationName} | DB={s.DatabasePath}");
                }

                // Cerca per StationID
                var station = allStations.FirstOrDefault(s => s.StationID == _stationID);

                if (station != null && !string.IsNullOrWhiteSpace(station.DatabasePath))
                {
                    _resolvedXmlPath = Path.Combine(station.DatabasePath, "lannertv.xml");
                    Console.WriteLine($"[LannerTV] ✅ Path risolto da stazione: {_resolvedXmlPath}");
                    return;
                }

                Console.WriteLine($"[LannerTV] ⚠️ StationID={_stationID} non trovato o DatabasePath vuoto");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore ricerca stazione: {ex.Message}");
            }

            // Fallback 1: ConfigManager.DATABASE_PATH
            try
            {
                string configPath = ConfigManager.DATABASE_PATH;
                if (!string.IsNullOrWhiteSpace(configPath))
                {
                    _resolvedXmlPath = Path.Combine(configPath, "lannertv.xml");
                    Console.WriteLine($"[LannerTV] ⚠️ Fallback ConfigManager: {_resolvedXmlPath}");
                    return;
                }
            }
            catch { }

            // Fallback 2: AppData
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AirADV", "Data"
            );
            _resolvedXmlPath = Path.Combine(appDataPath, "lannertv.xml");
            Console.WriteLine($"[LannerTV] ⚠️ Fallback AppData: {_resolvedXmlPath}");
        }

        /// <summary>
        /// Restituisce il path XML già risolto.
        /// </summary>
        private string GetXmlPath()
        {
            return _resolvedXmlPath;
        }

        // ═══════════════════════════════════════════════════════════
        // XML PERSISTENCE
        // ═══════════════════════════════════════════════════════════

        private void EnsureXmlExists()
        {
            try
            {
                string xmlPath = GetXmlPath();

                if (string.IsNullOrEmpty(xmlPath))
                {
                    Console.WriteLine("[LannerTV] ❌ xmlPath è null/vuoto!");
                    return;
                }

                string dir = Path.GetDirectoryName(xmlPath);

                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine($"[LannerTV] 📁 Directory creata: {dir}");
                }

                if (!File.Exists(xmlPath))
                {
                    var emptyData = new LannerCampaignData { Campaigns = new List<LannerCampaign>() };
                    var serializer = new XmlSerializer(typeof(LannerCampaignData));

                    using (var writer = new StreamWriter(xmlPath))
                    {
                        serializer.Serialize(writer, emptyData);
                    }

                    Console.WriteLine($"[LannerTV] 📄 File XML creato: {xmlPath}");
                }
                else
                {
                    var fi = new FileInfo(xmlPath);
                    Console.WriteLine($"[LannerTV] ✅ File XML esistente: {xmlPath} ({fi.Length} bytes)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore EnsureXmlExists: {ex.Message}");
                MessageBox.Show(
                    $"Errore creazione file Lanner TV:\n{ex.Message}\n\nPercorso: {_resolvedXmlPath}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadCampaigns()
        {
            try
            {
                string xmlPath = GetXmlPath();
                Console.WriteLine($"[LannerTV] 📂 LoadCampaigns da: {xmlPath}");

                if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
                {
                    Console.WriteLine($"[LannerTV] ⚠️ File non trovato, lista vuota");
                    _campaigns = new List<LannerCampaign>();
                    RefreshGrid();
                    UpdateCampaignCount();
                    return;
                }

                // Debug: leggi contenuto
                string rawXml = File.ReadAllText(xmlPath);
                Console.WriteLine($"[LannerTV] 📄 XML size: {rawXml.Length} chars");
                if (rawXml.Length < 2000)
                    Console.WriteLine($"[LannerTV] 📄 Contenuto:\n{rawXml}");

                // Deserializza
                var serializer = new XmlSerializer(typeof(LannerCampaignData));

                using (var stringReader = new StringReader(rawXml))
                {
                    var data = (LannerCampaignData)serializer.Deserialize(stringReader);

                    if (data != null && data.Campaigns != null)
                    {
                        _campaigns = data.Campaigns;
                    }
                    else
                    {
                        Console.WriteLine("[LannerTV] ⚠️ Deserializzazione: data o Campaigns è null");
                        _campaigns = new List<LannerCampaign>();
                    }
                }

                Console.WriteLine($"[LannerTV] ✅ Caricate {_campaigns.Count} campagne:");
                foreach (var c in _campaigns)
                {
                    Console.WriteLine($"[LannerTV]   → ID={c.ID} | {c.ClientName} | {c.CampaignName} | {c.StatusText} | {c.StartDate:dd/MM/yyyy}-{c.EndDate:dd/MM/yyyy}");
                }

                RefreshGrid();
                UpdateCampaignCount();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore LoadCampaigns: {ex.Message}");
                Console.WriteLine($"[LannerTV] StackTrace: {ex.StackTrace}");

                var result = MessageBox.Show(
                    $"Errore lettura campagne Lanner TV:\n{ex.Message}\n\nPercorso: {_resolvedXmlPath}\n\nVuoi creare un nuovo file vuoto?",
                    "Errore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (File.Exists(_resolvedXmlPath))
                        {
                            // Backup del file corrotto
                            string backup = _resolvedXmlPath + ".bak." + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            File.Copy(_resolvedXmlPath, backup, true);
                            Console.WriteLine($"[LannerTV] 📋 Backup creato: {backup}");
                        }
                        File.Delete(_resolvedXmlPath);
                        EnsureXmlExists();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"[LannerTV] ❌ Errore ricreazione: {ex2.Message}");
                    }
                }

                _campaigns = new List<LannerCampaign>();
                RefreshGrid();
                UpdateCampaignCount();
            }
        }

        private void SaveCampaigns()
        {
            try
            {
                string xmlPath = GetXmlPath();

                if (string.IsNullOrEmpty(xmlPath))
                {
                    MessageBox.Show("Percorso di salvataggio non disponibile!",
                        "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string dir = Path.GetDirectoryName(xmlPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var data = new LannerCampaignData { Campaigns = _campaigns };
                var serializer = new XmlSerializer(typeof(LannerCampaignData));

                using (var writer = new StreamWriter(xmlPath))
                {
                    serializer.Serialize(writer, data);
                }

                var fi = new FileInfo(xmlPath);
                Console.WriteLine($"[LannerTV] ✅ Salvate {_campaigns.Count} campagne in {xmlPath} ({fi.Length} bytes)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore SaveCampaigns: {ex.Message}");
                MessageBox.Show(
                    $"Errore salvataggio:\n{ex.Message}\n\nPercorso: {_resolvedXmlPath}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // GRID SETUP
        // ═══════════════════════════════════════════════════════════

        private void SetupCampaignsGrid()
        {
            dgvCampaigns.AutoGenerateColumns = false;
            dgvCampaigns.AllowUserToAddRows = false;
            dgvCampaigns.AllowUserToDeleteRows = false;
            dgvCampaigns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCampaigns.MultiSelect = false;
            dgvCampaigns.RowHeadersVisible = false;
            dgvCampaigns.ReadOnly = true;
            dgvCampaigns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCampaigns.RowTemplate.Height = 35;

            dgvCampaigns.Columns.Clear();

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Stato",
                FillWeight = 10
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClient",
                HeaderText = "Cliente",
                FillWeight = 18
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCampaign",
                HeaderText = "Campagna",
                FillWeight = 18
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = "Inizio",
                FillWeight = 10
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = "Fine",
                FillWeight = 10
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSlots",
                HeaderText = "Slot/Giorno",
                FillWeight = 8
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDuration",
                HeaderText = "Durata (min)",
                FillWeight = 8
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTimes",
                HeaderText = "Orari",
                FillWeight = 20
            });

            dgvCampaigns.Columns.Add(new DataGridViewImageColumn
            {
                Name = "colHasImage",
                HeaderText = "Img",
                FillWeight = 5,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });

            dgvCampaigns.CellDoubleClick += DgvCampaigns_CellDoubleClick;
            dgvCampaigns.SelectionChanged += DgvCampaigns_SelectionChanged;
            dgvCampaigns.CellFormatting += DgvCampaigns_CellFormatting;
        }

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var mnuDuplicate = new ToolStripMenuItem("📋 Duplica Campagna");
            mnuDuplicate.Click += MnuDuplicate_Click;

            var mnuEndToday = new ToolStripMenuItem("⏹️ Termina Oggi");
            mnuEndToday.Click += MnuEndToday_Click;

            var mnuDelete = new ToolStripMenuItem("🗑️ Elimina Campagna");
            mnuDelete.Click += MnuDelete_Click;

            contextMenu.Items.Add(mnuDuplicate);
            contextMenu.Items.Add(mnuEndToday);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(mnuDelete);

            contextMenu.Opening += ContextMenu_Opening;

            dgvCampaigns.ContextMenuStrip = contextMenu;
        }

        // ═══════════════════════════════════════════════════════════
        // GRID REFRESH & FORMATTING
        // ═══════════════════════════════════════════════════════════

        private void RefreshGrid()
        {
            dgvCampaigns.Rows.Clear();

            var sorted = _campaigns
                .OrderBy(c => c.Status == CampaignStatus.Active ? 0 :
                              c.Status == CampaignStatus.Future ? 1 : 2)
                .ThenBy(c => c.StartDate)
                .ToList();

            foreach (var campaign in sorted)
            {
                int idx = dgvCampaigns.Rows.Add();
                var row = dgvCampaigns.Rows[idx];

                row.Cells["colStatus"].Value = campaign.StatusText;
                row.Cells["colClient"].Value = campaign.ClientName;
                row.Cells["colCampaign"].Value = campaign.CampaignName;
                row.Cells["colStartDate"].Value = campaign.StartDate.ToString("dd/MM/yyyy");
                row.Cells["colEndDate"].Value = campaign.EndDate.ToString("dd/MM/yyyy");
                row.Cells["colSlots"].Value = campaign.DailySlots;
                row.Cells["colDuration"].Value = campaign.DurationMinutes;
                row.Cells["colTimes"].Value = campaign.SlotTimesDisplay;

                if (!string.IsNullOrEmpty(campaign.ImagePath) && File.Exists(campaign.ImagePath))
                {
                    row.Cells["colHasImage"].Value = CreateStatusIcon(Color.FromArgb(40, 167, 69), "✓");
                }
                else
                {
                    row.Cells["colHasImage"].Value = CreateStatusIcon(Color.FromArgb(220, 53, 69), "✗");
                }

                row.Tag = campaign;
            }

            Console.WriteLine($"[LannerTV] 🔄 Grid aggiornata: {sorted.Count} righe");
        }

        private Bitmap CreateStatusIcon(Color color, string text)
        {
            var bmp = new Bitmap(20, 20);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, 2, 2, 16, 16);
                }
                using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, textBrush, new RectangleF(0, 0, 20, 20), sf);
                }
            }
            return bmp;
        }

        private void DgvCampaigns_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCampaigns.Rows.Count) return;

            var campaign = dgvCampaigns.Rows[e.RowIndex].Tag as LannerCampaign;
            if (campaign == null) return;

            Color backColor;
            Color foreColor;

            switch (campaign.Status)
            {
                case CampaignStatus.Past:
                    backColor = Color.FromArgb(255, 205, 210);
                    foreColor = Color.FromArgb(183, 28, 28);
                    break;
                case CampaignStatus.Active:
                    backColor = Color.FromArgb(200, 230, 201);
                    foreColor = Color.FromArgb(27, 94, 32);
                    break;
                case CampaignStatus.Future:
                    backColor = Color.FromArgb(187, 222, 251);
                    foreColor = Color.FromArgb(13, 71, 161);
                    break;
                default:
                    backColor = Color.White;
                    foreColor = Color.Black;
                    break;
            }

            dgvCampaigns.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
            dgvCampaigns.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
            dgvCampaigns.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = ControlPaint.Dark(backColor, 0.1f);
            dgvCampaigns.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = foreColor;
        }

        // ═══════════════════════════════════════════════════════════
        // GRID EVENTS
        // ═══════════════════════════════════════════════════════════

        private void DgvCampaigns_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCampaigns.SelectedRows.Count > 0)
            {
                _selectedCampaign = dgvCampaigns.SelectedRows[0].Tag as LannerCampaign;
                LoadImagePreview();
            }
            else
            {
                _selectedCampaign = null;
            }
        }

        private void DgvCampaigns_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var campaign = dgvCampaigns.Rows[e.RowIndex].Tag as LannerCampaign;
            if (campaign == null) return;

            if (campaign.Status == CampaignStatus.Future)
            {
                LoadCampaignToEditPanel(campaign);
                SetEditMode(true);
            }
            else if (campaign.Status == CampaignStatus.Active)
            {
                var result = MessageBox.Show(
                    $"La campagna '{campaign.CampaignName}' è in corso.\n\nVuoi terminarla oggi ({DateTime.Now:dd/MM/yyyy})?",
                    "Campagna In Corso",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    EndCampaignToday(campaign);
                }
            }
            else
            {
                MessageBox.Show(
                    "Le campagne passate non sono modificabili.",
                    "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CONTEXT MENU
        // ═══════════════════════════════════════════════════════════

        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dgvCampaigns.SelectedRows.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            var campaign = dgvCampaigns.SelectedRows[0].Tag as LannerCampaign;
            if (campaign == null)
            {
                e.Cancel = true;
                return;
            }

            var menu = dgvCampaigns.ContextMenuStrip;

            // Duplica: sempre
            menu.Items[0].Enabled = true;

            // Termina Oggi: solo In Corso
            menu.Items[1].Enabled = (campaign.Status == CampaignStatus.Active);

            // Elimina: solo Future o Passate
            menu.Items[3].Enabled = (campaign.Status != CampaignStatus.Active);
        }

        private void MnuDuplicate_Click(object sender, EventArgs e)
        {
            if (_selectedCampaign == null) return;
            DuplicateCampaign(_selectedCampaign);
        }

        private void MnuEndToday_Click(object sender, EventArgs e)
        {
            if (_selectedCampaign == null) return;

            if (_selectedCampaign.Status != CampaignStatus.Active)
            {
                MessageBox.Show("Solo le campagne in corso possono essere terminate.",
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Terminare la campagna '{_selectedCampaign.CampaignName}' oggi ({DateTime.Now:dd/MM/yyyy})?",
                "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                EndCampaignToday(_selectedCampaign);
            }
        }

        private void MnuDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCampaign == null) return;

            if (_selectedCampaign.Status == CampaignStatus.Active)
            {
                MessageBox.Show(
                    "Impossibile eliminare una campagna in corso.\n\nUsa 'Termina Oggi' per concluderla.",
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Eliminare la campagna '{_selectedCampaign.CampaignName}'?",
                "Conferma Eliminazione",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _campaigns.RemoveAll(c => c.ID == _selectedCampaign.ID);
                _selectedCampaign = null;
                SaveCampaigns();
                RefreshGrid();
                ClearEditPanel();
                UpdateCampaignCount();
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CAMPAIGN ACTIONS
        // ═══════════════════════════════════════════════════════════

        private void EndCampaignToday(LannerCampaign campaign)
        {
            campaign.EndDate = DateTime.Now.Date;
            SaveCampaigns();
            RefreshGrid();
            UpdateCampaignCount();

            MessageBox.Show(
                $"✅ Campagna '{campaign.CampaignName}' terminata oggi.",
                "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DuplicateCampaign(LannerCampaign source)
        {
            int maxID = _campaigns.Any() ? _campaigns.Max(c => c.ID) : 0;

            var duplicate = new LannerCampaign
            {
                ID = maxID + 1,
                ClientName = source.ClientName,
                CampaignName = source.CampaignName + " (copia)",
                StartDate = DateTime.Now.Date.AddDays(1),
                EndDate = DateTime.Now.Date.AddDays(31),
                DailySlots = source.DailySlots,
                SlotTimes = new List<string>(source.SlotTimes),
                DurationMinutes = source.DurationMinutes,
                ImagePath = source.ImagePath,
                CreatedDate = DateTime.Now
            };

            _campaigns.Add(duplicate);
            SaveCampaigns();
            RefreshGrid();
            UpdateCampaignCount();

            SelectCampaignInGrid(duplicate.ID);
            LoadCampaignToEditPanel(duplicate);
            SetEditMode(true);

            MessageBox.Show(
                "✅ Campagna duplicata!\n\nImposta le nuove date.",
                "Duplicata", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ═══════════════════════════════════════════════════════════
        // EDIT PANEL
        // ═══════════════════════════════════════════════════════════

        private void SetEditMode(bool editing)
        {
            _isEditing = editing;

            // Controlli di input
            txtClientName.Enabled = editing;
            txtCampaignName.Enabled = editing;
            dtpStartDate.Enabled = editing;
            dtpEndDate.Enabled = editing;
            numDailySlots.Enabled = editing;
            numDurationMinutes.Enabled = editing;
            btnDefineSlotTimes.Enabled = editing;
            btnBrowseImage.Enabled = editing;

            // Bottoni azione
            btnSaveCampaign.Enabled = editing;
            btnCancelEdit.Enabled = editing;

            // btnNewCampaign: SEMPRE abilitato quando NON in editing
            btnNewCampaign.Enabled = !editing;

            // Sfondo visivo
            if (editing)
            {
                grpEditPanel.BackColor = Color.FromArgb(255, 255, 240);
            }
            else
            {
                grpEditPanel.BackColor = Color.White;
            }
        }

        private void ClearEditPanel()
        {
            txtClientName.Text = "";
            txtCampaignName.Text = "";
            dtpStartDate.Value = DateTime.Now.Date.AddDays(1);
            dtpEndDate.Value = DateTime.Now.Date.AddDays(31);
            numDailySlots.Value = 1;
            numDurationMinutes.Value = 5;
            txtSlotTimes.Text = "";
            txtImagePath.Text = "";
            picImagePreview.Image = null;
            _selectedCampaign = null;
        }

        private void LoadCampaignToEditPanel(LannerCampaign campaign)
        {
            _selectedCampaign = campaign;
            txtClientName.Text = campaign.ClientName;
            txtCampaignName.Text = campaign.CampaignName;
            dtpStartDate.Value = campaign.StartDate;
            dtpEndDate.Value = campaign.EndDate;
            numDailySlots.Value = Math.Max(1, campaign.DailySlots);
            numDurationMinutes.Value = Math.Max(1, campaign.DurationMinutes);
            txtSlotTimes.Text = string.Join(", ", campaign.SlotTimes);
            txtImagePath.Text = campaign.ImagePath ?? "";

            LoadImagePreview();
        }

        private void LoadImagePreview()
        {
            try
            {
                string path = _isEditing ? txtImagePath.Text : _selectedCampaign?.ImagePath;

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    picImagePreview.Image?.Dispose();
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        picImagePreview.Image = Image.FromStream(fs);
                    }
                }
                else
                {
                    picImagePreview.Image = null;
                }
            }
            catch
            {
                picImagePreview.Image = null;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // BUTTON EVENTS
        // ═══════════════════════════════════════════════════════════

        private void btnNewCampaign_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[LannerTV] 🆕 Nuova Campagna cliccato");
            _selectedCampaign = null;
            ClearEditPanel();
            SetEditMode(true);
            txtClientName.Focus();
        }

        private void btnSaveCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                // Validazioni
                if (string.IsNullOrWhiteSpace(txtClientName.Text))
                {
                    MessageBox.Show("Il nome cliente è obbligatorio!", "Attenzione",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClientName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCampaignName.Text))
                {
                    MessageBox.Show("Il nome campagna è obbligatorio!", "Attenzione",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCampaignName.Focus();
                    return;
                }

                if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)
                {
                    MessageBox.Show("La data di fine deve essere uguale o successiva alla data di inizio!",
                        "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSlotTimes.Text))
                {
                    MessageBox.Show("Devi definire almeno un orario!\n\nClicca '🕐 Definisci Orari' per impostarli.",
                        "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtImagePath.Text) || !File.Exists(txtImagePath.Text))
                {
                    var askResult = MessageBox.Show(
                        "Nessuna immagine selezionata.\n\nVuoi continuare senza immagine?",
                        "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (askResult == DialogResult.No) return;
                }

                var slotTimes = ParseSlotTimes(txtSlotTimes.Text);

                if (_selectedCampaign == null)
                {
                    // NUOVA CAMPAGNA
                    int maxID = _campaigns.Any() ? _campaigns.Max(c => c.ID) : 0;

                    var newCampaign = new LannerCampaign
                    {
                        ID = maxID + 1,
                        ClientName = txtClientName.Text.Trim(),
                        CampaignName = txtCampaignName.Text.Trim(),
                        StartDate = dtpStartDate.Value.Date,
                        EndDate = dtpEndDate.Value.Date,
                        DailySlots = (int)numDailySlots.Value,
                        DurationMinutes = (int)numDurationMinutes.Value,
                        SlotTimes = slotTimes,
                        ImagePath = txtImagePath.Text.Trim(),
                        CreatedDate = DateTime.Now
                    };

                    _campaigns.Add(newCampaign);
                    Console.WriteLine($"[LannerTV] ✅ Nuova campagna: ID={newCampaign.ID}, {newCampaign.ClientName} - {newCampaign.CampaignName}");
                }
                else
                {
                    // MODIFICA ESISTENTE
                    _selectedCampaign.ClientName = txtClientName.Text.Trim();
                    _selectedCampaign.CampaignName = txtCampaignName.Text.Trim();
                    _selectedCampaign.StartDate = dtpStartDate.Value.Date;
                    _selectedCampaign.EndDate = dtpEndDate.Value.Date;
                    _selectedCampaign.DailySlots = (int)numDailySlots.Value;
                    _selectedCampaign.DurationMinutes = (int)numDurationMinutes.Value;
                    _selectedCampaign.SlotTimes = slotTimes;
                    _selectedCampaign.ImagePath = txtImagePath.Text.Trim();
                    Console.WriteLine($"[LannerTV] ✅ Campagna modificata: ID={_selectedCampaign.ID}");
                }

                SaveCampaigns();
                RefreshGrid();
                ClearEditPanel();
                SetEditMode(false);
                UpdateCampaignCount();

                MessageBox.Show("✅ Campagna salvata!", "Successo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore salvataggio: {ex.Message}");
                MessageBox.Show($"Errore salvataggio:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearEditPanel();
            SetEditMode(false);
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Seleziona immagine per Lanner TV";
                dialog.Filter = "Immagini|*.jpg;*.jpeg;*.png;*.bmp|Tutti i file|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = dialog.FileName;
                    LoadImagePreview();
                }
            }
        }

        private void btnDefineSlotTimes_Click(object sender, EventArgs e)
        {
            var currentTimes = ParseSlotTimes(txtSlotTimes.Text);

            using (var editor = new SlotTimesEditorForm(currentTimes))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    txtSlotTimes.Text = string.Join(", ", editor.SlotTimes);

                    if (editor.SlotTimes.Count > 0)
                    {
                        numDailySlots.Value = Math.Max(1, editor.SlotTimes.Count);
                    }

                    Console.WriteLine($"[LannerTV] Orari impostati: {txtSlotTimes.Text}");
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════

        private List<string> ParseSlotTimes(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            return input.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();
        }

        private void SelectCampaignInGrid(int campaignID)
        {
            foreach (DataGridViewRow row in dgvCampaigns.Rows)
            {
                var c = row.Tag as LannerCampaign;
                if (c != null && c.ID == campaignID)
                {
                    dgvCampaigns.ClearSelection();
                    row.Selected = true;
                    dgvCampaigns.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void UpdateCampaignCount()
        {
            int active = _campaigns.Count(c => c.Status == CampaignStatus.Active);
            int future = _campaigns.Count(c => c.Status == CampaignStatus.Future);
            int past = _campaigns.Count(c => c.Status == CampaignStatus.Past);

            lblCampaignCount.Text = $"Totale: {_campaigns.Count}  |  🟢 In corso: {active}  |  🔵 Future: {future}  |  🔴 Passate: {past}";
        }

        private void ApplyLanguage()
        {
            try
            {
                this.Text = LanguageManager.Get("LannerTV.WindowTitle", "📺 Lanner TV - Programmazione");

                grpEditPanel.Text = LanguageManager.Get("LannerTV.GrpEditPanel", "📝 Campagna Lanner TV");
                lblClientNameLabel.Text = LanguageManager.Get("LannerTV.LblClientName", "Cliente:");
                lblCampaignNameLabel.Text = LanguageManager.Get("LannerTV.LblCampaignName", "Campagna:");
                lblStartDate.Text = LanguageManager.Get("LannerTV.LblStartDate", "Da:");
                lblEndDate.Text = LanguageManager.Get("LannerTV.LblEndDate", "A:");
                lblDailySlots.Text = LanguageManager.Get("LannerTV.LblDailySlots", "Slot/Giorno:");
                lblDurationMinutes.Text = LanguageManager.Get("LannerTV.LblDuration", "Durata (min):");
                lblSlotTimesLabel.Text = LanguageManager.Get("LannerTV.LblSlotTimes", "Orari:");
                lblSlotTimesHint.Text = LanguageManager.Get("LannerTV.SlotTimesHint", "Clicca 'Definisci Orari' per impostare gli orari giornalieri");
                lblImageLabel.Text = LanguageManager.Get("LannerTV.LblImage", "Immagine:");

                btnDefineSlotTimes.Text = LanguageManager.Get("LannerTV.BtnDefineSlotTimes", "🕐 Definisci Orari");
                btnBrowseImage.Text = LanguageManager.Get("LannerTV.BtnBrowse", "📁 Sfoglia...");
                btnNewCampaign.Text = LanguageManager.Get("LannerTV.BtnNew", "➕ Nuova Campagna");
                btnSaveCampaign.Text = LanguageManager.Get("Common.Save", "💾 Salva");
                btnCancelEdit.Text = LanguageManager.Get("Common.Cancel", "✖ Annulla");

                if (dgvCampaigns.Columns.Count > 0)
                {
                    dgvCampaigns.Columns["colStatus"].HeaderText = LanguageManager.Get("LannerTV.ColStatus", "Stato");
                    dgvCampaigns.Columns["colClient"].HeaderText = LanguageManager.Get("LannerTV.ColClient", "Cliente");
                    dgvCampaigns.Columns["colCampaign"].HeaderText = LanguageManager.Get("LannerTV.ColCampaign", "Campagna");
                    dgvCampaigns.Columns["colStartDate"].HeaderText = LanguageManager.Get("LannerTV.ColStartDate", "Inizio");
                    dgvCampaigns.Columns["colEndDate"].HeaderText = LanguageManager.Get("LannerTV.ColEndDate", "Fine");
                    dgvCampaigns.Columns["colSlots"].HeaderText = LanguageManager.Get("LannerTV.ColSlots", "Slot/Giorno");
                    dgvCampaigns.Columns["colDuration"].HeaderText = LanguageManager.Get("LannerTV.ColDuration", "Durata (min)");
                    dgvCampaigns.Columns["colTimes"].HeaderText = LanguageManager.Get("LannerTV.ColTimes", "Orari");
                    dgvCampaigns.Columns["colHasImage"].HeaderText = LanguageManager.Get("LannerTV.ColImage", "Img");
                }

                UpdateCampaignCount();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LannerTV] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_isEditing)
            {
                var result = MessageBox.Show(
                    "Ci sono modifiche non salvate.\nVuoi uscire comunque?",
                    "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}