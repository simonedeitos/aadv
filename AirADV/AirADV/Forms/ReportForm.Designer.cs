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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            panelTop = new Panel();
            lblTitle = new Label();
            grpFilters = new GroupBox();
            lblDateFrom = new Label();
            dtpDateFrom = new DateTimePicker();
            lblDateTo = new Label();
            dtpDateTo = new DateTimePicker();
            lblClient = new Label();
            cmbClient = new ComboBox();
            lblCampaign = new Label();
            cmbCampaign = new ComboBox();
            btnGenerate = new Button();
            lblLoading = new Label();
            tabReports = new TabControl();
            tabStatistics = new TabPage();
            panelStatistics = new Panel();
            lblTotalDays = new Label();
            lblTotalPasses = new Label();
            lblTotalDuration = new Label();
            lblUniqueCampaigns = new Label();
            lblUniqueClients = new Label();
            lblUniqueSpots = new Label();
            lblAvgPassesPerDay = new Label();
            lblAvgDurationPerDay = new Label();
            tabByCampaign = new TabPage();
            dgvCampaigns = new DataGridView();
            tabByClient = new TabPage();
            dgvClients = new DataGridView();
            tabByCategory = new TabPage();
            dgvCategories = new DataGridView();
            panelBottom = new Panel();
            btnExportPdf = new Button();
            btnExportCsv = new Button();
            panelTop.SuspendLayout();
            grpFilters.SuspendLayout();
            tabReports.SuspendLayout();
            tabStatistics.SuspendLayout();
            panelStatistics.SuspendLayout();
            tabByCampaign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).BeginInit();
            tabByClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
            tabByCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(45, 45, 48);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1400, 60);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(211, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📊 Report e Statistiche";
            // 
            // grpFilters
            // 
            grpFilters.BackColor = Color.White;
            grpFilters.Controls.Add(lblDateFrom);
            grpFilters.Controls.Add(dtpDateFrom);
            grpFilters.Controls.Add(lblDateTo);
            grpFilters.Controls.Add(dtpDateTo);
            grpFilters.Controls.Add(lblClient);
            grpFilters.Controls.Add(cmbClient);
            grpFilters.Controls.Add(lblCampaign);
            grpFilters.Controls.Add(cmbCampaign);
            grpFilters.Controls.Add(btnGenerate);
            grpFilters.Controls.Add(lblLoading);
            grpFilters.Dock = DockStyle.Top;
            grpFilters.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpFilters.Location = new Point(0, 60);
            grpFilters.Name = "grpFilters";
            grpFilters.Padding = new Padding(10);
            grpFilters.Size = new Size(1400, 100);
            grpFilters.TabIndex = 1;
            grpFilters.TabStop = false;
            grpFilters.Text = "🔍 Filtri";
            // 
            // lblDateFrom
            // 
            lblDateFrom.AutoSize = true;
            lblDateFrom.Font = new Font("Segoe UI", 9F);
            lblDateFrom.Location = new Point(20, 35);
            lblDateFrom.Name = "lblDateFrom";
            lblDateFrom.Size = new Size(27, 15);
            lblDateFrom.TabIndex = 0;
            lblDateFrom.Text = "Dal:";
            // 
            // dtpDateFrom
            // 
            dtpDateFrom.Font = new Font("Segoe UI", 9F);
            dtpDateFrom.Format = DateTimePickerFormat.Short;
            dtpDateFrom.Location = new Point(60, 32);
            dtpDateFrom.Name = "dtpDateFrom";
            dtpDateFrom.Size = new Size(120, 23);
            dtpDateFrom.TabIndex = 1;
            // 
            // lblDateTo
            // 
            lblDateTo.AutoSize = true;
            lblDateTo.Font = new Font("Segoe UI", 9F);
            lblDateTo.Location = new Point(200, 35);
            lblDateTo.Name = "lblDateTo";
            lblDateTo.Size = new Size(21, 15);
            lblDateTo.TabIndex = 2;
            lblDateTo.Text = "Al:";
            // 
            // dtpDateTo
            // 
            dtpDateTo.Font = new Font("Segoe UI", 9F);
            dtpDateTo.Format = DateTimePickerFormat.Short;
            dtpDateTo.Location = new Point(230, 32);
            dtpDateTo.Name = "dtpDateTo";
            dtpDateTo.Size = new Size(120, 23);
            dtpDateTo.TabIndex = 3;
            // 
            // lblClient
            // 
            lblClient.AutoSize = true;
            lblClient.Font = new Font("Segoe UI", 9F);
            lblClient.Location = new Point(370, 35);
            lblClient.Name = "lblClient";
            lblClient.Size = new Size(47, 15);
            lblClient.TabIndex = 4;
            lblClient.Text = "Cliente:";
            // 
            // cmbClient
            // 
            cmbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClient.Font = new Font("Segoe UI", 9F);
            cmbClient.FormattingEnabled = true;
            cmbClient.Location = new Point(430, 32);
            cmbClient.Name = "cmbClient";
            cmbClient.Size = new Size(250, 23);
            cmbClient.TabIndex = 5;
            cmbClient.SelectedIndexChanged += cmbClient_SelectedIndexChanged;
            // 
            // lblCampaign
            // 
            lblCampaign.AutoSize = true;
            lblCampaign.Font = new Font("Segoe UI", 9F);
            lblCampaign.Location = new Point(700, 35);
            lblCampaign.Name = "lblCampaign";
            lblCampaign.Size = new Size(68, 15);
            lblCampaign.TabIndex = 6;
            lblCampaign.Text = "Campagna:";
            // 
            // cmbCampaign
            // 
            cmbCampaign.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCampaign.Font = new Font("Segoe UI", 9F);
            cmbCampaign.FormattingEnabled = true;
            cmbCampaign.Location = new Point(780, 32);
            cmbCampaign.Name = "cmbCampaign";
            cmbCampaign.Size = new Size(300, 23);
            cmbCampaign.TabIndex = 7;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.FromArgb(0, 123, 255);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(1100, 28);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(150, 32);
            btnGenerate.TabIndex = 8;
            btnGenerate.Text = "📊 Genera Report";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblLoading.ForeColor = Color.OrangeRed;
            lblLoading.Location = new Point(1270, 35);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(119, 19);
            lblLoading.TabIndex = 9;
            lblLoading.Text = "⏳ Generazione...";
            lblLoading.Visible = false;
            // 
            // tabReports
            // 
            tabReports.Controls.Add(tabStatistics);
            tabReports.Controls.Add(tabByCampaign);
            tabReports.Controls.Add(tabByClient);
            tabReports.Controls.Add(tabByCategory);
            tabReports.Dock = DockStyle.Fill;
            tabReports.Font = new Font("Segoe UI", 9F);
            tabReports.Location = new Point(0, 160);
            tabReports.Name = "tabReports";
            tabReports.SelectedIndex = 0;
            tabReports.Size = new Size(1400, 570);
            tabReports.TabIndex = 2;
            // 
            // tabStatistics
            // 
            tabStatistics.BackColor = Color.White;
            tabStatistics.Controls.Add(panelStatistics);
            tabStatistics.Location = new Point(4, 24);
            tabStatistics.Name = "tabStatistics";
            tabStatistics.Padding = new Padding(10);
            tabStatistics.Size = new Size(1392, 542);
            tabStatistics.TabIndex = 0;
            tabStatistics.Text = "📈 Statistiche";
            // 
            // panelStatistics
            // 
            panelStatistics.Controls.Add(lblTotalDays);
            panelStatistics.Controls.Add(lblTotalPasses);
            panelStatistics.Controls.Add(lblTotalDuration);
            panelStatistics.Controls.Add(lblUniqueCampaigns);
            panelStatistics.Controls.Add(lblUniqueClients);
            panelStatistics.Controls.Add(lblUniqueSpots);
            panelStatistics.Controls.Add(lblAvgPassesPerDay);
            panelStatistics.Controls.Add(lblAvgDurationPerDay);
            panelStatistics.Dock = DockStyle.Fill;
            panelStatistics.Location = new Point(10, 10);
            panelStatistics.Name = "panelStatistics";
            panelStatistics.Size = new Size(1372, 522);
            panelStatistics.TabIndex = 0;
            // 
            // lblTotalDays
            // 
            lblTotalDays.AutoSize = true;
            lblTotalDays.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalDays.Location = new Point(50, 50);
            lblTotalDays.Name = "lblTotalDays";
            lblTotalDays.Size = new Size(156, 21);
            lblTotalDays.TabIndex = 0;
            lblTotalDays.Text = "Giorni analizzati:  0";
            // 
            // lblTotalPasses
            // 
            lblTotalPasses.AutoSize = true;
            lblTotalPasses.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalPasses.Location = new Point(50, 50);
            lblTotalPasses.Name = "lblTotalPasses";
            lblTotalPasses.Size = new Size(139, 21);
            lblTotalPasses.TabIndex = 1;
            lblTotalPasses.Text = "Passaggi totali: 0";
            // 
            // lblTotalDuration
            // 
            lblTotalDuration.AutoSize = true;
            lblTotalDuration.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalDuration.Location = new Point(50, 50);
            lblTotalDuration.Name = "lblTotalDuration";
            lblTotalDuration.Size = new Size(181, 21);
            lblTotalDuration.TabIndex = 2;
            lblTotalDuration.Text = "Durata totale: 00:00:00";
            // 
            // lblUniqueCampaigns
            // 
            lblUniqueCampaigns.AutoSize = true;
            lblUniqueCampaigns.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUniqueCampaigns.Location = new Point(50, 50);
            lblUniqueCampaigns.Name = "lblUniqueCampaigns";
            lblUniqueCampaigns.Size = new Size(157, 21);
            lblUniqueCampaigns.TabIndex = 3;
            lblUniqueCampaigns.Text = "Campagne attive: 0";
            // 
            // lblUniqueClients
            // 
            lblUniqueClients.AutoSize = true;
            lblUniqueClients.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUniqueClients.Location = new Point(700, 50);
            lblUniqueClients.Name = "lblUniqueClients";
            lblUniqueClients.Size = new Size(121, 21);
            lblUniqueClients.TabIndex = 4;
            lblUniqueClients.Text = "Clienti attivi: 0";
            // 
            // lblUniqueSpots
            // 
            lblUniqueSpots.AutoSize = true;
            lblUniqueSpots.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUniqueSpots.Location = new Point(700, 50);
            lblUniqueSpots.Name = "lblUniqueSpots";
            lblUniqueSpots.Size = new Size(117, 21);
            lblUniqueSpots.TabIndex = 5;
            lblUniqueSpots.Text = "Spot diversi: 0";
            // 
            // lblAvgPassesPerDay
            // 
            lblAvgPassesPerDay.AutoSize = true;
            lblAvgPassesPerDay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAvgPassesPerDay.Location = new Point(700, 50);
            lblAvgPassesPerDay.Name = "lblAvgPassesPerDay";
            lblAvgPassesPerDay.Size = new Size(217, 21);
            lblAvgPassesPerDay.TabIndex = 6;
            lblAvgPassesPerDay.Text = "Media passaggi/giorno: 0.0";
            // 
            // lblAvgDurationPerDay
            // 
            lblAvgDurationPerDay.AutoSize = true;
            lblAvgDurationPerDay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAvgDurationPerDay.Location = new Point(700, 50);
            lblAvgDurationPerDay.Name = "lblAvgDurationPerDay";
            lblAvgDurationPerDay.Size = new Size(240, 21);
            lblAvgDurationPerDay.TabIndex = 7;
            lblAvgDurationPerDay.Text = "Media durata/giorno: 00:00:00";
            // 
            // tabByCampaign
            // 
            tabByCampaign.BackColor = Color.White;
            tabByCampaign.Controls.Add(dgvCampaigns);
            tabByCampaign.Location = new Point(4, 24);
            tabByCampaign.Name = "tabByCampaign";
            tabByCampaign.Padding = new Padding(10);
            tabByCampaign.Size = new Size(1392, 542);
            tabByCampaign.TabIndex = 1;
            tabByCampaign.Text = "📋 Per Campagna";
            // 
            // dgvCampaigns
            // 
            dgvCampaigns.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCampaigns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCampaigns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCampaigns.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCampaigns.Dock = DockStyle.Fill;
            dgvCampaigns.Location = new Point(10, 10);
            dgvCampaigns.Name = "dgvCampaigns";
            dgvCampaigns.Size = new Size(1372, 522);
            dgvCampaigns.TabIndex = 0;
            // 
            // tabByClient
            // 
            tabByClient.BackColor = Color.White;
            tabByClient.Controls.Add(dgvClients);
            tabByClient.Location = new Point(4, 24);
            tabByClient.Name = "tabByClient";
            tabByClient.Padding = new Padding(10);
            tabByClient.Size = new Size(1392, 542);
            tabByClient.TabIndex = 2;
            tabByClient.Text = "👥 Per Cliente";
            // 
            // dgvClients
            // 
            dgvClients.BackgroundColor = Color.White;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvClients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvClients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvClients.DefaultCellStyle = dataGridViewCellStyle4;
            dgvClients.Dock = DockStyle.Fill;
            dgvClients.Location = new Point(10, 10);
            dgvClients.Name = "dgvClients";
            dgvClients.Size = new Size(1372, 522);
            dgvClients.TabIndex = 0;
            // 
            // tabByCategory
            // 
            tabByCategory.BackColor = Color.White;
            tabByCategory.Controls.Add(dgvCategories);
            tabByCategory.Location = new Point(4, 24);
            tabByCategory.Name = "tabByCategory";
            tabByCategory.Padding = new Padding(10);
            tabByCategory.Size = new Size(1392, 542);
            tabByCategory.TabIndex = 3;
            tabByCategory.Text = "🏷️ Per Categoria";
            // 
            // dgvCategories
            // 
            dgvCategories.BackgroundColor = Color.White;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvCategories.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvCategories.DefaultCellStyle = dataGridViewCellStyle6;
            dgvCategories.Dock = DockStyle.Fill;
            dgvCategories.Location = new Point(10, 10);
            dgvCategories.Name = "dgvCategories";
            dgvCategories.Size = new Size(1372, 522);
            dgvCategories.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(240, 240, 240);
            panelBottom.Controls.Add(btnExportPdf);
            panelBottom.Controls.Add(btnExportCsv);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 730);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1400, 70);
            panelBottom.TabIndex = 3;
            // 
            // btnExportPdf
            // 
            btnExportPdf.BackColor = Color.FromArgb(220, 53, 69);
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportPdf.ForeColor = Color.White;
            btnExportPdf.Location = new Point(1050, 15);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(150, 40);
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "📄 Esporta PDF";
            btnExportPdf.UseVisualStyleBackColor = false;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // btnExportCsv
            // 
            btnExportCsv.BackColor = Color.FromArgb(40, 167, 69);
            btnExportCsv.FlatStyle = FlatStyle.Flat;
            btnExportCsv.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportCsv.ForeColor = Color.White;
            btnExportCsv.Location = new Point(1220, 15);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(150, 40);
            btnExportCsv.TabIndex = 1;
            btnExportCsv.Text = "📊 Esporta CSV";
            btnExportCsv.UseVisualStyleBackColor = false;
            btnExportCsv.Click += btnExportCsv_Click;
            // 
            // ReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1400, 800);
            Controls.Add(tabReports);
            Controls.Add(grpFilters);
            Controls.Add(panelTop);
            Controls.Add(panelBottom);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ReportForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "📊 Report e Statistiche";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            grpFilters.ResumeLayout(false);
            grpFilters.PerformLayout();
            tabReports.ResumeLayout(false);
            tabStatistics.ResumeLayout(false);
            panelStatistics.ResumeLayout(false);
            panelStatistics.PerformLayout();
            tabByCampaign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).EndInit();
            tabByClient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
            tabByCategory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}