namespace AirADV.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top Panel
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox picStationLogo;
        private System.Windows.Forms.Label lblStationName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnChangeStation;

        // Main Panel - Bottoni Launcher
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnClients;
        private System.Windows.Forms.Button btnTimeSlots;
        private System.Windows.Forms.Button btnCategories;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSettings;

        // ✅ NUOVO: Bottone Lanner TV
        private System.Windows.Forms.Button btnLannerTV;

        // Bottom Panel
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnExit;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panelTop = new Panel();
            picStationLogo = new PictureBox();
            btnExit = new Button();
            lblStationName = new Label();
            lblTitle = new Label();
            btnChangeStation = new Button();
            panelMain = new Panel();
            btnClients = new Button();
            btnTimeSlots = new Button();
            btnCategories = new Button();
            btnSchedule = new Button();
            btnLannerTV = new Button();
            btnSettings = new Button();
            btnReports = new Button();
            panelBottom = new Panel();
            lblVersion = new Label();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picStationLogo).BeginInit();
            panelMain.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(45, 45, 48);
            panelTop.Controls.Add(picStationLogo);
            panelTop.Controls.Add(btnExit);
            panelTop.Controls.Add(lblStationName);
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnChangeStation);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1023, 90);
            panelTop.TabIndex = 0;
            // 
            // picStationLogo
            // 
            picStationLogo.BackColor = Color.Transparent;
            picStationLogo.BorderStyle = BorderStyle.FixedSingle;
            picStationLogo.Location = new Point(20, 10);
            picStationLogo.Name = "picStationLogo";
            picStationLogo.Size = new Size(127, 70);
            picStationLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picStationLogo.TabIndex = 0;
            picStationLogo.TabStop = false;
            picStationLogo.Visible = false;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(220, 53, 69);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(823, 27);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(140, 35);
            btnExit.TabIndex = 1;
            btnExit.Text = "✖ Esci";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // lblStationName
            // 
            lblStationName.AutoSize = true;
            lblStationName.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStationName.ForeColor = Color.White;
            lblStationName.Location = new Point(160, 30);
            lblStationName.Name = "lblStationName";
            lblStationName.Size = new Size(200, 32);
            lblStationName.TabIndex = 1;
            lblStationName.Text = "Nome Emittente";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 10F);
            lblTitle.ForeColor = Color.LightGray;
            lblTitle.Location = new Point(162, 62);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(193, 19);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Gestionale Pubblicitario v1.0.0";
            // 
            // btnChangeStation
            // 
            btnChangeStation.BackColor = Color.FromArgb(0, 123, 255);
            btnChangeStation.FlatAppearance.BorderSize = 0;
            btnChangeStation.FlatStyle = FlatStyle.Flat;
            btnChangeStation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChangeStation.ForeColor = Color.White;
            btnChangeStation.Location = new Point(630, 27);
            btnChangeStation.Name = "btnChangeStation";
            btnChangeStation.Size = new Size(160, 35);
            btnChangeStation.TabIndex = 3;
            btnChangeStation.Text = "🔄 Cambia Emittente";
            btnChangeStation.UseVisualStyleBackColor = false;
            btnChangeStation.Click += btnChangeStation_Click;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(240, 240, 245);
            panelMain.Controls.Add(btnClients);
            panelMain.Controls.Add(btnTimeSlots);
            panelMain.Controls.Add(btnCategories);
            panelMain.Controls.Add(btnSchedule);
            panelMain.Controls.Add(btnLannerTV);
            panelMain.Controls.Add(btnSettings);
            panelMain.Controls.Add(btnReports);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 90);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1023, 207);
            panelMain.TabIndex = 1;
            panelMain.Paint += panelMain_Paint;
            // 
            // btnClients
            // 
            btnClients.BackColor = Color.White;
            btnClients.BackgroundImage = (Image)resources.GetObject("btnClients.BackgroundImage");
            btnClients.BackgroundImageLayout = ImageLayout.Stretch;
            btnClients.Cursor = Cursors.Hand;
            btnClients.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnClients.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnClients.FlatStyle = FlatStyle.Flat;
            btnClients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClients.ForeColor = Color.FromArgb(45, 45, 48);
            btnClients.Location = new Point(60, 32);
            btnClients.Name = "btnClients";
            btnClients.Padding = new Padding(0, 20, 0, 10);
            btnClients.Size = new Size(140, 140);
            btnClients.TabIndex = 0;
            btnClients.Text = "Clienti";
            btnClients.TextAlign = ContentAlignment.BottomCenter;
            btnClients.UseVisualStyleBackColor = false;
            btnClients.Click += btnClients_Click;
            // 
            // btnTimeSlots
            // 
            btnTimeSlots.BackColor = Color.White;
            btnTimeSlots.BackgroundImage = (Image)resources.GetObject("btnTimeSlots.BackgroundImage");
            btnTimeSlots.BackgroundImageLayout = ImageLayout.Stretch;
            btnTimeSlots.Cursor = Cursors.Hand;
            btnTimeSlots.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnTimeSlots.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnTimeSlots.FlatStyle = FlatStyle.Flat;
            btnTimeSlots.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTimeSlots.ForeColor = Color.FromArgb(45, 45, 48);
            btnTimeSlots.Location = new Point(478, 32);
            btnTimeSlots.Name = "btnTimeSlots";
            btnTimeSlots.Padding = new Padding(0, 20, 0, 10);
            btnTimeSlots.Size = new Size(140, 140);
            btnTimeSlots.TabIndex = 1;
            btnTimeSlots.Text = "\r\n\r\nPunti Orari";
            btnTimeSlots.TextAlign = ContentAlignment.BottomCenter;
            btnTimeSlots.UseVisualStyleBackColor = false;
            btnTimeSlots.Click += btnTimeSlots_Click;
            // 
            // btnCategories
            // 
            btnCategories.BackColor = Color.White;
            btnCategories.BackgroundImage = (Image)resources.GetObject("btnCategories.BackgroundImage");
            btnCategories.BackgroundImageLayout = ImageLayout.Stretch;
            btnCategories.Cursor = Cursors.Hand;
            btnCategories.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnCategories.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnCategories.FlatStyle = FlatStyle.Flat;
            btnCategories.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCategories.ForeColor = Color.FromArgb(45, 45, 48);
            btnCategories.Location = new Point(650, 32);
            btnCategories.Name = "btnCategories";
            btnCategories.Padding = new Padding(0, 20, 0, 10);
            btnCategories.Size = new Size(140, 140);
            btnCategories.TabIndex = 2;
            btnCategories.Text = "\r\n\r\nCategorie";
            btnCategories.TextAlign = ContentAlignment.BottomCenter;
            btnCategories.UseVisualStyleBackColor = false;
            btnCategories.Click += btnCategories_Click;
            // 
            // btnSchedule
            // 
            btnSchedule.BackColor = Color.White;
            btnSchedule.BackgroundImage = (Image)resources.GetObject("btnSchedule.BackgroundImage");
            btnSchedule.BackgroundImageLayout = ImageLayout.Stretch;
            btnSchedule.Cursor = Cursors.Hand;
            btnSchedule.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnSchedule.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnSchedule.FlatStyle = FlatStyle.Flat;
            btnSchedule.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSchedule.ForeColor = Color.FromArgb(45, 45, 48);
            btnSchedule.Location = new Point(232, 32);
            btnSchedule.Name = "btnSchedule";
            btnSchedule.Padding = new Padding(0, 20, 0, 10);
            btnSchedule.Size = new Size(140, 140);
            btnSchedule.TabIndex = 3;
            btnSchedule.Text = "\r\n\r\nPalinsesto";
            btnSchedule.TextAlign = ContentAlignment.BottomCenter;
            btnSchedule.UseVisualStyleBackColor = false;
            btnSchedule.Click += btnSchedule_Click;
            // 
            // btnLannerTV
            // 
            btnLannerTV.BackColor = Color.White;
            btnLannerTV.Cursor = Cursors.Hand;
            btnLannerTV.FlatAppearance.BorderColor = Color.FromArgb(142, 36, 170);
            btnLannerTV.FlatAppearance.MouseOverBackColor = Color.FromArgb(186, 104, 200);
            btnLannerTV.FlatStyle = FlatStyle.Flat;
            btnLannerTV.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLannerTV.ForeColor = Color.Black;
            btnLannerTV.Location = new Point(823, 32);
            btnLannerTV.Name = "btnLannerTV";
            btnLannerTV.Size = new Size(140, 65);
            btnLannerTV.TabIndex = 6;
            btnLannerTV.Text = "📺 Lanner TV";
            btnLannerTV.UseVisualStyleBackColor = false;
            btnLannerTV.Visible = false;
            btnLannerTV.Click += btnLannerTV_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.White;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSettings.ForeColor = Color.FromArgb(45, 45, 48);
            btnSettings.Location = new Point(823, 32);
            btnSettings.Name = "btnSettings";
            btnSettings.Padding = new Padding(0, 20, 0, 10);
            btnSettings.Size = new Size(140, 140);
            btnSettings.TabIndex = 5;
            btnSettings.Text = "\nImpostazioni";
            btnSettings.TextAlign = ContentAlignment.BottomCenter;
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.White;
            btnReports.Cursor = Cursors.Hand;
            btnReports.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReports.ForeColor = Color.FromArgb(45, 45, 48);
            btnReports.Location = new Point(924, 112);
            btnReports.Name = "btnReports";
            btnReports.Padding = new Padding(0, 20, 0, 10);
            btnReports.Size = new Size(71, 60);
            btnReports.TabIndex = 4;
            btnReports.Text = "Report";
            btnReports.UseVisualStyleBackColor = false;
            btnReports.Visible = false;
            btnReports.Click += btnReports_Click;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(45, 45, 48);
            panelBottom.Controls.Add(lblVersion);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 297);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1023, 50);
            panelBottom.TabIndex = 2;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 9F);
            lblVersion.ForeColor = Color.LightGray;
            lblVersion.Location = new Point(20, 17);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(86, 15);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "AirADV © 2025";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1023, 347);
            Controls.Add(panelMain);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AirADV - Advertising Scheduler";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picStationLogo).EndInit();
            panelMain.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}