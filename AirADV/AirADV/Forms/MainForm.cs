using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Forms;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class MainForm : Form
    {
        private int _currentStationID = 0;

        // ═══════════════════════════════════════════════════════════
        // ✅ Posizioni bottoni per layout dinamico
        // ═══════════════════════════════════════════════════════════
        private const int SETTINGS_X = 823;
        private const int SETTINGS_NORMAL_Y = 32;          // Posizione normale (no Lanner)
        private const int SETTINGS_SHIFTED_Y = 107;         // Posizione abbassata (con Lanner sopra)
        private const int SETTINGS_NORMAL_HEIGHT = 140;     // Altezza normale
        private const int SETTINGS_SHIFTED_HEIGHT = 65;     // Altezza ridotta quando c'è Lanner
        private const int LANNER_Y = 32;                    // Posizione Lanner (sopra)
        private const int LANNER_HEIGHT = 65;               // Altezza Lanner

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Seleziona emittente
                if (!SelectStation())
                {
                    Application.Exit();
                    return;
                }

                ApplyLanguage();
                UpdateStationInfo();
                UpdateLannerTVVisibility();

                // ✅ Reagisce al cambio lingua
                LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.StartupError", "Errore avvio")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            Console.WriteLine("[MainForm] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
            UpdateStationInfo();
        }

        // ═══════════════════════════════════════════════════════════
        // ✅ NUOVO: Gestione visibilità Lanner TV
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Mostra/nasconde il bottone Lanner TV in base alla modalità.
        /// Se Radio-TV → mostra Lanner e abbassa Settings.
        /// Se Radio    → nasconde Lanner e Settings torna alla posizione normale.
        /// </summary>
        private void UpdateLannerTVVisibility()
        {
            try
            {
                bool isRadioTV = ConfigManager.StationMode == "Radio-TV";

                if (isRadioTV)
                {
                    // ✅ Mostra Lanner TV sopra
                    btnLannerTV.Visible = true;
                    btnLannerTV.Location = new Point(SETTINGS_X, LANNER_Y);
                    btnLannerTV.Size = new Size(140, LANNER_HEIGHT);

                    // ✅ Abbassa e rimpicciolisci Settings
                    btnSettings.Location = new Point(SETTINGS_X, SETTINGS_SHIFTED_Y);
                    btnSettings.Size = new Size(140, SETTINGS_SHIFTED_HEIGHT);
                    btnSettings.Padding = new Padding(0, 5, 0, 5);
                    btnSettings.Text = LanguageManager.Get("MainForm.BtnSettings", "⚙️ Impostazioni");

                    Console.WriteLine("[MainForm] 📺 Modalità Radio-TV: Lanner TV visibile");
                }
                else
                {
                    // ✅ Nascondi Lanner TV
                    btnLannerTV.Visible = false;

                    // ✅ Settings torna alla posizione/dimensione normale
                    btnSettings.Location = new Point(SETTINGS_X, SETTINGS_NORMAL_Y);
                    btnSettings.Size = new Size(140, SETTINGS_NORMAL_HEIGHT);
                    btnSettings.Padding = new Padding(0, 20, 0, 10);
                    btnSettings.Text = LanguageManager.Get("MainForm.BtnSettings", "⚙️ Impostazioni");

                    Console.WriteLine("[MainForm] 📻 Modalità Radio: Lanner TV nascosto");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MainForm] ❌ Errore UpdateLannerTVVisibility: {ex.Message}");
            }
        }

        /// <summary>
        /// ✅ NUOVO: Click su Lanner TV → apre form programmazione
        /// </summary>
        private void btnLannerTV_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new LannerCampaignForm(_currentStationID);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ═══════════════════════════════════════════════════════════
        // SELEZIONE EMITTENTE
        // ═══════════════════════════════════════════════════════════

        private bool SelectStation()
        {
            try
            {
                var stations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc")
                    .Where(s => s.IsActive)
                    .ToList();

                if (stations.Count == 0)
                {
                    var result = MessageBox.Show(
                        LanguageManager.Get("MainForm.NoStationsConfigured", "Nessuna emittente configurata.\n\nVuoi configurarne una ora?"),
                        LanguageManager.Get("MainForm.FirstConfiguration", "Prima Configurazione"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        var stationForm = new StationManagerForm();
                        if (stationForm.ShowDialog() == DialogResult.OK)
                        {
                            return SelectStation();
                        }
                    }
                    return false;
                }

                if (stations.Count == 1)
                {
                    _currentStationID = stations[0].StationID;
                    ConfigManager.CurrentStationID = _currentStationID;
                    ConfigManager.Save();
                    Console.WriteLine($"[MainForm] Emittente selezionata automaticamente: {stations[0].StationName}");
                    return true;
                }

                return ShowStationSelectionDialog(stations);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("MainForm.StationSelectionError", "Errore selezione emittente")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }

        private bool ShowStationSelectionDialog(System.Collections.Generic.List<DbcManager.Station> stations)
        {
            using (var selectForm = new Form())
            {
                selectForm.Text = LanguageManager.Get("MainForm.SelectStationTitle", "AirADV:  Seleziona Emittente");
                selectForm.Width = 400;
                selectForm.Height = 200;
                selectForm.StartPosition = FormStartPosition.CenterScreen;
                selectForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                selectForm.MaximizeBox = false;
                selectForm.MinimizeBox = false;

                var lblPrompt = new Label
                {
                    Text = LanguageManager.Get("MainForm.SelectStationPrompt", "Seleziona un'emittente per l'avvio: "),
                    Location = new Point(20, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F)
                };

                var cmbStations = new ComboBox
                {
                    Location = new Point(20, 50),
                    Width = 340,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10F)
                };

                foreach (var station in stations)
                {
                    cmbStations.Items.Add(station);
                }
                cmbStations.DisplayMember = "StationName";
                cmbStations.ValueMember = "StationID";

                if (ConfigManager.CurrentStationID > 0)
                {
                    var lastStation = stations.FirstOrDefault(s => s.StationID == ConfigManager.CurrentStationID);
                    if (lastStation != null)
                    {
                        cmbStations.SelectedItem = lastStation;
                    }
                }

                if (cmbStations.SelectedIndex < 0)
                    cmbStations.SelectedIndex = 0;

                var btnOK = new Button
                {
                    Text = LanguageManager.Get("Common.OK", "OK"),
                    DialogResult = DialogResult.OK,
                    Location = new Point(285, 120),
                    Width = 75,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var btnCancel = new Button
                {
                    Text = LanguageManager.Get("Common.Cancel", "Annulla"),
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(200, 120),
                    Width = 75
                };

                selectForm.Controls.Add(lblPrompt);
                selectForm.Controls.Add(cmbStations);
                selectForm.Controls.Add(btnOK);
                selectForm.Controls.Add(btnCancel);
                selectForm.AcceptButton = btnOK;
                selectForm.CancelButton = btnCancel;

                if (selectForm.ShowDialog() == DialogResult.OK && cmbStations.SelectedItem is DbcManager.Station selected)
                {
                    _currentStationID = selected.StationID;
                    ConfigManager.CurrentStationID = _currentStationID;
                    ConfigManager.Save();
                    Console.WriteLine($"[MainForm] Emittente selezionata: {selected.StationName}");
                    return true;
                }

                return false;
            }
        }

        private void UpdateStationInfo()
        {
            try
            {
                var station = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc")
                    .FirstOrDefault(s => s.StationID == _currentStationID);

                if (station != null)
                {
                    lblStationName.Text = station.StationName;
                    this.Text = $"AirADV - {station.StationName}";

                    if (!string.IsNullOrEmpty(station.LogoPath) && System.IO.File.Exists(station.LogoPath))
                    {
                        try
                        {
                            picStationLogo.Image?.Dispose();
                            picStationLogo.Image = Image.FromFile(station.LogoPath);
                            picStationLogo.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[MainForm] Errore caricamento logo: {ex.Message}");
                            picStationLogo.Visible = false;
                        }
                    }
                    else
                    {
                        picStationLogo.Visible = false;
                    }

                    Console.WriteLine($"[MainForm] Emittente caricata: {station.StationName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MainForm] Errore UpdateStationInfo: {ex.Message}");
            }
        }

        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[MainForm] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                lblTitle.Text = LanguageManager.Get("MainForm.Title", "📻 Gestionale Pubblicitario");

                btnClients.Text = LanguageManager.Get("MainForm.BtnClients", "👥 Clienti");
                btnTimeSlots.Text = LanguageManager.Get("MainForm.BtnTimeSlots", "🕐 Punti Orari");
                btnCategories.Text = LanguageManager.Get("MainForm.BtnCategories", "🏷️ Categorie");
                btnSchedule.Text = LanguageManager.Get("MainForm.BtnSchedule", "📅 Palinsesto");
                btnReports.Text = LanguageManager.Get("MainForm.BtnReports", "📊 Report");
                btnSettings.Text = LanguageManager.Get("MainForm.BtnSettings", "⚙️ Impostazioni");

                // ✅ NUOVO: Traduzione Lanner TV
                btnLannerTV.Text = LanguageManager.Get("MainForm.BtnLannerTV", "📺 Lanner TV");

                btnChangeStation.Text = LanguageManager.Get("MainForm.BtnChangeStation", "🔄 Cambia Emittente");
                btnExit.Text = LanguageManager.Get("MainForm.BtnExit", "✖ Esci");

                // ✅ Aggiorna anche la visibilità del Lanner (rilayout)
                UpdateLannerTVVisibility();

                this.Refresh();

                Console.WriteLine("[MainForm] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MainForm] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════════
        // HANDLER BOTTONI
        // ═══════════════════════════════════════════════════════════

        private void btnClients_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ClientManagementForm(_currentStationID);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnTimeSlots_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new TimeSlotsForm(_currentStationID);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new CategoriesForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ScheduleViewForm(_currentStationID, DateTime.Now.Date);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ReportForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ConfigurationForm();
                form.ShowDialog();

                // ✅ Dopo la chiusura di Settings, aggiorna visibilità Lanner
                // (l'utente potrebbe aver cambiato la modalità Radio/Radio-TV)
                UpdateLannerTVVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.OpenFormError", "Errore apertura form")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnChangeStation_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new StationManagerForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _currentStationID = form.SelectedStationID;
                    UpdateStationInfo();

                    MessageBox.Show(
                        LanguageManager.Get("MainForm.StationChanged", "✅ Emittente cambiata con successo!\n\nL'applicazione è ora connessa alla nuova emittente."),
                        LanguageManager.Get("MainForm.StationChangedTitle", "Emittente Cambiata"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.ChangeStationError", "Errore cambio emittente")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.ConfirmExit", "Vuoi uscire dal programma?"),
                    LanguageManager.Get("Messages.ConfirmExitTitle", "Conferma Uscita"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MainForm] Errore btnExit_Click: {ex.Message}");
                Application.Exit();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            try
            {
                if (ConfigManager.CurrentStationID > 0)
                {
                    Console.WriteLine("[MainForm] Export finale AirDirector...");

                    bool exported = AirDirectorExportService.ExportFullSchedule(
                        ConfigManager.CurrentStationID,
                        DateTime.Now.Date,
                        DateTime.Now.Date.AddDays(30)
                    );

                    if (exported)
                    {
                        bool valid = AirDirectorExportService.ValidateExport(ConfigManager.CurrentStationID);

                        if (valid)
                        {
                            var stats = AirDirectorExportService.GetExportStats(ConfigManager.CurrentStationID);
                            Console.WriteLine($"[MainForm] ✅ Export finale completato:");
                            Console.WriteLine($"  - Giorni: {stats.DaysCount}");
                            Console.WriteLine($"  - Slot: {stats.SlotsCount}");
                            Console.WriteLine($"  - Spot: {stats.TotalSpots}");
                            Console.WriteLine($"  - Durata: {stats.TotalDuration}s");
                        }
                        else
                        {
                            Console.WriteLine("[MainForm] ⚠️ Export finale con warning (file mancanti)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MainForm] ⚠️ Errore export finale: {ex.Message}");
            }

            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
            ConfigManager.Save();
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            // Empty by design
        }
    }
}