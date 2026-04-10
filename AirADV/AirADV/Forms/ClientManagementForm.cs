using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;
using LibVLCSharp.Shared;

namespace AirADV.Forms
{
    public partial class ClientManagementForm : Form
    {
        private int _stationID;
        private List<DbcManager.Client> _clients = new List<DbcManager.Client>();
        private DbcManager.Client _selectedClient = null;
        private AudioManager _audioPlayer;

        // ═══════════════════════════════════════════════════════════
        // ✅ LibVLCSharp - Player video
        // ═══════════════════════════════════════════════════════════
        private LibVLC _libVLC;
        private LibVLCSharp.Shared.MediaPlayer _vlcMediaPlayer;

        // ═══════════════════════════════════════════════════════════
        // ✅ ESTENSIONI VIDEO RICONOSCIUTE
        // ═══════════════════════════════════════════════════════════
        private static readonly string[] VIDEO_EXTENSIONS = new string[]
        {
            ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v", ".mpg", ".mpeg"
        };

        private bool IsVideoFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            return VIDEO_EXTENSIONS.Contains(ext);
        }

        // ═══════════════════════════════════════════════════════════
        // ✅ FILTRI FILE in base alla modalità (Radio / Radio-TV)
        // ═══════════════════════════════════════════════════════════

        private string GetMediaFilter()
        {
            string mode = ConfigManager.StationMode;

            if (mode == "Radio-TV")
            {
                return LanguageManager.Get("ClientManagement.MediaFilter",
                    "File Media|*.mp3;*.wav;*.wma;*.aac;*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv" +
                    "|File Audio|*.mp3;*.wav;*.wma;*.aac" +
                    "|File Video|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv" +
                    "|Tutti i file|*.*");
            }
            else
            {
                return LanguageManager.Get("ClientManagement.AudioFilter",
                    "File Audio|*.mp3;*.wav;*.wma;*.aac");
            }
        }

        private string GetMediaDialogTitle()
        {
            string mode = ConfigManager.StationMode;

            if (mode == "Radio-TV")
            {
                return LanguageManager.Get("ClientManagement.SelectMediaFiles", "Seleziona file media (audio/video)");
            }
            else
            {
                return LanguageManager.Get("ClientManagement.SelectAudioFiles", "Seleziona file audio");
            }
        }

        private bool IsRadioTVMode()
        {
            return ConfigManager.StationMode == "Radio-TV";
        }

        public ClientManagementForm(int stationID)
        {
            InitializeComponent();
            _stationID = stationID;
            this.Load += ClientManagementForm_Load;
        }

        private void ClientManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                _audioPlayer = new AudioManager();

                SetupClientsGrid();
                SetupSpotsGrid();
                SetupCampaignsGrid();
                SetupVideoPreview();

                LoadClients();
                ApplyLanguage();

                tabClientDetails.Enabled = false;

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
            Console.WriteLine("[ClientManagement] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
        }

        // ═══════════════════════════════════════════════════════════
        // ✅ SETUP VIDEO PREVIEW con LibVLCSharp
        // ═══════════════════════════════════════════════════════════

