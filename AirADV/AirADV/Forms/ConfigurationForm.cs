using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Licensing;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class ConfigurationForm : Form
    {
        private bool _isDirty = false;
        private bool _isLoading = false;

        public ConfigurationForm()
        {
            InitializeComponent();
            this.Load += ConfigurationForm_Load;
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;
                LoadSettings();
                ApplyLanguage();
                UpdateLicenseButtonsVisibility();
                _isLoading = false;
                _isDirty = false;

                LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show($"Errore caricamento impostazioni:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"[ConfigurationForm] 🔄 Evento LanguageChanged ricevuto");
            ApplyLanguage();
        }

        private void LoadSettings()
        {
            try
            {
                LoadAvailableLanguages();
                chkAutoSave.Checked = ConfigManager.AutoSave;

                // ✅ Carica Modalità dal Registro
                string currentMode = ConfigManager.StationMode;
                if (currentMode == "Radio-TV")
                {
                    rbModeRadioTV.Checked = true;
                }
                else
                {
                    rbModeRadio.Checked = true; // Default: Radio
                }

                lblDatabasePathValue.Text = ConfigManager.DATABASE_PATH;
                lblStationsPathValue.Text = ConfigManager.STATIONS_PATH;
                lblMediaPathValue.Text = ConfigManager.MediaLibraryPath;
                lblBackupPathValue.Text = ConfigManager.BACKUP_PATH;
                lblReportsPathValue.Text = ConfigManager.PdfOutputPath;
                lblLogsPathValue.Text = ConfigManager.LogsPath;

                chkAutoBackup.Checked = ConfigManager.AutoBackup;
                numBackupRetentionDays.Value = ConfigManager.BackupRetentionDays;
                numBackupRetentionDays.Enabled = chkAutoBackup.Checked;

                LoadAudioDevices();
                trackMiniPlayerVolume.Value = ConfigManager.MiniPlayerVolume;
                lblVolumeValue.Text = $"{ConfigManager.MiniPlayerVolume}%";

                _isDirty = false;

                Console.WriteLine("[ConfigurationForm] ✅ Impostazioni caricate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigurationForm] ❌ Errore LoadSettings: {ex.Message}");
            }
        }

        /// <summary>
        /// ✅ Carica lingue disponibili - SENZA bandierine, solo nome
        /// </summary>
        private void LoadAvailableLanguages()
        {
            try
            {
                cmbLanguage.Items.Clear();

                var availableLanguages = LanguageManager.GetAvailableLanguages();

                if (availableLanguages.Count == 0)
                {
                    Console.WriteLine("[ConfigurationForm] ⚠️ Nessuna lingua trovata, aggiungo default");
                    cmbLanguage.Items.Add(new LanguageItem
                    {
                        FileName = "Italian",
                        DisplayName = "Italiano",
                        Code = "it-IT"
                    });
                }
                else
                {
                    foreach (var lang in availableLanguages)
                    {
                        // ✅ Estrai nome file senza estensione
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(lang.FilePath);

                        cmbLanguage.Items.Add(new LanguageItem
                        {
                            FileName = fileName,           // Es: "Italian", "English"
                            DisplayName = lang.Name,       // Es: "Italiano", "English"
                            Code = lang.Code              // Es: "it-IT", "en-US"
                        });

                        Console.WriteLine($"[ConfigurationForm] Lingua aggiunta: {lang.Name} (file: {fileName})");
                    }
                }

                cmbLanguage.DisplayMember = "DisplayName";  // ✅ Mostra solo il nome
                cmbLanguage.ValueMember = "FileName";       // ✅ Valore = nome file

                // ✅ Seleziona lingua corrente (cerca per nome file)
                string currentLanguageFile = ConfigManager.Language; // Es: "Italian" o "English"

                for (int i = 0; i < cmbLanguage.Items.Count; i++)
                {
                    var item = cmbLanguage.Items[i] as LanguageItem;
                    if (item != null && item.FileName == currentLanguageFile)
                    {
                        cmbLanguage.SelectedIndex = i;
                        Console.WriteLine($"[ConfigurationForm] Lingua corrente selezionata: {item.DisplayName} ({item.FileName})");
                        break;
                    }
                }

                // Se nessuna selezione, prendi la prima
                if (cmbLanguage.SelectedIndex < 0 && cmbLanguage.Items.Count > 0)
                {
                    cmbLanguage.SelectedIndex = 0;
                }

                Console.WriteLine($"[ConfigurationForm] ✅ Caricate {cmbLanguage.Items.Count} lingue");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigurationForm] ❌ Errore LoadAvailableLanguages: {ex.Message}");
                cmbLanguage.Items.Clear();
                cmbLanguage.Items.Add(new LanguageItem { FileName = "Italian", DisplayName = "Italiano", Code = "it-IT" });
                cmbLanguage.SelectedIndex = 0;
            }
        }

        private void LoadAudioDevices()
        {
            try
            {
                cmbOutputDevice.Items.Clear();
                cmbOutputDevice.Items.Add(new AudioDeviceItem
                {
                    DeviceNumber = -1,
                    DeviceName = LanguageManager.Get("Configuration.DefaultAudioDevice", "Dispositivo Predefinito")
                });

                for (int i = 0; i < 10; i++)
                {
                    cmbOutputDevice.Items.Add(new AudioDeviceItem
                    {
                        DeviceNumber = i,
                        DeviceName = $"{LanguageManager.Get("Configuration.AudioDevice", "Dispositivo Audio")} {i + 1}"
                    });
                }

                cmbOutputDevice.DisplayMember = "DeviceName";
                cmbOutputDevice.ValueMember = "DeviceNumber";

                for (int i = 0; i < cmbOutputDevice.Items.Count; i++)
                {
                    var item = cmbOutputDevice.Items[i] as AudioDeviceItem;
                    if (item != null && item.DeviceNumber == ConfigManager.OutputDeviceNumber)
                    {
                        cmbOutputDevice.SelectedIndex = i;
                        break;
                    }
                }

                if (cmbOutputDevice.SelectedIndex < 0)
                    cmbOutputDevice.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigurationForm] Errore LoadAudioDevices:  {ex.Message}");
            }
        }

        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[ConfigurationForm] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})...");

                // ✅ TITOLO FINESTRA
                this.Text = LanguageManager.Get("Configuration.WindowTitle", "⚙️ Configurazione");

                // ✅ TAB
                tabGeneral.Text = "   " + LanguageManager.Get("Configuration.TabGeneral", "🌐 Generale") + "   ";
                tabPaths.Text = "   " + LanguageManager.Get("Configuration.TabPaths", "📁 Percorsi") + "   ";
                tabBackup.Text = "   " + LanguageManager.Get("Configuration.TabBackup", "💾 Backup") + "   ";

                // ✅ TAB GENERALE
                lblLanguage.Text = LanguageManager.Get("Configuration.LblLanguage", "Lingua:");
                chkAutoSave.Text = LanguageManager.Get("Configuration.ChkAutoSave", "Salvataggio automatico modifiche");

                // ✅ MODALITÀ
                lblMode.Text = LanguageManager.Get("Configuration.LblMode", "Modalità:");
                rbModeRadio.Text = LanguageManager.Get("Configuration.ModeRadio", "📻 Radio");
                rbModeRadioTV.Text = LanguageManager.Get("Configuration.ModeRadioTV", "📺 Radio-TV");

                // ✅ TAB PERCORSI
                grpPaths.Text = LanguageManager.Get("Configuration.GrpPaths", "📂 Percorsi Sistema");

                lblDatabasePath.Text = LanguageManager.Get("Configuration.LblDatabasePath", "Database:");
                lblStationsPath.Text = LanguageManager.Get("Configuration.LblStationsPath", "Emittenti:");
                lblMediaPath.Text = LanguageManager.Get("Configuration.LblMediaPath", "Media:");
                lblBackupPath.Text = LanguageManager.Get("Configuration.LblBackupPath", "Backup:");
                lblReportsPath.Text = LanguageManager.Get("Configuration.LblReportsPath", "Report:");
                lblLogsPath.Text = LanguageManager.Get("Configuration.LblLogsPath", "Log:");

                string openText = LanguageManager.Get("Configuration.BtnOpenFolder", "📂 Apri");
                btnOpenDatabase.Text = openText;
                btnOpenStations.Text = openText;
                btnOpenMedia.Text = openText;
                btnOpenBackup.Text = openText;
                btnOpenReports.Text = openText;
                btnOpenLogs.Text = openText;

                // ✅ TAB BACKUP
                grpBackupSettings.Text = LanguageManager.Get("Configuration.GrpBackupSettings", "💾 Impostazioni Backup");
                chkAutoBackup.Text = LanguageManager.Get("Configuration.ChkAutoBackup", "Backup automatico all'avvio");
                lblBackupRetentionDays.Text = LanguageManager.Get("Configuration.LblBackupRetentionDays", "Conserva backup per (giorni):");
                btnBackupNow.Text = LanguageManager.Get("Configuration.BtnBackupNow", "💾 Esegui Backup Ora");
                btnCleanOldBackups.Text = LanguageManager.Get("Configuration.BtnCleanOldBackups", "🗑️ Pulisci Backup Vecchi");

                // ✅ TAB LICENZA
                tabLicense.Text = "   " + LanguageManager.Get("Configuration.TabLicense", "🔑 Licenza") + "   ";
                grpLicense.Text = LanguageManager.Get("Configuration.GrpLicense", "🔑 Gestione Licenza");
                lblLicenseInfo.Text = LanguageManager.Get("Configuration.LblLicenseInfo", "Gestisci la tua licenza AirADV: visualizza i dettagli, attiva una nuova licenza o rimuovi quella esistente.");
                btnLicenseInfo.Text = LanguageManager.Get("Configuration.BtnLicenseInfo", "ℹ️ Informazioni Licenza");
                btnLicenseActivate.Text = LanguageManager.Get("Configuration.BtnLicenseActivate", "🔑 Attiva Licenza");
                btnLicenseRemove.Text = LanguageManager.Get("Configuration.BtnLicenseRemove", "🗑️ Rimuovi Licenza");

                // ✅ TAB AUDIO
                grpAudioSettings.Text = LanguageManager.Get("Configuration.GrpAudioSettings", "🔊 Impostazioni Audio");
                lblOutputDevice.Text = LanguageManager.Get("Configuration.LblOutputDevice", "Dispositivo Output:");
                lblMiniPlayerVolume.Text = LanguageManager.Get("Configuration.LblMiniPlayerVolume", "Volume Mini Player:");

                // ✅ BOTTONI INFERIORI
                btnSave.Text = LanguageManager.Get("Common.Save", "💾 Salva");
                btnCancel.Text = LanguageManager.Get("Common.Cancel", "✖ Annulla");
                btnApply.Text = LanguageManager.Get("Common.Apply", "✓ Applica");

                // ✅ RICARICA ELEMENTI DINAMICI
                LoadAudioDevices();

                // ✅ FORZA AGGIORNAMENTO VISIVO
                UpdateLicenseButtonsVisibility();
                this.Refresh();
                tabMain.Refresh();
                grpPaths.Refresh();
                grpBackupSettings.Refresh();

                Console.WriteLine("[ConfigurationForm] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigurationForm] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════════
        // EVENTI
        // ═══════════════════════════════════════════════════════════

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
        }

        private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
        }

        private void chkAutoBackup_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
            numBackupRetentionDays.Enabled = chkAutoBackup.Checked;
        }

        private void numBackupRetentionDays_ValueChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
        }

        private void cmbOutputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoading) _isDirty = true;
        }

        private void trackMiniPlayerVolume_Scroll(object sender, EventArgs e)
        {
            lblVolumeValue.Text = $"{trackMiniPlayerVolume.Value}%";
            if (!_isLoading) _isDirty = true;
        }

        // ═══════════════════════════════════════════════════════════
        // APRI CARTELLE
        // ═══════════════════════════════════════════════════════════

        private void btnOpenDatabase_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.DATABASE_PATH);
        }

        private void btnOpenStations_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.STATIONS_PATH);
        }

        private void btnOpenMedia_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.MediaLibraryPath);
        }

        private void btnOpenBackup_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.BACKUP_PATH);
        }

        private void btnOpenReports_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.PdfOutputPath);
        }

        private void btnOpenLogs_Click(object sender, EventArgs e)
        {
            OpenFolder(ConfigManager.LogsPath);
        }

        private void OpenFolder(string path)
        {
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.CannotOpenFolder", "Impossibile aprire la cartella")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ═══════════════════════════════════════════════════════════
        // BACKUP
        // ═══════════════════════════════════════════════════════════

        private void btnBackupNow_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfigManager.CurrentStationID <= 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("Messages.NoStationSelected", "Nessuna emittente selezionata!"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.ConfirmBackup", "Eseguire il backup del database dell'emittente corrente?"),
                    LanguageManager.Get("Messages.ConfirmBackupTitle", "Conferma Backup"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    bool success = DbcManager.BackupDatabase(ConfigManager.CurrentStationID);

                    if (success)
                    {
                        MessageBox.Show(
                            LanguageManager.Get("Messages.BackupSuccess", "✅ Backup completato con successo!"),
                            LanguageManager.Get("Common.Success", "Successo"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            LanguageManager.Get("Messages.BackupError", "❌ Errore durante il backup!"),
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
                    $"{LanguageManager.Get("Messages.BackupError", "Errore backup")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCleanOldBackups_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    string.Format(
                        LanguageManager.Get("Messages.ConfirmCleanBackups", "Eliminare tutti i backup più vecchi di {0} giorni?"),
                        numBackupRetentionDays.Value
                    ),
                    LanguageManager.Get("Messages.ConfirmCleanTitle", "Conferma Pulizia"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    ConfigManager.CleanOldBackups();
                    MessageBox.Show(
                        LanguageManager.Get("Messages.CleanBackupsSuccess", "✅ Pulizia backup completata! "),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.CleanBackupsError", "Errore pulizia backup")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ═══════════════════════════════════════════════════════════
        // SALVATAGGIO
        // ═══════════════════════════════════════════════════════════

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.UnsavedChangesExit", "Ci sono modifiche non salvate.Vuoi uscire comunque?"),
                    LanguageManager.Get("Common.Confirm", "Conferma"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SaveSettings()
        {
            try
            {
                // ✅ Salva lingua (nome file, non codice)
                if (cmbLanguage.SelectedItem is LanguageItem langItem)
                {
                    if (ConfigManager.Language != langItem.FileName)
                    {
                        Console.WriteLine($"[ConfigurationForm] Cambio lingua:  {ConfigManager.Language} → {langItem.FileName}");

                        // ✅ Salva nome file (es: "Italian", "English")
                        ConfigManager.Language = langItem.FileName;

                        // ✅ Carica lingua usando nome file
                        LanguageManager.LoadLanguageByFileName(langItem.FileName);
                    }
                }

                ConfigManager.AutoSave = chkAutoSave.Checked;

                // ✅ Salva Modalità nel Registro
                string newMode = rbModeRadioTV.Checked ? "Radio-TV" : "Radio";
                ConfigManager.StationMode = newMode;
                Console.WriteLine($"[ConfigurationForm] Modalità salvata: {newMode}");

                ConfigManager.AutoBackup = chkAutoBackup.Checked;
                ConfigManager.BackupRetentionDays = (int)numBackupRetentionDays.Value;

                if (cmbOutputDevice.SelectedItem is AudioDeviceItem audioItem)
                {
                    ConfigManager.OutputDeviceNumber = audioItem.DeviceNumber;
                }

                ConfigManager.MiniPlayerVolume = trackMiniPlayerVolume.Value;
                ConfigManager.Save();

                _isDirty = false;

                MessageBox.Show(
                    LanguageManager.Get("Messages.SettingsSaved", "✅ Impostazioni salvate con successo!"),
                    LanguageManager.Get("Common.Success", "Successo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                Console.WriteLine("[ConfigurationForm] ✅ Impostazioni salvate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigurationForm] ❌ Errore salvataggio: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("Messages.SaveError", "Errore salvataggio impostazioni")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_isDirty && e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.UnsavedChangesSave", "Ci sono modifiche non salvate.Vuoi salvarle? "),
                    LanguageManager.Get("Messages.UnsavedChangesTitle", "Modifiche Non Salvate"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    SaveSettings();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }

        // ═══════════════════════════════════════════════════════════
        // GESTIONE LICENZA
        // ═══════════════════════════════════════════════════════════

        private void btnLicenseInfo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new LicenseInfoForm())
                {
                    form.ShowDialog(this);
                }
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

        private void btnLicenseActivate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new LicenseForm())
                {
                    form.ShowDialog(this);
                }
                UpdateLicenseButtonsVisibility();
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

        private void btnLicenseRemove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new LicenseRemoveConfirmForm())
                {
                    form.ShowDialog(this);
                }
                UpdateLicenseButtonsVisibility();
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
        // CLASSI HELPER
        // ═══════════════════════════════════════════════════════════

        private void UpdateLicenseButtonsVisibility()
        {
            bool isLicensed = LicenseManager.IsLicenseValid();
            btnLicenseActivate.Visible = !isLicensed;
            btnLicenseRemove.Visible = isLicensed;
        }

        private class LanguageItem
        {
            public string FileName { get; set; }      // Es: "Italiano", "English"
            public string DisplayName { get; set; }   // Es: "Italiano", "English"
            public string Code { get; set; }          // Es: "it-IT", "en-US" (per riferimento)

            public override string ToString() => DisplayName;
        }

        private class AudioDeviceItem
        {
            public int DeviceNumber { get; set; }
            public string DeviceName { get; set; }

            public override string ToString() => DeviceName;
        }
    }
}