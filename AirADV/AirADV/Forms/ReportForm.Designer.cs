namespace AirADV.Forms
{
    partial class ReportForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top Panel
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;

        // Filters
        private System.Windows.Forms.GroupBox grpFilters;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.Label lblCampaign;
        private System.Windows.Forms.ComboBox cmbCampaign;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblLoading;

        // Tab Control
        private System.Windows.Forms.TabControl tabReports;
        private System.Windows.Forms.TabPage tabStatistics;
        private System.Windows.Forms.TabPage tabByCampaign;
        private System.Windows.Forms.TabPage tabByClient;
        private System.Windows.Forms.TabPage tabByCategory;

        // Statistics Tab
        private System.Windows.Forms.Panel panelStatistics;
        private System.Windows.Forms.Label lblTotalDays;
        private System.Windows.Forms.Label lblTotalPasses;
        private System.Windows.Forms.Label lblTotalDuration;
        private System.Windows.Forms.Label lblUniqueCampaigns;
        private System.Windows.Forms.Label lblUniqueClients;
        private System.Windows.Forms.Label lblUniqueSpots;
        private System.Windows.Forms.Label lblAvgPassesPerDay;
        private System.Windows.Forms.Label lblAvgDurationPerDay;

        // DataGridViews
        private System.Windows.Forms.DataGridView dgvCampaigns;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.DataGridView dgvCategories;

        // Bottom Panel
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportCsv;

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

            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblClient = new System.Windows.Forms.Label();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.lblCampaign = new System.Windows.Forms.Label();
            this.cmbCampaign = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblLoading = new System.Windows.Forms.Label();

            this.tabReports = new System.Windows.Forms.TabControl();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.tabByCampaign = new System.Windows.Forms.TabPage();
            this.tabByClient = new System.Windows.Forms.TabPage();
            this.tabByCategory = new System.Windows.Forms.TabPage();

            this.panelStatistics = new System.Windows.Forms.Panel();
            this.lblTotalDays = new System.Windows.Forms.Label();
            this.lblTotalPasses = new System.Windows.Forms.Label();
            this.lblTotalDuration = new System.Windows.Forms.Label();
            this.lblUniqueCampaigns = new System.Windows.Forms.Label();
            this.lblUniqueClients = new System.Windows.Forms.Label();
            this.lblUniqueSpots = new System.Windows.Forms.Label();
            this.lblAvgPassesPerDay = new System.Windows.Forms.Label();
            this.lblAvgDurationPerDay = new System.Windows.Forms.Label();

            this.dgvCampaigns = new System.Windows.Forms.DataGridView();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.dgvCategories = new System.Windows.Forms.DataGridView();

            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();

            this.panelTop.SuspendLayout();
            this.grpFilters.SuspendLayout();
            this.tabReports.SuspendLayout();
            this.tabStatistics.SuspendLayout();
            this.tabByCampaign.SuspendLayout();
            this.tabByClient.SuspendLayout();
            this.tabByCategory.SuspendLayout();
            this.panelStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaigns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1400, 60);
            this.panelTop.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📊 Report e Statistiche";

            // 
            // grpFilters
            // 
            this.grpFilters.BackColor = System.Drawing.Color.White;
            this.grpFilters.Controls.Add(this.lblDateFrom);
            this.grpFilters.Controls.Add(this.dtpDateFrom);
            this.grpFilters.Controls.Add(this.lblDateTo);
            this.grpFilters.Controls.Add(this.dtpDateTo);
            this.grpFilters.Controls.Add(this.lblClient);
            this.grpFilters.Controls.Add(this.cmbClient);
            this.grpFilters.Controls.Add(this.lblCampaign);
            this.grpFilters.Controls.Add(this.cmbCampaign);
            this.grpFilters.Controls.Add(this.btnGenerate);
            this.grpFilters.Controls.Add(this.lblLoading);
            this.grpFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFilters.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpFilters.Location = new System.Drawing.Point(0, 60);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Padding = new System.Windows.Forms.Padding(10);
            this.grpFilters.Size = new System.Drawing.Size(1400, 100);
            this.grpFilters.TabIndex = 1;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "🔍 Filtri";

            // lblDateFrom
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateFrom.Location = new System.Drawing.Point(20, 35);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(30, 15);
            this.lblDateFrom.TabIndex = 0;
            this.lblDateFrom.Text = "Dal:";

            // dtpDateFrom
            this.dtpDateFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(60, 32);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(120, 23);
            this.dtpDateFrom.TabIndex = 1;

            // lblDateTo
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTo.Location = new System.Drawing.Point(200, 35);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(21, 15);
            this.lblDateTo.TabIndex = 2;
            this.lblDateTo.Text = "Al:";

            // dtpDateTo
            this.dtpDateTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(230, 32);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(120, 23);
            this.dtpDateTo.TabIndex = 3;

            // lblClient
            this.lblClient.AutoSize = true;
            this.lblClient.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblClient.Location = new System.Drawing.Point(370, 35);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(47, 15);
            this.lblClient.TabIndex = 4;
            this.lblClient.Text = "Cliente:";

            // cmbClient
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClient.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbClient.FormattingEnabled = true;
            this.cmbClient.Location = new System.Drawing.Point(430, 32);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(250, 23);
            this.cmbClient.TabIndex = 5;
            this.cmbClient.SelectedIndexChanged += new System.EventHandler(this.cmbClient_SelectedIndexChanged);

            // lblCampaign
            this.lblCampaign.AutoSize = true;
            this.lblCampaign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCampaign.Location = new System.Drawing.Point(700, 35);
            this.lblCampaign.Name = "lblCampaign";
            this.lblCampaign.Size = new System.Drawing.Size(67, 15);
            this.lblCampaign.TabIndex = 6;
            this.lblCampaign.Text = "Campagna:";

            // cmbCampaign
            this.cmbCampaign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCampaign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCampaign.FormattingEnabled = true;
            this.cmbCampaign.Location = new System.Drawing.Point(780, 32);
            this.cmbCampaign.Name = "cmbCampaign";
            this.cmbCampaign.Size = new System.Drawing.Size(300, 23);
            this.cmbCampaign.TabIndex = 7;

            // btnGenerate
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(1100, 28);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(150, 32);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "📊 Genera Report";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);

            // lblLoading
            this.lblLoading.AutoSize = true;
            this.lblLoading.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblLoading.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblLoading.Location = new System.Drawing.Point(1270, 35);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(110, 19);
            this.lblLoading.TabIndex = 9;
            this.lblLoading.Text = "⏳ Generazione...";
            this.lblLoading.Visible = false;

            // 
            // tabReports
            // 
            this.tabReports.Controls.Add(this.tabStatistics);
            this.tabReports.Controls.Add(this.tabByCampaign);
            this.tabReports.Controls.Add(this.tabByClient);
            this.tabReports.Controls.Add(this.tabByCategory);
            this.tabReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReports.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabReports.Location = new System.Drawing.Point(0, 160);
            this.tabReports.Name = "tabReports";
            this.tabReports.SelectedIndex = 0;
            this.tabReports.Size = new System.Drawing.Size(1400, 570);
            this.tabReports.TabIndex = 2;

            // 
            // tabStatistics
            // 
            this.tabStatistics.BackColor = System.Drawing.Color.White;
            this.tabStatistics.Controls.Add(this.panelStatistics);
            this.tabStatistics.Location = new System.Drawing.Point(4, 24);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(10);
            this.tabStatistics.Size = new System.Drawing.Size(1392, 542);
            this.tabStatistics.TabIndex = 0;
            this.tabStatistics.Text = "📈 Statistiche";

            // panelStatistics
            this.panelStatistics.Controls.Add(this.lblTotalDays);
            this.panelStatistics.Controls.Add(this.lblTotalPasses);
            this.panelStatistics.Controls.Add(this.lblTotalDuration);
            this.panelStatistics.Controls.Add(this.lblUniqueCampaigns);
            this.panelStatistics.Controls.Add(this.lblUniqueClients);
            this.panelStatistics.Controls.Add(this.lblUniqueSpots);
            this.panelStatistics.Controls.Add(this.lblAvgPassesPerDay);
            this.panelStatistics.Controls.Add(this.lblAvgDurationPerDay);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatistics.Location = new System.Drawing.Point(10, 10);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Size = new System.Drawing.Size(1372, 522);
            this.panelStatistics.TabIndex = 0;

            // Statistics Labels - Layout a 2 colonne
            int labelX1 = 50;
            int labelX2 = 700;
            int labelY = 50;
            int labelSpacing = 60;

            // Colonna 1
            this.lblTotalDays.AutoSize = true;
            this.lblTotalDays.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalDays.Location = new System.Drawing.Point(labelX1, labelY);
            this.lblTotalDays.Name = "lblTotalDays";
            this.lblTotalDays.Size = new System.Drawing.Size(150, 21);
            this.lblTotalDays.TabIndex = 0;
            this.lblTotalDays.Text = "Giorni analizzati:  0";

            this.lblTotalPasses.AutoSize = true;
            this.lblTotalPasses.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPasses.Location = new System.Drawing.Point(labelX1, labelY + labelSpacing);
            this.lblTotalPasses.Name = "lblTotalPasses";
            this.lblTotalPasses.Size = new System.Drawing.Size(150, 21);
            this.lblTotalPasses.TabIndex = 1;
            this.lblTotalPasses.Text = "Passaggi totali: 0";

            this.lblTotalDuration.AutoSize = true;
            this.lblTotalDuration.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalDuration.Location = new System.Drawing.Point(labelX1, labelY + labelSpacing * 2);
            this.lblTotalDuration.Name = "lblTotalDuration";
            this.lblTotalDuration.Size = new System.Drawing.Size(200, 21);
            this.lblTotalDuration.TabIndex = 2;
            this.lblTotalDuration.Text = "Durata totale: 00:00:00";

            this.lblUniqueCampaigns.AutoSize = true;
            this.lblUniqueCampaigns.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUniqueCampaigns.Location = new System.Drawing.Point(labelX1, labelY + labelSpacing * 3);
            this.lblUniqueCampaigns.Name = "lblUniqueCampaigns";
            this.lblUniqueCampaigns.Size = new System.Drawing.Size(160, 21);
            this.lblUniqueCampaigns.TabIndex = 3;
            this.lblUniqueCampaigns.Text = "Campagne attive: 0";

            // Colonna 2
            this.lblUniqueClients.AutoSize = true;
            this.lblUniqueClients.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUniqueClients.Location = new System.Drawing.Point(labelX2, labelY);
            this.lblUniqueClients.Name = "lblUniqueClients";
            this.lblUniqueClients.Size = new System.Drawing.Size(130, 21);
            this.lblUniqueClients.TabIndex = 4;
            this.lblUniqueClients.Text = "Clienti attivi: 0";

            this.lblUniqueSpots.AutoSize = true;
            this.lblUniqueSpots.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUniqueSpots.Location = new System.Drawing.Point(labelX2, labelY + labelSpacing);
            this.lblUniqueSpots.Name = "lblUniqueSpots";
            this.lblUniqueSpots.Size = new System.Drawing.Size(130, 21);
            this.lblUniqueSpots.TabIndex = 5;
            this.lblUniqueSpots.Text = "Spot diversi: 0";

            this.lblAvgPassesPerDay.AutoSize = true;
            this.lblAvgPassesPerDay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAvgPassesPerDay.Location = new System.Drawing.Point(labelX2, labelY + labelSpacing * 2);
            this.lblAvgPassesPerDay.Name = "lblAvgPassesPerDay";
            this.lblAvgPassesPerDay.Size = new System.Drawing.Size(230, 21);
            this.lblAvgPassesPerDay.TabIndex = 6;
            this.lblAvgPassesPerDay.Text = "Media passaggi/giorno: 0.0";

            this.lblAvgDurationPerDay.AutoSize = true;
            this.lblAvgDurationPerDay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAvgDurationPerDay.Location = new System.Drawing.Point(labelX2, labelY + labelSpacing * 3);
            this.lblAvgDurationPerDay.Name = "lblAvgDurationPerDay";
            this.lblAvgDurationPerDay.Size = new System.Drawing.Size(280, 21);
            this.lblAvgDurationPerDay.TabIndex = 7;
            this.lblAvgDurationPerDay.Text = "Media durata/giorno: 00:00:00";

            // 
            // tabByCampaign
            // 
            this.tabByCampaign.BackColor = System.Drawing.Color.White;
            this.tabByCampaign.Controls.Add(this.dgvCampaigns);
            this.tabByCampaign.Location = new System.Drawing.Point(4, 24);
            this.tabByCampaign.Name = "tabByCampaign";
            this.tabByCampaign.Padding = new System.Windows.Forms.Padding(10);
            this.tabByCampaign.Size = new System.Drawing.Size(1392, 542);
            this.tabByCampaign.TabIndex = 1;
            this.tabByCampaign.Text = "📋 Per Campagna";

            // dgvCampaigns
            this.dgvCampaigns.BackgroundColor = System.Drawing.Color.White;
            this.dgvCampaigns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCampaigns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCampaigns.Location = new System.Drawing.Point(10, 10);
            this.dgvCampaigns.Name = "dgvCampaigns";
            this.dgvCampaigns.RowTemplate.Height = 25;
            this.dgvCampaigns.Size = new System.Drawing.Size(1372, 522);
            this.dgvCampaigns.TabIndex = 0;

            // 
            // tabByClient
            // 
            this.tabByClient.BackColor = System.Drawing.Color.White;
            this.tabByClient.Controls.Add(this.dgvClients);
            this.tabByClient.Location = new System.Drawing.Point(4, 24);
            this.tabByClient.Name = "tabByClient";
            this.tabByClient.Padding = new System.Windows.Forms.Padding(10);
            this.tabByClient.Size = new System.Drawing.Size(1392, 542);
            this.tabByClient.TabIndex = 2;
            this.tabByClient.Text = "👥 Per Cliente";

            // dgvClients
            this.dgvClients.BackgroundColor = System.Drawing.Color.White;
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClients.Location = new System.Drawing.Point(10, 10);
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.RowTemplate.Height = 25;
            this.dgvClients.Size = new System.Drawing.Size(1372, 522);
            this.dgvClients.TabIndex = 0;

            // 
            // tabByCategory
            // 
            this.tabByCategory.BackColor = System.Drawing.Color.White;
            this.tabByCategory.Controls.Add(this.dgvCategories);
            this.tabByCategory.Location = new System.Drawing.Point(4, 24);
            this.tabByCategory.Name = "tabByCategory";
            this.tabByCategory.Padding = new System.Windows.Forms.Padding(10);
            this.tabByCategory.Size = new System.Drawing.Size(1392, 542);
            this.tabByCategory.TabIndex = 3;
            this.tabByCategory.Text = "🏷️ Per Categoria";

            // dgvCategories
            this.dgvCategories.BackgroundColor = System.Drawing.Color.White;
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategories.Location = new System.Drawing.Point(10, 10);
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.RowTemplate.Height = 25;
            this.dgvCategories.Size = new System.Drawing.Size(1372, 522);
            this.dgvCategories.TabIndex = 0;

            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelBottom.Controls.Add(this.btnExportPdf);
            this.panelBottom.Controls.Add(this.btnExportCsv);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 730);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1400, 70);
            this.panelBottom.TabIndex = 3;

            // btnExportPdf
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Location = new System.Drawing.Point(1050, 15);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(150, 40);
            this.btnExportPdf.TabIndex = 0;
            this.btnExportPdf.Text = "📄 Esporta PDF";
            this.btnExportPdf.UseVisualStyleBackColor = false;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);

            // btnExportCsv
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(1220, 15);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(150, 40);
            this.btnExportCsv.TabIndex = 1;
            this.btnExportCsv.Text = "📊 Esporta CSV";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);

            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.tabReports);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📊 Report e Statistiche";

            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            this.tabReports.ResumeLayout(false);
            this.tabStatistics.ResumeLayout(false);
            this.tabByCampaign.ResumeLayout(false);
            this.tabByClient.ResumeLayout(false);
            this.tabByCategory.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            this.panelStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaigns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}