        private void SetupVideoPreview()
        {
            try
            {
                // Inizializza LibVLC
                Core.Initialize();

                _libVLC = new LibVLC(
                //    "--no-audio",           // il video preview è muto di default
                    "--no-video-title-show"  // non mostrare titolo sul video
                );

                _vlcMediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);

                // Assegna il MediaPlayer alla VideoView del Designer
                vlcVideoView.MediaPlayer = _vlcMediaPlayer;

                // Pannello nascosto di default
                pnlVideoPreview.Visible = false;

                Console.WriteLine("[ClientManagement] ✅ LibVLCSharp video preview configurato");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] ⚠️ Errore setup video preview: {ex.Message}");
                pnlVideoPreview.Visible = false;
            }
        }

        private void ShowVideoPreview(string filePath, string title)
        {
            try
            {
                // Ferma tutto (audio + eventuale video precedente)
                StopAllPlayback();

                // Mostra pannello video
                pnlVideoPreview.Visible = true;
                lblVideoTitle.Text = $"📺 {title}";

                // Carica media in VLC
                var media = new Media(_libVLC, new Uri(filePath));
                _vlcMediaPlayer.Media = media;

                // Aggiorna stato player
                lblPlayerStatus.Text = $"📺 {title}";
                btnPlaySpot.Enabled = true;
                btnPlaySpot.Text = "▶";

                Console.WriteLine($"[ClientManagement] 📺 Video caricato: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] ❌ Errore caricamento video: {ex.Message}");
                HideVideoPreview();
            }
        }

        private void HideVideoPreview()
        {
            try
            {
                if (_vlcMediaPlayer != null && _vlcMediaPlayer.IsPlaying)
                {
                    _vlcMediaPlayer.Stop();
                }

                if (_vlcMediaPlayer != null)
                {
                    _vlcMediaPlayer.Media = null;
                }

                pnlVideoPreview.Visible = false;
            }
            catch { }
        }

        private void StopAllPlayback()
        {
            try
            {
                // Ferma audio
                if (_audioPlayer != null && _audioPlayer.IsPlaying)
                {
                    _audioPlayer.Stop();
                }

                // Ferma video VLC
                if (_vlcMediaPlayer != null && _vlcMediaPlayer.IsPlaying)
                {
                    _vlcMediaPlayer.Stop();
                }
            }
            catch { }
        }

        private bool IsVideoActive()
        {
            return pnlVideoPreview.Visible && _vlcMediaPlayer != null && _vlcMediaPlayer.Media != null;
        }

        // ═══════════════════════════════════════════════════════════
        // SETUP GRIDS
        // ═══════════════════════════════════════════════════════════

        private void SetupClientsGrid()
        {
            dgvClients.AutoGenerateColumns = false;
            dgvClients.AllowUserToAddRows = false;
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.MultiSelect = false;
            dgvClients.RowHeadersVisible = false;
            dgvClients.ReadOnly = true;
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvClients.Columns.Clear();

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClientCode",
                HeaderText = LanguageManager.Get("ClientManagement.ColClientCode", "Codice"),
                FillWeight = 15,
                ReadOnly = true
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClientName",
                HeaderText = LanguageManager.Get("ClientManagement.ColClientName", "Nome Cliente"),
                FillWeight = 40,
                ReadOnly = true
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCompany",
                HeaderText = LanguageManager.Get("ClientManagement.ColCompany", "Ragione Sociale"),
                FillWeight = 40,
                ReadOnly = true
            });

            dgvClients.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colActive",
                HeaderText = LanguageManager.Get("ClientManagement.ColActive", "Attivo"),
                FillWeight = 10,
                ReadOnly = true
            });

            dgvClients.SelectionChanged += DgvClients_SelectionChanged;
            dgvClients.CellDoubleClick += DgvClients_CellDoubleClick;
            dgvClients.CellClick += DgvClients_CellClick;
        }

        private void SetupSpotsGrid()
        {
            dgvSpots.AutoGenerateColumns = false;
            dgvSpots.AllowUserToAddRows = false;
            dgvSpots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSpots.RowHeadersVisible = false;
            dgvSpots.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSpots.Columns.Clear();

            dgvSpots.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSpotCode",
                HeaderText = LanguageManager.Get("ClientManagement.ColSpotCode", "Codice"),
                FillWeight = 15
            });

            dgvSpots.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSpotTitle",
                HeaderText = LanguageManager.Get("ClientManagement.ColSpotTitle", "Titolo Spot"),
                FillWeight = 35
            });

            dgvSpots.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFile",
                HeaderText = IsRadioTVMode()
                    ? LanguageManager.Get("ClientManagement.ColFileMedia", "File Media")
                    : LanguageManager.Get("ClientManagement.ColFile", "File Audio"),
                FillWeight = 30,
                ReadOnly = true
            });

            dgvSpots.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colBrowse",
                HeaderText = "",
                Text = "📁",
                UseColumnTextForButtonValue = true,
                FillWeight = 5
            });

            dgvSpots.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDuration",
                HeaderText = LanguageManager.Get("ClientManagement.ColDuration", "Durata"),
                FillWeight = 10,
                ReadOnly = true
            });

            dgvSpots.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colSpotActive",
                HeaderText = LanguageManager.Get("ClientManagement.ColSpotActive", "Attivo"),
                FillWeight = 10
            });

            dgvSpots.CellContentClick += DgvSpots_CellContentClick;
            dgvSpots.SelectionChanged += DgvSpots_SelectionChanged;
        }

        private void SetupCampaignsGrid()
        {
            dgvCampaigns.AutoGenerateColumns = false;
            dgvCampaigns.AllowUserToAddRows = false;
            dgvCampaigns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCampaigns.RowHeadersVisible = false;
            dgvCampaigns.ReadOnly = true;
            dgvCampaigns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCampaigns.Columns.Clear();

            dgvCampaigns.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colPDF",
                HeaderText = "",
                Text = "📄",
                UseColumnTextForButtonValue = true,
                FillWeight = 5
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCampaignCode",
                HeaderText = LanguageManager.Get("ClientManagement.ColCampaignCode", "Codice"),
                FillWeight = 12,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCampaignName",
                HeaderText = LanguageManager.Get("ClientManagement.ColCampaignName", "Nome Campagna"),
                FillWeight = 25,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSpotName",
                HeaderText = LanguageManager.Get("ClientManagement.ColSpot", "Spot"),
                FillWeight = 20,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = LanguageManager.Get("ClientManagement.ColStartDate", "Inizio"),
                FillWeight = 12,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = LanguageManager.Get("ClientManagement.ColEndDate", "Fine"),
                FillWeight = 12,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPasses",
                HeaderText = LanguageManager.Get("ClientManagement.ColPasses", "Pass/Giorno"),
                FillWeight = 10,
                ReadOnly = true
            });

            dgvCampaigns.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colCampaignActive",
                HeaderText = LanguageManager.Get("ClientManagement.ColCampaignActive", "Attiva"),
                FillWeight = 8,
                ReadOnly = true
            });

            dgvCampaigns.CellContentClick += DgvCampaigns_CellContentClick;
        }

        private void DgvCampaigns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvCampaigns.Columns["colPDF"].Index)
                return;

            var campaign = dgvCampaigns.Rows[e.RowIndex].Tag as DbcManager.Campaign;
            if (campaign == null) return;

            try
            {
                Console.WriteLine($"[ClientManagement] 📄 Apertura PDF per campagna: {campaign.CampaignName}");

                var client = _clients.FirstOrDefault(c => c.ID == campaign.ClientID);
                var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");

                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.CampaignID == campaign.ID && s.FileType == "SPOT")
                    .ToList();

                var spotIDs = schedules.Select(s => s.SpotID).Distinct().ToList();
                var selectedSpots = allSpots.Where(s => spotIDs.Contains(s.ID)).ToList();

                var groupedByDate = schedules
                    .GroupBy(s => s.ScheduleDate.Date)
                    .OrderBy(g => g.Key)
                    .ToList();

                var dailySchedules = new List<ScheduleEngine.DailySchedule>();

                foreach (var group in groupedByDate)
                {
                    var dailySchedule = new ScheduleEngine.DailySchedule
                    {
                        Date = group.Key,
                        TimeSlots = group.Select(s => s.SlotTime).Distinct().ToList(),
                        IsConfirmed = true,
                        IsModified = false
                    };

                    dailySchedules.Add(dailySchedule);
                }

                if (dailySchedules.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.NoScheduleFound", "Nessuna schedulazione trovata per questa campagna."),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var pdfForm = new CampaignPDFPreviewForm(campaign, dailySchedules, selectedSpots, client);
                pdfForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] ❌ Errore apertura PDF: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.PDFError", "Errore apertura PDF")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ═══════════════════════════════════════════════════════════
        // LOAD DATA
        // ═══════════════════════════════════════════════════════════

        private void LoadClients()
        {
            try
            {
                _clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc")
                    .OrderBy(c => c.ClientName)
                    .ToList();

                dgvClients.Rows.Clear();

                foreach (var client in _clients)
                {
                    int idx = dgvClients.Rows.Add();
                    var row = dgvClients.Rows[idx];

                    row.Cells["colClientCode"].Value = client.ClientCode;
                    row.Cells["colClientName"].Value = client.ClientName;
                    row.Cells["colCompany"].Value = client.CompanyName;
                    row.Cells["colActive"].Value = client.IsActive;
                    row.Tag = client;
                }

                lblClientsCount.Text = string.Format(
                    LanguageManager.Get("ClientManagement.ClientsCount", "Clienti:  {0}"),
                    _clients.Count
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.LoadClientsError", "Errore caricamento clienti")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DgvClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var client = dgvClients.Rows[e.RowIndex].Tag as DbcManager.Client;
                if (client != null)
                {
                    _selectedClient = client;
                }
            }
        }

        private void DgvClients_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                var client = dgvClients.SelectedRows[0].Tag as DbcManager.Client;
                if (client != null)
                {
                    _selectedClient = client;
                    LoadClientDetails(client);
                }
            }
            else
            {
                _selectedClient = null;
                tabClientDetails.Enabled = false;
            }
        }

        private void DgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var client = dgvClients.Rows[e.RowIndex].Tag as DbcManager.Client;
                if (client != null)
                {
                    LoadClientDetails(client);
                    tabClientDetails.SelectedTab = tabInfo;
                    txtClientName.Focus();
                    txtClientName.SelectAll();
                }
            }
        }

        private void LoadClientDetails(DbcManager.Client client)
        {
            try
            {
                if (client == null) return;

                _selectedClient = client;

                txtClientCode.Text = client.ClientCode ?? "";
                txtClientName.Text = client.ClientName ?? "";
                txtCompanyName.Text = client.CompanyName ?? "";
                txtAddress.Text = client.Address ?? "";
                txtCity.Text = client.City ?? "";
                txtPostalCode.Text = client.PostalCode ?? "";
                txtPhone.Text = client.Phone ?? "";
                txtEmail.Text = client.Email ?? "";
                txtVATNumber.Text = client.VATNumber ?? "";
                txtNotes.Text = client.Notes ?? "";
                chkClientActive.Checked = client.IsActive;

                tabClientDetails.Enabled = true;

                // ✅ Nascondi video quando si cambia cliente
                HideVideoPreview();

                LoadClientSpots(client.ID);
                LoadClientCampaigns(client.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.LoadDetailsError", "Errore caricamento dettagli")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadClientSpots(int clientID)
        {
            try
            {
                var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc")
                    .Where(s => s.ClientID == clientID)
                    .OrderBy(s => s.SpotCode)
                    .ToList();

                dgvSpots.Rows.Clear();

                foreach (var spot in spots)
                {
                    int idx = dgvSpots.Rows.Add();
                    var row = dgvSpots.Rows[idx];

                    row.Cells["colSpotCode"].Value = spot.SpotCode;
                    row.Cells["colSpotTitle"].Value = spot.SpotTitle;
                    row.Cells["colFile"].Value = Path.GetFileName(spot.FilePath);
                    row.Cells["colDuration"].Value = FormatDuration(spot.Duration);
                    row.Cells["colSpotActive"].Value = spot.IsActive;
                    row.Tag = spot;
                }

                lblSpotsCount.Text = string.Format(
                    LanguageManager.Get("ClientManagement.SpotsCount", "Spot: {0}"),
                    spots.Count
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] Errore LoadClientSpots: {ex.Message}");
            }
        }

        private void LoadClientCampaigns(int clientID)
        {
            try
            {
                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc")
                    .Where(c => c.ClientID == clientID && c.StationID == _stationID)
                    .OrderByDescending(c => c.StartDate)
                    .ToList();

                var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");

                dgvCampaigns.Rows.Clear();

                foreach (var campaign in campaigns)
                {
                    int idx = dgvCampaigns.Rows.Add();
                    var row = dgvCampaigns.Rows[idx];

                    var spot = spots.FirstOrDefault(s => s.ID == campaign.SpotID);

                    row.Cells["colCampaignCode"].Value = campaign.CampaignCode;
                    row.Cells["colCampaignName"].Value = campaign.CampaignName;
                    row.Cells["colSpotName"].Value = spot?.SpotTitle ?? "N/A";
                    row.Cells["colStartDate"].Value = campaign.StartDate.ToString("dd/MM/yyyy");
                    row.Cells["colEndDate"].Value = campaign.EndDate.ToString("dd/MM/yyyy");
                    row.Cells["colPasses"].Value = campaign.DailyPasses;
                    row.Cells["colCampaignActive"].Value = campaign.IsActive;
                    row.Tag = campaign;
                }

                lblCampaignsCount.Text = string.Format(
                    LanguageManager.Get("ClientManagement.CampaignsCount", "Campagne: {0}"),
                    campaigns.Count
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] Errore LoadClientCampaigns: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CLIENT ACTIONS
        // ═══════════════════════════════════════════════════════════

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            try
            {
                var newClient = new DbcManager.Client
                {
                    ClientCode = GenerateClientCode(),
                    ClientName = LanguageManager.Get("ClientManagement.NewClient", "Nuovo Cliente"),
                    CompanyName = "",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                var allClients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
                int maxID = allClients.Any() ? allClients.Max(c => c.ID) : 0;
                newClient.ID = maxID + 1;
                allClients.Add(newClient);

                DbcManager.Save("ADV_Clients.dbc", allClients);
                LoadClients();

                foreach (DataGridViewRow row in dgvClients.Rows)
                {
                    var client = row.Tag as DbcManager.Client;
                    if (client != null && client.ID == newClient.ID)
                    {
                        dgvClients.ClearSelection();
                        row.Selected = true;
                        dgvClients.CurrentCell = row.Cells[0];

                        _selectedClient = client;
                        LoadClientDetails(client);

                        tabClientDetails.SelectedTab = tabInfo;
                        txtClientName.Focus();
                        txtClientName.SelectAll();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.AddClientError", "Errore creazione cliente")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDeleteClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedClient == null)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.SelectClientToDelete", "Seleziona un cliente da eliminare! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc")
                    .Where(s => s.ClientID == _selectedClient.ID)
                    .ToList();

                if (spots.Count > 0)
                {
                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("ClientManagement.ClientHasSpots", "Impossibile eliminare.\n\nIl cliente ha {0} spot associati."),
                            spots.Count
                        ),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var result = MessageBox.Show(
                    string.Format(
                        LanguageManager.Get("ClientManagement.ConfirmDeleteClient", "Eliminare il cliente '{0}'?"),
                        _selectedClient.ClientName
                    ),
                    LanguageManager.Get("Common.Confirm", "Conferma"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var allClients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
                    allClients.RemoveAll(c => c.ID == _selectedClient.ID);
                    DbcManager.Save("ADV_Clients.dbc", allClients);

                    _selectedClient = null;
                    tabClientDetails.Enabled = false;
                    LoadClients();

                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.ClientDeleted", "✅ Cliente eliminato! "),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.DeleteError", "Errore eliminazione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedClient == null)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.NoClientSelected", "Nessun cliente selezionato!"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtClientName.Text))
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.ClientNameRequired", "Il nome cliente è obbligatorio!"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    txtClientName.Focus();
                    return;
                }

                _selectedClient.ClientCode = txtClientCode.Text?.Trim() ?? "";
                _selectedClient.ClientName = txtClientName.Text?.Trim() ?? "";
                _selectedClient.CompanyName = txtCompanyName.Text?.Trim() ?? "";
                _selectedClient.Address = txtAddress.Text?.Trim() ?? "";
                _selectedClient.City = txtCity.Text?.Trim() ?? "";
                _selectedClient.PostalCode = txtPostalCode.Text?.Trim() ?? "";
                _selectedClient.Phone = txtPhone.Text?.Trim() ?? "";
                _selectedClient.Email = txtEmail.Text?.Trim() ?? "";
                _selectedClient.VATNumber = txtVATNumber.Text?.Trim() ?? "";
                _selectedClient.Notes = txtNotes.Text?.Trim() ?? "";
                _selectedClient.IsActive = chkClientActive.Checked;

                var allClients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");

                for (int i = 0; i < allClients.Count; i++)
                {
                    if (allClients[i].ID == _selectedClient.ID)
                    {
                        allClients[i] = _selectedClient;
                        break;
                    }
                }

                DbcManager.Save("ADV_Clients.dbc", allClients);

                MessageBox.Show(
                    LanguageManager.Get("ClientManagement.ClientSaved", "✅ Cliente salvato!"),
                    LanguageManager.Get("Common.Success", "Successo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadClients();
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

        private string GenerateClientCode()
        {
            var allClients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
            int maxNum = 0;

            foreach (var c in allClients)
            {
                if (c.ClientCode.StartsWith("CLI-"))
                {
                    if (int.TryParse(c.ClientCode.Substring(4), out int num))
                    {
                        if (num > maxNum) maxNum = num;
                    }
                }
            }

            return $"CLI-{(maxNum + 1):D3}";
        }

        // ═══════════════════════════════════════════════════════════
        // SPOT ACTIONS
        // ═══════════════════════════════════════════════════════════

        private void btnAddSpot_Click(object sender, EventArgs e)
        {
            if (_selectedClient == null) return;

            var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
            int maxID = allSpots.Any() ? allSpots.Max(s => s.ID) : 0;

            var newSpot = new DbcManager.Spot
            {
                ID = maxID + 1,
                ClientID = _selectedClient.ID,
                SpotCode = GenerateSpotCodeGlobal(allSpots),
                SpotTitle = LanguageManager.Get("ClientManagement.NewSpot", "Nuovo Spot"),
                IsActive = true,
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddYears(10),
                CreatedDate = DateTime.Now
            };

            allSpots.Add(newSpot);
            DbcManager.Save("ADV_Spots.dbc", allSpots);
            LoadClientSpots(_selectedClient.ID);
        }

        private void btnImportSpots_Click(object sender, EventArgs e)
        {
            if (_selectedClient == null) return;

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = GetMediaDialogTitle();
                dialog.Filter = GetMediaFilter();
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
                    int maxID = allSpots.Any() ? allSpots.Max(s => s.ID) : 0;
                    int imported = 0;

                    foreach (string file in dialog.FileNames)
                    {
                        try
                        {
                            var spot = new DbcManager.Spot
                            {
                                ID = ++maxID,
                                ClientID = _selectedClient.ID,
                                SpotCode = GenerateSpotCodeGlobal(allSpots),
                                SpotTitle = Path.GetFileNameWithoutExtension(file),
                                FilePath = file,
                                Duration = AudioManager.GetDuration(file),
                                IsActive = true,
                                ValidFrom = DateTime.Now,
                                ValidTo = DateTime.Now.AddYears(10),
                                CreatedDate = DateTime.Now
                            };

                            allSpots.Add(spot);
                            imported++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ClientManagement] Errore import file {file}: {ex.Message}");
                        }
                    }

                    DbcManager.Save("ADV_Spots.dbc", allSpots);
                    LoadClientSpots(_selectedClient.ID);

                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("ClientManagement.SpotsImported", "✅ Importati {0} spot! "),
                            imported
                        ),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        private void btnDeleteSpot_Click(object sender, EventArgs e)
        {
            if (dgvSpots.SelectedRows.Count == 0) return;

            var spot = dgvSpots.SelectedRows[0].Tag as DbcManager.Spot;
            if (spot == null) return;

            var result = MessageBox.Show(
                string.Format(
                    LanguageManager.Get("ClientManagement.ConfirmDeleteSpot", "Eliminare lo spot '{0}'?"),
                    spot.SpotTitle
                ),
                LanguageManager.Get("Common.Confirm", "Conferma"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
                allSpots.RemoveAll(s => s.ID == spot.ID);
                DbcManager.Save("ADV_Spots.dbc", allSpots);
                LoadClientSpots(_selectedClient.ID);
            }
        }

        private void btnSaveSpots_Click(object sender, EventArgs e)
        {
            if (_selectedClient == null) return;

            var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");

            foreach (DataGridViewRow row in dgvSpots.Rows)
            {
                var spot = row.Tag as DbcManager.Spot;
                if (spot != null)
                {
                    spot.SpotCode = row.Cells["colSpotCode"].Value?.ToString();
                    spot.SpotTitle = row.Cells["colSpotTitle"].Value?.ToString();
                    spot.IsActive = Convert.ToBoolean(row.Cells["colSpotActive"].Value);

                    var existing = allSpots.FirstOrDefault(s => s.ID == spot.ID);
                    if (existing != null)
                    {
                        allSpots[allSpots.IndexOf(existing)] = spot;
                    }
                }
            }

            DbcManager.Save("ADV_Spots.dbc", allSpots);
            MessageBox.Show(
                LanguageManager.Get("ClientManagement.SpotsSaved", "✅ Spot salvati!"),
                LanguageManager.Get("Common.Success", "Successo"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void DgvSpots_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvSpots.Columns["colBrowse"].Index)
                return;

            var spot = dgvSpots.Rows[e.RowIndex].Tag as DbcManager.Spot;
            if (spot == null) return;

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = GetMediaDialogTitle();
                dialog.Filter = GetMediaFilter();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    spot.FilePath = dialog.FileName;
                    spot.Duration = AudioManager.GetDuration(dialog.FileName);

                    dgvSpots.Rows[e.RowIndex].Cells["colFile"].Value = Path.GetFileName(spot.FilePath);
                    dgvSpots.Rows[e.RowIndex].Cells["colDuration"].Value = FormatDuration(spot.Duration);
                }
            }
        }

        /// <summary>
        /// ✅ Gestisce sia file audio che video con LibVLCSharp
        /// </summary>
        private void DgvSpots_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvSpots.SelectedRows.Count > 0)
                {
                    var spot = dgvSpots.SelectedRows[0].Tag as DbcManager.Spot;
                    if (spot != null && !string.IsNullOrEmpty(spot.FilePath) && File.Exists(spot.FilePath))
                    {
                        if (IsVideoFile(spot.FilePath))
                        {
                            // ✅ FILE VIDEO → mostra mini-monitor VLC
                            ShowVideoPreview(spot.FilePath, spot.SpotTitle);
                        }
                        else
                        {
                            // ✅ FILE AUDIO → usa AudioManager
                            HideVideoPreview();

                            if (_audioPlayer != null && _audioPlayer.IsPlaying)
                            {
                                _audioPlayer.Stop();
                            }

                            _audioPlayer.Load(spot.FilePath);
                            lblPlayerStatus.Text = $"🎵 {spot.SpotTitle}";
                            btnPlaySpot.Enabled = true;
                            btnPlaySpot.Text = "▶";
                        }
                    }
                    else
                    {
                        HideVideoPreview();
                        lblPlayerStatus.Text = LanguageManager.Get("ClientManagement.NoFileLoaded", "Nessun file caricato");
                        btnPlaySpot.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] Errore SelectionChanged: {ex.Message}");
                lblPlayerStatus.Text = LanguageManager.Get("ClientManagement.LoadError", "Errore caricamento file");
                btnPlaySpot.Enabled = false;
            }
        }

        /// <summary>
        /// ✅ Play/Pause gestisce sia audio che video (LibVLCSharp)
        /// </summary>
        private void btnPlaySpot_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsVideoActive())
                {
                    // ✅ CONTROLLO VIDEO tramite LibVLCSharp
                    if (_vlcMediaPlayer.IsPlaying)
                    {
                        _vlcMediaPlayer.Pause();
                        btnPlaySpot.Text = "▶";
                    }
                    else
                    {
                        _vlcMediaPlayer.Play();
                        btnPlaySpot.Text = "⏸";
                    }
                }
                else
                {
                    // ✅ CONTROLLO AUDIO tramite AudioManager
                    if (_audioPlayer == null) return;

                    if (_audioPlayer.IsPlaying)
                    {
                        _audioPlayer.Pause();
                        btnPlaySpot.Text = "▶";
                    }
                    else
                    {
                        _audioPlayer.Play();
                        btnPlaySpot.Text = "⏸";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.PlayError", "Errore riproduzione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string GenerateSpotCodeGlobal(List<DbcManager.Spot> allSpots)
        {
            int maxNum = 0;

            foreach (var s in allSpots)
            {
                if (s.SpotCode.StartsWith("SPOT-"))
                {
                    if (int.TryParse(s.SpotCode.Substring(5), out int num))
                    {
                        if (num > maxNum) maxNum = num;
                    }
                }
            }

            return $"SPOT-{(maxNum + 1):D3}";
        }

        // ═══════════════════════════════════════════════════════════
        // CAMPAIGN ACTIONS
        // ═══════════════════════════════════════════════════════════

        private void btnAddCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedClient == null)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.SelectClientFirst", "Seleziona prima un cliente!"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var clientSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc")
                    .Where(s => s.ClientID == _selectedClient.ID && s.IsActive)
                    .ToList();

                if (clientSpots.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("ClientManagement.NoActiveSpotsForCampaign", "Il cliente non ha spot attivi!\n\nCrea prima uno spot prima di creare una campagna."),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    tabClientDetails.SelectedTab = tabSpots;
                    return;
                }

                var form = new CampaignWizardForm(_stationID, _selectedClient.ID);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadClientCampaigns(_selectedClient.ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("ClientManagement.CampaignWizardError", "Errore apertura wizard campagna")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnEditCampaign_Click(object sender, EventArgs e)
        {
            if (dgvCampaigns.SelectedRows.Count == 0) return;

            var campaign = dgvCampaigns.SelectedRows[0].Tag as DbcManager.Campaign;
            if (campaign != null)
            {
                var form = new CampaignWizardForm(_stationID, campaign);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadClientCampaigns(_selectedClient.ID);
                }
            }
        }

        private void btnDeleteCampaign_Click(object sender, EventArgs e)
        {
            if (dgvCampaigns.SelectedRows.Count == 0) return;

            var campaign = dgvCampaigns.SelectedRows[0].Tag as DbcManager.Campaign;
            if (campaign == null) return;

            var result = MessageBox.Show(
                string.Format(
                    LanguageManager.Get("ClientManagement.ConfirmDeleteCampaign", "Eliminare la campagna '{0}'?"),
                    campaign.CampaignName
                ),
                LanguageManager.Get("Common.Confirm", "Conferma"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                var allCampaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                allCampaigns.RemoveAll(c => c.ID == campaign.ID);
                DbcManager.Save("ADV_Campaigns.dbc", allCampaigns);
                LoadClientCampaigns(_selectedClient.ID);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════

        private string FormatDuration(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;
            return $"{mins}:{secs:D2}";
        }

        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[ClientManagement] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                this.Text = LanguageManager.Get("ClientManagement.WindowTitle", "👥 Gestione Clienti");

                var grpClientsArray = this.Controls.Find("grpClients", true);
                if (grpClientsArray.Length > 0 && grpClientsArray[0] is GroupBox grpClients)
                {
                    grpClients.Text = LanguageManager.Get("ClientManagement.GrpClients", "📋 Elenco Clienti");
                }

                if (lblClientsTitle != null)
                {
                    lblClientsTitle.Text = LanguageManager.Get("ClientManagement.ClientsTitle", "📋 Elenco Clienti");
                }

                tabInfo.Text = $"   {LanguageManager.Get("ClientManagement.TabInfo", "📋 Dati Anagrafici")}   ";
                tabSpots.Text = $"   {LanguageManager.Get("ClientManagement.TabSpots", "🎵 Spot")}   ";
                tabCampaigns.Text = $"   {LanguageManager.Get("ClientManagement.TabCampaigns", "📅 Campagne")}   ";

                btnAddClient.Text = LanguageManager.Get("ClientManagement.BtnAddClient", "➕ Nuovo Cliente");
                btnDeleteClient.Text = LanguageManager.Get("ClientManagement.BtnDeleteClient", "🗑️ Elimina");
                btnSaveClient.Text = LanguageManager.Get("Common.Save", "💾 Salva");

                lblClientCode.Text = LanguageManager.Get("ClientManagement.LblClientCode", "Codice Cliente:");
                lblClientName.Text = LanguageManager.Get("ClientManagement.LblClientName", "Nome Cliente:");
                lblCompanyName.Text = LanguageManager.Get("ClientManagement.LblCompanyName", "Ragione Sociale:");
                lblAddress.Text = LanguageManager.Get("ClientManagement.LblAddress", "Indirizzo:");
                lblCity.Text = LanguageManager.Get("ClientManagement.LblCity", "Città:");
                lblPostalCode.Text = LanguageManager.Get("ClientManagement.LblPostalCode", "CAP:");
                lblPhone.Text = LanguageManager.Get("ClientManagement.LblPhone", "Telefono:");
                lblEmail.Text = LanguageManager.Get("ClientManagement.LblEmail", "Email:");
                lblVATNumber.Text = LanguageManager.Get("ClientManagement.LblVATNumber", "Partita IVA:");
                lblNotes.Text = LanguageManager.Get("ClientManagement.LblNotes", "Note:");
                chkClientActive.Text = LanguageManager.Get("ClientManagement.ChkClientActive", "✓ Cliente Attivo");

                btnAddSpot.Text = LanguageManager.Get("ClientManagement.BtnAddSpot", "➕ Nuovo Spot");
                btnImportSpots.Text = IsRadioTVMode()
                    ? LanguageManager.Get("ClientManagement.BtnImportMedia", "📂 Importa Media")
                    : LanguageManager.Get("ClientManagement.BtnImportSpots", "📂 Importa Audio");
                btnDeleteSpot.Text = LanguageManager.Get("ClientManagement.BtnDeleteSpot", "🗑️ Elimina");
                btnSaveSpots.Text = LanguageManager.Get("Common.Save", "💾 Salva");

                btnPlaySpot.Text = "▶";
                if (lblPlayerStatus.Text == "Nessun file caricato" || lblPlayerStatus.Text == "No file loaded")
                {
                    lblPlayerStatus.Text = LanguageManager.Get("ClientManagement.NoFileLoaded", "Nessun file caricato");
                }

                if (lblVideoTitle != null && !lblVideoTitle.Text.StartsWith("📺"))
                {
                    lblVideoTitle.Text = LanguageManager.Get("ClientManagement.VideoPreview", "📺 Anteprima Video");
                }

                btnAddCampaign.Text = LanguageManager.Get("ClientManagement.BtnAddCampaign", "➕ Nuova Campagna");
                btnEditCampaign.Text = LanguageManager.Get("ClientManagement.BtnEditCampaign", "✏️ Modifica");
                btnDeleteCampaign.Text = LanguageManager.Get("ClientManagement.BtnDeleteCampaign", "🗑️ Elimina");

                if (dgvClients.Columns.Count > 0)
                {
                    dgvClients.Columns["colClientCode"].HeaderText = LanguageManager.Get("ClientManagement.ColClientCode", "Codice");
                    dgvClients.Columns["colClientName"].HeaderText = LanguageManager.Get("ClientManagement.ColClientName", "Nome Cliente");
                    dgvClients.Columns["colCompany"].HeaderText = LanguageManager.Get("ClientManagement.ColCompany", "Ragione Sociale");
                    dgvClients.Columns["colActive"].HeaderText = LanguageManager.Get("ClientManagement.ColActive", "Attivo");
                }

                if (dgvSpots.Columns.Count > 0)
                {
                    dgvSpots.Columns["colSpotCode"].HeaderText = LanguageManager.Get("ClientManagement.ColSpotCode", "Codice");
                    dgvSpots.Columns["colSpotTitle"].HeaderText = LanguageManager.Get("ClientManagement.ColSpotTitle", "Titolo Spot");

                    dgvSpots.Columns["colFile"].HeaderText = IsRadioTVMode()
                        ? LanguageManager.Get("ClientManagement.ColFileMedia", "File Media")
                        : LanguageManager.Get("ClientManagement.ColFile", "File Audio");

                    dgvSpots.Columns["colDuration"].HeaderText = LanguageManager.Get("ClientManagement.ColDuration", "Durata");
                    dgvSpots.Columns["colSpotActive"].HeaderText = LanguageManager.Get("ClientManagement.ColSpotActive", "Attivo");
                }

                if (dgvCampaigns.Columns.Count > 0)
                {
                    dgvCampaigns.Columns["colCampaignCode"].HeaderText = LanguageManager.Get("ClientManagement.ColCampaignCode", "Codice");
                    dgvCampaigns.Columns["colCampaignName"].HeaderText = LanguageManager.Get("ClientManagement.ColCampaignName", "Nome Campagna");
                    dgvCampaigns.Columns["colSpotName"].HeaderText = LanguageManager.Get("ClientManagement.ColSpot", "Spot");
                    dgvCampaigns.Columns["colStartDate"].HeaderText = LanguageManager.Get("ClientManagement.ColStartDate", "Inizio");
                    dgvCampaigns.Columns["colEndDate"].HeaderText = LanguageManager.Get("ClientManagement.ColEndDate", "Fine");
                    dgvCampaigns.Columns["colPasses"].HeaderText = LanguageManager.Get("ClientManagement.ColPasses", "Pass/Giorno");
                    dgvCampaigns.Columns["colCampaignActive"].HeaderText = LanguageManager.Get("ClientManagement.ColCampaignActive", "Attiva");
                }

                if (_clients.Count > 0)
                {
                    lblClientsCount.Text = string.Format(
                        LanguageManager.Get("ClientManagement.ClientsCount", "Clienti:  {0}"),
                        _clients.Count
                    );
                }

                Console.WriteLine("[ClientManagement] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientManagement] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            try
            {
                // ✅ Ferma tutto
                StopAllPlayback();

                // ✅ Rilascia AudioManager
                if (_audioPlayer != null)
                {
                    _audioPlayer.Dispose();
                    _audioPlayer = null;
                }

                // ✅ Rilascia LibVLCSharp
                if (_vlcMediaPlayer != null)
                {
                    _vlcMediaPlayer.Dispose();
                    _vlcMediaPlayer = null;
                }

                if (_libVLC != null)
                {
                    _libVLC.Dispose();
                    _libVLC = null;
                }

                LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
            }
            catch { }
        }
    }
}