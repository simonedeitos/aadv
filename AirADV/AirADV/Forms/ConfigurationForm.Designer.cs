namespace AirADV.Forms
{
    partial class ConfigurationForm
    {
        private System.ComponentModel.IContainer components = null;

        // Tab Control
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabPaths;
        private System.Windows.Forms.TabPage tabBackup;
        private System.Windows.Forms.TabPage tabAudio;

        // Tab Generale
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.CheckBox chkAutoSave;

        // ✅ NUOVO: Modalità Radio / Radio-TV
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rbModeRadio;
        private System.Windows.Forms.RadioButton rbModeRadioTV;

        // Tab Percorsi
        private System.Windows.Forms.GroupBox grpPaths;
        private System.Windows.Forms.Label lblDatabasePath;
        private System.Windows.Forms.Label lblDatabasePathValue;
        private System.Windows.Forms.Button btnOpenDatabase;
        private System.Windows.Forms.Label lblStationsPath;
        private System.Windows.Forms.Label lblStationsPathValue;
        private System.Windows.Forms.Button btnOpenStations;
        private System.Windows.Forms.Label lblMediaPath;
        private System.Windows.Forms.Label lblMediaPathValue;
        private System.Windows.Forms.Button btnOpenMedia;
        private System.Windows.Forms.Label lblBackupPath;
        private System.Windows.Forms.Label lblBackupPathValue;
        private System.Windows.Forms.Button btnOpenBackup;
        private System.Windows.Forms.Label lblReportsPath;
        private System.Windows.Forms.Label lblReportsPathValue;
        private System.Windows.Forms.Button btnOpenReports;
        private System.Windows.Forms.Label lblLogsPath;
        private System.Windows.Forms.Label lblLogsPathValue;
        private System.Windows.Forms.Button btnOpenLogs;

        // Tab Backup
        private System.Windows.Forms.GroupBox grpBackupSettings;
        private System.Windows.Forms.CheckBox chkAutoBackup;
        private System.Windows.Forms.Label lblBackupRetentionDays;
        private System.Windows.Forms.NumericUpDown numBackupRetentionDays;
        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.Button btnCleanOldBackups;

        // Tab Audio
        private System.Windows.Forms.GroupBox grpAudioSettings;
        private System.Windows.Forms.Label lblOutputDevice;
        private System.Windows.Forms.ComboBox cmbOutputDevice;
        private System.Windows.Forms.Label lblMiniPlayerVolume;
        private System.Windows.Forms.TrackBar trackMiniPlayerVolume;
        private System.Windows.Forms.Label lblVolumeValue;

        // Bottoni
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabPaths = new System.Windows.Forms.TabPage();
            this.tabBackup = new System.Windows.Forms.TabPage();
            this.tabAudio = new System.Windows.Forms.TabPage();

            // Tab Generale
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();

            // ✅ NUOVO: Modalità
            this.lblMode = new System.Windows.Forms.Label();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rbModeRadio = new System.Windows.Forms.RadioButton();
            this.rbModeRadioTV = new System.Windows.Forms.RadioButton();

            // Tab Percorsi
            this.grpPaths = new System.Windows.Forms.GroupBox();
            this.lblDatabasePath = new System.Windows.Forms.Label();
            this.lblDatabasePathValue = new System.Windows.Forms.Label();
            this.btnOpenDatabase = new System.Windows.Forms.Button();
            this.lblStationsPath = new System.Windows.Forms.Label();
            this.lblStationsPathValue = new System.Windows.Forms.Label();
            this.btnOpenStations = new System.Windows.Forms.Button();
            this.lblMediaPath = new System.Windows.Forms.Label();
            this.lblMediaPathValue = new System.Windows.Forms.Label();
            this.btnOpenMedia = new System.Windows.Forms.Button();
            this.lblBackupPath = new System.Windows.Forms.Label();
            this.lblBackupPathValue = new System.Windows.Forms.Label();
            this.btnOpenBackup = new System.Windows.Forms.Button();
            this.lblReportsPath = new System.Windows.Forms.Label();
            this.lblReportsPathValue = new System.Windows.Forms.Label();
            this.btnOpenReports = new System.Windows.Forms.Button();
            this.lblLogsPath = new System.Windows.Forms.Label();
            this.lblLogsPathValue = new System.Windows.Forms.Label();
            this.btnOpenLogs = new System.Windows.Forms.Button();

            // Tab Backup
            this.grpBackupSettings = new System.Windows.Forms.GroupBox();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            this.lblBackupRetentionDays = new System.Windows.Forms.Label();
            this.numBackupRetentionDays = new System.Windows.Forms.NumericUpDown();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.btnCleanOldBackups = new System.Windows.Forms.Button();

            // Tab Audio
            this.grpAudioSettings = new System.Windows.Forms.GroupBox();
            this.lblOutputDevice = new System.Windows.Forms.Label();
            this.cmbOutputDevice = new System.Windows.Forms.ComboBox();
            this.lblMiniPlayerVolume = new System.Windows.Forms.Label();
            this.trackMiniPlayerVolume = new System.Windows.Forms.TrackBar();
            this.lblVolumeValue = new System.Windows.Forms.Label();

            // Bottoni
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();

            this.tabMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.tabPaths.SuspendLayout();
            this.grpPaths.SuspendLayout();
            this.tabBackup.SuspendLayout();
            this.grpBackupSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupRetentionDays)).BeginInit();
            this.tabAudio.SuspendLayout();
            this.grpAudioSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMiniPlayerVolume)).BeginInit();
            this.SuspendLayout();

            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabGeneral);
            this.tabMain.Controls.Add(this.tabPaths);
            this.tabMain.Controls.Add(this.tabBackup);
            // this.tabMain.Controls.Add(this.tabAudio);     // TAB AUDIO DISATTIVATA
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(700, 500);
            this.tabMain.TabIndex = 0;

            // ═══════════════════════════════════════════════════════════
            // TAB GENERALE
            // ═══════════════════════════════════════════════════════════

            this.tabGeneral.BackColor = System.Drawing.Color.White;
            this.tabGeneral.Controls.Add(this.lblLanguage);
            this.tabGeneral.Controls.Add(this.cmbLanguage);
            this.tabGeneral.Controls.Add(this.chkAutoSave);
            this.tabGeneral.Controls.Add(this.lblMode);
            this.tabGeneral.Controls.Add(this.grpMode);
            this.tabGeneral.Location = new System.Drawing.Point(4, 28);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(20);
            this.tabGeneral.Size = new System.Drawing.Size(692, 468);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "🌐 Generale";

            // lblLanguage
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLanguage.Location = new System.Drawing.Point(30, 30);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(55, 19);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "Lingua:";

            // cmbLanguage
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(150, 27);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(250, 25);
            this.cmbLanguage.TabIndex = 1;
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);

            // chkAutoSave
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkAutoSave.Location = new System.Drawing.Point(30, 80);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(250, 23);
            this.chkAutoSave.TabIndex = 2;
            this.chkAutoSave.Text = "Salvataggio automatico modifiche";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);

            // ═══════════════════════════════════════════════════════════
            // ✅ NUOVO: MODALITÀ (Radio / Radio-TV)
            // ═══════════════════════════════════════════════════════════

            // lblMode
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMode.Location = new System.Drawing.Point(30, 135);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(72, 19);
            this.lblMode.TabIndex = 3;
            this.lblMode.Text = "Modalità:";

            // grpMode (GroupBox senza bordo visibile, serve solo per raggruppare i RadioButton)
            this.grpMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpMode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.grpMode.Location = new System.Drawing.Point(135, 115);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(320, 55);
            this.grpMode.TabIndex = 4;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "";

            // rbModeRadio
            this.rbModeRadio.AutoSize = true;
            this.rbModeRadio.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rbModeRadio.Location = new System.Drawing.Point(15, 20);
            this.rbModeRadio.Name = "rbModeRadio";
            this.rbModeRadio.Size = new System.Drawing.Size(90, 23);
            this.rbModeRadio.TabIndex = 0;
            this.rbModeRadio.Checked = true;
            this.rbModeRadio.Text = "📻 Radio";
            this.rbModeRadio.UseVisualStyleBackColor = true;
            this.rbModeRadio.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);

            // rbModeRadioTV
            this.rbModeRadioTV.AutoSize = true;
            this.rbModeRadioTV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rbModeRadioTV.Location = new System.Drawing.Point(155, 20);
            this.rbModeRadioTV.Name = "rbModeRadioTV";
            this.rbModeRadioTV.Size = new System.Drawing.Size(110, 23);
            this.rbModeRadioTV.TabIndex = 1;
            this.rbModeRadioTV.Text = "📺 Radio-TV";
            this.rbModeRadioTV.UseVisualStyleBackColor = true;
            this.rbModeRadioTV.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);

            // Aggiungi RadioButton al GroupBox
            this.grpMode.Controls.Add(this.rbModeRadio);
            this.grpMode.Controls.Add(this.rbModeRadioTV);

            // ═══════════════════════════════════════════════════════════
            // TAB PERCORSI
            // ═══════════════════════════════════════════════════════════

            this.tabPaths.BackColor = System.Drawing.Color.White;
            this.tabPaths.Controls.Add(this.grpPaths);
            this.tabPaths.Location = new System.Drawing.Point(4, 28);
            this.tabPaths.Name = "tabPaths";
            this.tabPaths.Padding = new System.Windows.Forms.Padding(20);
            this.tabPaths.Size = new System.Drawing.Size(692, 468);
            this.tabPaths.TabIndex = 1;
            this.tabPaths.Text = "📁 Percorsi";

            // grpPaths
            this.grpPaths.Controls.Add(this.lblDatabasePath);
            this.grpPaths.Controls.Add(this.lblDatabasePathValue);
            this.grpPaths.Controls.Add(this.btnOpenDatabase);
            this.grpPaths.Controls.Add(this.lblStationsPath);
            this.grpPaths.Controls.Add(this.lblStationsPathValue);
            this.grpPaths.Controls.Add(this.btnOpenStations);
            this.grpPaths.Controls.Add(this.lblMediaPath);
            this.grpPaths.Controls.Add(this.lblMediaPathValue);
            this.grpPaths.Controls.Add(this.btnOpenMedia);
            this.grpPaths.Controls.Add(this.lblBackupPath);
            this.grpPaths.Controls.Add(this.lblBackupPathValue);
            this.grpPaths.Controls.Add(this.btnOpenBackup);
            this.grpPaths.Controls.Add(this.lblReportsPath);
            this.grpPaths.Controls.Add(this.lblReportsPathValue);
            this.grpPaths.Controls.Add(this.btnOpenReports);
            this.grpPaths.Controls.Add(this.lblLogsPath);
            this.grpPaths.Controls.Add(this.lblLogsPathValue);
            this.grpPaths.Controls.Add(this.btnOpenLogs);
            this.grpPaths.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpPaths.Location = new System.Drawing.Point(20, 20);
            this.grpPaths.Name = "grpPaths";
            this.grpPaths.Size = new System.Drawing.Size(650, 420);
            this.grpPaths.TabIndex = 0;
            this.grpPaths.TabStop = false;
            this.grpPaths.Text = "📂 Percorsi Sistema";

            int yPos = 35;
            int yStep = 50;

            // Database
            this.lblDatabasePath.AutoSize = true;
            this.lblDatabasePath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDatabasePath.Location = new System.Drawing.Point(20, yPos);
            this.lblDatabasePath.Name = "lblDatabasePath";
            this.lblDatabasePath.Size = new System.Drawing.Size(65, 15);
            this.lblDatabasePath.TabIndex = 0;
            this.lblDatabasePath.Text = "Database:";

            this.lblDatabasePathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDatabasePathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblDatabasePathValue.Name = "lblDatabasePathValue";
            this.lblDatabasePathValue.Size = new System.Drawing.Size(420, 15);
            this.lblDatabasePathValue.TabIndex = 1;
            this.lblDatabasePathValue.Text = "C:\\AirADV\\Database";

            this.btnOpenDatabase.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenDatabase.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenDatabase.Name = "btnOpenDatabase";
            this.btnOpenDatabase.Size = new System.Drawing.Size(80, 23);
            this.btnOpenDatabase.TabIndex = 2;
            this.btnOpenDatabase.Text = "📂 Apri";
            this.btnOpenDatabase.UseVisualStyleBackColor = true;
            this.btnOpenDatabase.Click += new System.EventHandler(this.btnOpenDatabase_Click);

            yPos += yStep;

            // Stations
            this.lblStationsPath.AutoSize = true;
            this.lblStationsPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStationsPath.Location = new System.Drawing.Point(20, yPos);
            this.lblStationsPath.Name = "lblStationsPath";
            this.lblStationsPath.Size = new System.Drawing.Size(65, 15);
            this.lblStationsPath.TabIndex = 3;
            this.lblStationsPath.Text = "Emittenti:";

            this.lblStationsPathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStationsPathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblStationsPathValue.Name = "lblStationsPathValue";
            this.lblStationsPathValue.Size = new System.Drawing.Size(420, 15);
            this.lblStationsPathValue.TabIndex = 4;
            this.lblStationsPathValue.Text = "C:\\AirADV\\Stations";

            this.btnOpenStations.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenStations.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenStations.Name = "btnOpenStations";
            this.btnOpenStations.Size = new System.Drawing.Size(80, 23);
            this.btnOpenStations.TabIndex = 5;
            this.btnOpenStations.Text = "📂 Apri";
            this.btnOpenStations.UseVisualStyleBackColor = true;
            this.btnOpenStations.Click += new System.EventHandler(this.btnOpenStations_Click);

            yPos += yStep;

            // Media
            this.lblMediaPath.AutoSize = true;
            this.lblMediaPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMediaPath.Location = new System.Drawing.Point(20, yPos);
            this.lblMediaPath.Name = "lblMediaPath";
            this.lblMediaPath.Size = new System.Drawing.Size(45, 15);
            this.lblMediaPath.TabIndex = 6;
            this.lblMediaPath.Text = "Media:";

            this.lblMediaPathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMediaPathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblMediaPathValue.Name = "lblMediaPathValue";
            this.lblMediaPathValue.Size = new System.Drawing.Size(420, 15);
            this.lblMediaPathValue.TabIndex = 7;
            this.lblMediaPathValue.Text = "C:\\AirADV\\Media";

            this.btnOpenMedia.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenMedia.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenMedia.Name = "btnOpenMedia";
            this.btnOpenMedia.Size = new System.Drawing.Size(80, 23);
            this.btnOpenMedia.TabIndex = 8;
            this.btnOpenMedia.Text = "📂 Apri";
            this.btnOpenMedia.UseVisualStyleBackColor = true;
            this.btnOpenMedia.Click += new System.EventHandler(this.btnOpenMedia_Click);

            yPos += yStep;

            // Backup
            this.lblBackupPath.AutoSize = true;
            this.lblBackupPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBackupPath.Location = new System.Drawing.Point(20, yPos);
            this.lblBackupPath.Name = "lblBackupPath";
            this.lblBackupPath.Size = new System.Drawing.Size(50, 15);
            this.lblBackupPath.TabIndex = 9;
            this.lblBackupPath.Text = "Backup:";

            this.lblBackupPathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBackupPathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblBackupPathValue.Name = "lblBackupPathValue";
            this.lblBackupPathValue.Size = new System.Drawing.Size(420, 15);
            this.lblBackupPathValue.TabIndex = 10;
            this.lblBackupPathValue.Text = "C:\\AirADV\\Backups";

            this.btnOpenBackup.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenBackup.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenBackup.Name = "btnOpenBackup";
            this.btnOpenBackup.Size = new System.Drawing.Size(80, 23);
            this.btnOpenBackup.TabIndex = 11;
            this.btnOpenBackup.Text = "📂 Apri";
            this.btnOpenBackup.UseVisualStyleBackColor = true;
            this.btnOpenBackup.Click += new System.EventHandler(this.btnOpenBackup_Click);

            yPos += yStep;

            // Reports
            this.lblReportsPath.AutoSize = true;
            this.lblReportsPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblReportsPath.Location = new System.Drawing.Point(20, yPos);
            this.lblReportsPath.Name = "lblReportsPath";
            this.lblReportsPath.Size = new System.Drawing.Size(50, 15);
            this.lblReportsPath.TabIndex = 12;
            this.lblReportsPath.Text = "Report:";

            this.lblReportsPathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblReportsPathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblReportsPathValue.Name = "lblReportsPathValue";
            this.lblReportsPathValue.Size = new System.Drawing.Size(420, 15);
            this.lblReportsPathValue.TabIndex = 13;
            this.lblReportsPathValue.Text = "C:\\AirADV\\Reports";

            this.btnOpenReports.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenReports.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenReports.Name = "btnOpenReports";
            this.btnOpenReports.Size = new System.Drawing.Size(80, 23);
            this.btnOpenReports.TabIndex = 14;
            this.btnOpenReports.Text = "📂 Apri";
            this.btnOpenReports.UseVisualStyleBackColor = true;
            this.btnOpenReports.Click += new System.EventHandler(this.btnOpenReports_Click);

            yPos += yStep;

            // Logs
            this.lblLogsPath.AutoSize = true;
            this.lblLogsPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLogsPath.Location = new System.Drawing.Point(20, yPos);
            this.lblLogsPath.Name = "lblLogsPath";
            this.lblLogsPath.Size = new System.Drawing.Size(35, 15);
            this.lblLogsPath.TabIndex = 15;
            this.lblLogsPath.Text = "Log:";

            this.lblLogsPathValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLogsPathValue.Location = new System.Drawing.Point(120, yPos);
            this.lblLogsPathValue.Name = "lblLogsPathValue";
            this.lblLogsPathValue.Size = new System.Drawing.Size(420, 15);
            this.lblLogsPathValue.TabIndex = 16;
            this.lblLogsPathValue.Text = "C:\\AirADV\\Logs";

            this.btnOpenLogs.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenLogs.Location = new System.Drawing.Point(550, yPos - 3);
            this.btnOpenLogs.Name = "btnOpenLogs";
            this.btnOpenLogs.Size = new System.Drawing.Size(80, 23);
            this.btnOpenLogs.TabIndex = 17;
            this.btnOpenLogs.Text = "📂 Apri";
            this.btnOpenLogs.UseVisualStyleBackColor = true;
            this.btnOpenLogs.Click += new System.EventHandler(this.btnOpenLogs_Click);

            // ═══════════════════════════════════════════════════════════
            // TAB BACKUP
            // ═══════════════════════════════════════════════════════════

            this.tabBackup.BackColor = System.Drawing.Color.White;
            this.tabBackup.Controls.Add(this.grpBackupSettings);
            this.tabBackup.Location = new System.Drawing.Point(4, 28);
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Padding = new System.Windows.Forms.Padding(20);
            this.tabBackup.Size = new System.Drawing.Size(692, 468);
            this.tabBackup.TabIndex = 2;
            this.tabBackup.Text = "💾 Backup";

            // grpBackupSettings
            this.grpBackupSettings.Controls.Add(this.chkAutoBackup);
            this.grpBackupSettings.Controls.Add(this.lblBackupRetentionDays);
            this.grpBackupSettings.Controls.Add(this.numBackupRetentionDays);
            this.grpBackupSettings.Controls.Add(this.btnBackupNow);
            this.grpBackupSettings.Controls.Add(this.btnCleanOldBackups);
            this.grpBackupSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpBackupSettings.Location = new System.Drawing.Point(20, 20);
            this.grpBackupSettings.Name = "grpBackupSettings";
            this.grpBackupSettings.Size = new System.Drawing.Size(650, 250);
            this.grpBackupSettings.TabIndex = 0;
            this.grpBackupSettings.TabStop = false;
            this.grpBackupSettings.Text = "💾 Impostazioni Backup";

            // chkAutoBackup
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkAutoBackup.Location = new System.Drawing.Point(30, 40);
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size(220, 23);
            this.chkAutoBackup.TabIndex = 0;
            this.chkAutoBackup.Text = "Backup automatico all'avvio";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            this.chkAutoBackup.CheckedChanged += new System.EventHandler(this.chkAutoBackup_CheckedChanged);

            // lblBackupRetentionDays
            this.lblBackupRetentionDays.AutoSize = true;
            this.lblBackupRetentionDays.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBackupRetentionDays.Location = new System.Drawing.Point(30, 80);
            this.lblBackupRetentionDays.Name = "lblBackupRetentionDays";
            this.lblBackupRetentionDays.Size = new System.Drawing.Size(210, 19);
            this.lblBackupRetentionDays.TabIndex = 1;
            this.lblBackupRetentionDays.Text = "Conserva backup per (giorni):";

            // numBackupRetentionDays
            this.numBackupRetentionDays.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numBackupRetentionDays.Location = new System.Drawing.Point(250, 78);
            this.numBackupRetentionDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.numBackupRetentionDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numBackupRetentionDays.Name = "numBackupRetentionDays";
            this.numBackupRetentionDays.Size = new System.Drawing.Size(80, 25);
            this.numBackupRetentionDays.TabIndex = 2;
            this.numBackupRetentionDays.Value = new decimal(new int[] { 30, 0, 0, 0 });
            this.numBackupRetentionDays.ValueChanged += new System.EventHandler(this.numBackupRetentionDays_ValueChanged);

            // btnBackupNow
            this.btnBackupNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnBackupNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupNow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBackupNow.ForeColor = System.Drawing.Color.White;
            this.btnBackupNow.Location = new System.Drawing.Point(30, 140);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(250, 40);
            this.btnBackupNow.TabIndex = 3;
            this.btnBackupNow.Text = "💾 Esegui Backup Ora";
            this.btnBackupNow.UseVisualStyleBackColor = false;
            this.btnBackupNow.Click += new System.EventHandler(this.btnBackupNow_Click);

            // btnCleanOldBackups
            this.btnCleanOldBackups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCleanOldBackups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanOldBackups.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCleanOldBackups.ForeColor = System.Drawing.Color.White;
            this.btnCleanOldBackups.Location = new System.Drawing.Point(290, 140);
            this.btnCleanOldBackups.Name = "btnCleanOldBackups";
            this.btnCleanOldBackups.Size = new System.Drawing.Size(250, 40);
            this.btnCleanOldBackups.TabIndex = 4;
            this.btnCleanOldBackups.Text = "🗑️ Pulisci Backup Vecchi";
            this.btnCleanOldBackups.UseVisualStyleBackColor = false;
            this.btnCleanOldBackups.Click += new System.EventHandler(this.btnCleanOldBackups_Click);

            // ═══════════════════════════════════════════════════════════
            // TAB AUDIO
            // ═══════════════════════════════════════════════════════════

            this.tabAudio.BackColor = System.Drawing.Color.White;
            this.tabAudio.Controls.Add(this.grpAudioSettings);
            this.tabAudio.Location = new System.Drawing.Point(4, 28);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Padding = new System.Windows.Forms.Padding(20);
            this.tabAudio.Size = new System.Drawing.Size(692, 468);
            this.tabAudio.TabIndex = 3;
            this.tabAudio.Text = "🔊 Audio";
            this.tabAudio.Visible = false;

            // grpAudioSettings
            this.grpAudioSettings.Controls.Add(this.lblOutputDevice);
            this.grpAudioSettings.Controls.Add(this.cmbOutputDevice);
            this.grpAudioSettings.Controls.Add(this.lblMiniPlayerVolume);
            this.grpAudioSettings.Controls.Add(this.trackMiniPlayerVolume);
            this.grpAudioSettings.Controls.Add(this.lblVolumeValue);
            this.grpAudioSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpAudioSettings.Location = new System.Drawing.Point(20, 20);
            this.grpAudioSettings.Name = "grpAudioSettings";
            this.grpAudioSettings.Size = new System.Drawing.Size(650, 200);
            this.grpAudioSettings.TabIndex = 0;
            this.grpAudioSettings.TabStop = false;
            this.grpAudioSettings.Text = "🔊 Impostazioni Audio";

            // lblOutputDevice
            this.lblOutputDevice.AutoSize = true;
            this.lblOutputDevice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblOutputDevice.Location = new System.Drawing.Point(30, 40);
            this.lblOutputDevice.Name = "lblOutputDevice";
            this.lblOutputDevice.Size = new System.Drawing.Size(130, 19);
            this.lblOutputDevice.TabIndex = 0;
            this.lblOutputDevice.Text = "Dispositivo Output:";

            // cmbOutputDevice
            this.cmbOutputDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputDevice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbOutputDevice.FormattingEnabled = true;
            this.cmbOutputDevice.Location = new System.Drawing.Point(180, 37);
            this.cmbOutputDevice.Name = "cmbOutputDevice";
            this.cmbOutputDevice.Size = new System.Drawing.Size(400, 25);
            this.cmbOutputDevice.TabIndex = 1;
            this.cmbOutputDevice.SelectedIndexChanged += new System.EventHandler(this.cmbOutputDevice_SelectedIndexChanged);

            // lblMiniPlayerVolume
            this.lblMiniPlayerVolume.AutoSize = true;
            this.lblMiniPlayerVolume.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMiniPlayerVolume.Location = new System.Drawing.Point(30, 90);
            this.lblMiniPlayerVolume.Name = "lblMiniPlayerVolume";
            this.lblMiniPlayerVolume.Size = new System.Drawing.Size(140, 19);
            this.lblMiniPlayerVolume.TabIndex = 2;
            this.lblMiniPlayerVolume.Text = "Volume Mini Player:";

            // trackMiniPlayerVolume
            this.trackMiniPlayerVolume.Location = new System.Drawing.Point(180, 85);
            this.trackMiniPlayerVolume.Maximum = 100;
            this.trackMiniPlayerVolume.Name = "trackMiniPlayerVolume";
            this.trackMiniPlayerVolume.Size = new System.Drawing.Size(350, 45);
            this.trackMiniPlayerVolume.TabIndex = 3;
            this.trackMiniPlayerVolume.TickFrequency = 10;
            this.trackMiniPlayerVolume.Value = 80;
            this.trackMiniPlayerVolume.Scroll += new System.EventHandler(this.trackMiniPlayerVolume_Scroll);

            // lblVolumeValue
            this.lblVolumeValue.AutoSize = true;
            this.lblVolumeValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblVolumeValue.Location = new System.Drawing.Point(540, 90);
            this.lblVolumeValue.Name = "lblVolumeValue";
            this.lblVolumeValue.Size = new System.Drawing.Size(40, 19);
            this.lblVolumeValue.TabIndex = 4;
            this.lblVolumeValue.Text = "80%";

            // ═══════════════════════════════════════════════════════════
            // BOTTONI
            // ═══════════════════════════════════════════════════════════

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(370, 520);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 Salva";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnApply
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(480, 520);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 35);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "✓ Applica";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(590, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "✖ Annulla";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 570);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "⚙️ Configurazione";
            this.tabMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.tabPaths.ResumeLayout(false);
            this.grpPaths.ResumeLayout(false);
            this.grpPaths.PerformLayout();
            this.tabBackup.ResumeLayout(false);
            this.grpBackupSettings.ResumeLayout(false);
            this.grpBackupSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupRetentionDays)).EndInit();
            this.tabAudio.ResumeLayout(false);
            this.grpAudioSettings.ResumeLayout(false);
            this.grpAudioSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMiniPlayerVolume)).EndInit();
            this.ResumeLayout(false);
        }
    }
}