namespace AirADV.Forms
{
    partial class ScheduleViewForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top Panel
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;

        // Toolbar Panel (senza calendario)
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnPrevDay;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnNextDay;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnToggleView;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExportPdf;

        // Main Grid
        private System.Windows.Forms.DataGridView dgvSchedule;

        // Bottom Status
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblStatus;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleViewForm));
            panelTop = new Panel();
            lblTitle = new Label();
            panelToolbar = new Panel();
            btnPrevDay = new Button();
            lblDate = new Label();
            btnNextDay = new Button();
            btnToday = new Button();
            btnToggleView = new Button();
            btnSave = new Button();
            btnRefresh = new Button();
            btnExportPdf = new Button();
            dgvSchedule = new DataGridView();
            panelBottom = new Panel();
            lblStatus = new Label();
            panelTop.SuspendLayout();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).BeginInit();
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
            panelTop.Size = new Size(1600, 50);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 13);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(281, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📅 PALINSESTO GIORNALIERO";
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.FromArgb(240, 240, 245);
            panelToolbar.Controls.Add(btnPrevDay);
            panelToolbar.Controls.Add(lblDate);
            panelToolbar.Controls.Add(btnNextDay);
            panelToolbar.Controls.Add(btnToday);
            panelToolbar.Controls.Add(btnToggleView);
            panelToolbar.Controls.Add(btnSave);
            panelToolbar.Controls.Add(btnRefresh);
            panelToolbar.Controls.Add(btnExportPdf);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 50);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(1600, 70);
            panelToolbar.TabIndex = 1;
            // 
            // btnPrevDay
            // 
            btnPrevDay.BackColor = Color.White;
            btnPrevDay.FlatStyle = FlatStyle.Flat;
            btnPrevDay.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPrevDay.Location = new Point(20, 10);
            btnPrevDay.Name = "btnPrevDay";
            btnPrevDay.Size = new Size(60, 50);
            btnPrevDay.TabIndex = 0;
            btnPrevDay.Text = "◀";
            btnPrevDay.UseVisualStyleBackColor = false;
            btnPrevDay.Click += btnPrevDay_Click;
            // 
            // lblDate
            // 
            lblDate.BackColor = Color.White;
            lblDate.BorderStyle = BorderStyle.FixedSingle;
            lblDate.Cursor = Cursors.Hand;
            lblDate.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblDate.Location = new Point(90, 10);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(400, 50);
            lblDate.TabIndex = 1;
            lblDate.Text = "Lunedì 15 Gennaio 2025";
            lblDate.TextAlign = ContentAlignment.MiddleCenter;
            lblDate.Click += lblDate_Click;
            // 
            // btnNextDay
            // 
            btnNextDay.BackColor = Color.White;
            btnNextDay.FlatStyle = FlatStyle.Flat;
            btnNextDay.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnNextDay.Location = new Point(500, 10);
            btnNextDay.Name = "btnNextDay";
            btnNextDay.Size = new Size(60, 50);
            btnNextDay.TabIndex = 2;
            btnNextDay.Text = "▶";
            btnNextDay.UseVisualStyleBackColor = false;
            btnNextDay.Click += btnNextDay_Click;
            // 
            // btnToday
            // 
            btnToday.BackColor = Color.FromArgb(0, 123, 255);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnToday.ForeColor = Color.White;
            btnToday.Location = new Point(570, 10);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(120, 50);
            btnToday.TabIndex = 3;
            btnToday.Text = "📍 Oggi";
            btnToday.UseVisualStyleBackColor = false;
            btnToday.Click += btnToday_Click;
            // 
            // btnToggleView
            // 
            btnToggleView.BackColor = Color.FromArgb(108, 117, 125);
            btnToggleView.FlatStyle = FlatStyle.Flat;
            btnToggleView.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnToggleView.ForeColor = Color.White;
            btnToggleView.Location = new Point(720, 10);
            btnToggleView.Name = "btnToggleView";
            btnToggleView.Size = new Size(150, 50);
            btnToggleView.TabIndex = 4;
            btnToggleView.Text = "👤 Cliente";
            btnToggleView.UseVisualStyleBackColor = false;
            btnToggleView.Click += btnToggleView_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(40, 167, 69);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(880, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 50);
            btnSave.TabIndex = 5;
            btnSave.Text = "💾 Salva";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(0, 123, 255);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1020, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(150, 50);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "🔄 Aggiorna";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnExportPdf
            // 
            btnExportPdf.BackColor = Color.FromArgb(220, 53, 69);
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportPdf.ForeColor = Color.White;
            btnExportPdf.Location = new Point(1160, 10);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(130, 50);
            btnExportPdf.TabIndex = 7;
            btnExportPdf.Text = "📄 PDF";
            btnExportPdf.UseVisualStyleBackColor = false;
            btnExportPdf.Visible = false;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // dgvSchedule
            // 
            dgvSchedule.AllowUserToAddRows = false;
            dgvSchedule.AllowUserToDeleteRows = false;
            dgvSchedule.BackgroundColor = Color.White;
            dgvSchedule.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvSchedule.ColumnHeadersHeight = 35;
            dgvSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSchedule.DefaultCellStyle = dataGridViewCellStyle2;
            dgvSchedule.Dock = DockStyle.Fill;
            dgvSchedule.EnableHeadersVisualStyles = false;
            dgvSchedule.GridColor = Color.FromArgb(200, 200, 200);
            dgvSchedule.Location = new Point(0, 120);
            dgvSchedule.Name = "dgvSchedule";
            dgvSchedule.RowHeadersWidth = 80;
            dgvSchedule.RowTemplate.Height = 35;
            dgvSchedule.Size = new Size(1600, 650);
            dgvSchedule.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(240, 240, 245);
            panelBottom.Controls.Add(lblStatus);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 770);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1600, 30);
            panelBottom.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(20, 8);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(143, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Slot attivi: 0 | Spot totali: 0";
            // 
            // ScheduleViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 800);
            Controls.Add(dgvSchedule);
            Controls.Add(panelToolbar);
            Controls.Add(panelTop);
            Controls.Add(panelBottom);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1400, 700);
            Name = "ScheduleViewForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "📅 Palinsesto Giornaliero";
            WindowState = FormWindowState.Maximized;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}