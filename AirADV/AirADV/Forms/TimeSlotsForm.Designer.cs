namespace AirADV.Forms
{
    partial class TimeSlotsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top Panel
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;

        // Main Split Container
        private System.Windows.Forms.SplitContainer splitMain;

        // LEFT PANEL - Configurazione
        // STEP 1: Inserimento Orari
        private System.Windows.Forms.GroupBox grpAddSlots;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.CheckBox chkAutoIncrement;
        private System.Windows.Forms.Label lblIncrementHours;
        private System.Windows.Forms.NumericUpDown numIncrementHours;
        private System.Windows.Forms.Label lblHours;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Button btnAddSlots;

        // STEP 2: Configurazione Nuovi Slot
        private System.Windows.Forms.GroupBox grpConfigurations;
        private System.Windows.Forms.Label lblConfigMaxDuration;
        private System.Windows.Forms.NumericUpDown numConfigMaxDuration;
        private System.Windows.Forms.Label lblConfigPriority;
        private System.Windows.Forms.NumericUpDown numConfigPriority;
        private System.Windows.Forms.Label lblConfigOpening;
        private System.Windows.Forms.TextBox txtConfigOpening;
        private System.Windows.Forms.Button btnBrowseOpening;
        private System.Windows.Forms.Label lblConfigInfraSpot;
        private System.Windows.Forms.TextBox txtConfigInfraSpot;
        private System.Windows.Forms.Button btnBrowseInfraSpot;
        private System.Windows.Forms.Label lblConfigClosing;
        private System.Windows.Forms.TextBox txtConfigClosing;
        private System.Windows.Forms.Button btnBrowseClosing;

        // Bottoni Selezione
        private System.Windows.Forms.Panel panelSelection;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;

        // RIGHT PANEL - Lista Punti Orari
        private System.Windows.Forms.DataGridView dgvTimeSlots;
        private System.Windows.Forms.Panel panelGridButtons;
        private System.Windows.Forms.Button btnDeleteSlots;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;

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
            panelTop = new Panel();
            lblTitle = new Label();
            splitMain = new SplitContainer();
            grpAddSlots = new GroupBox();
            lblStartTime = new Label();
            dtpStartTime = new DateTimePicker();
            chkAutoIncrement = new CheckBox();
            lblIncrementHours = new Label();
            numIncrementHours = new NumericUpDown();
            lblHours = new Label();
            dtpEndTime = new DateTimePicker();
            btnAddSlots = new Button();
            grpConfigurations = new GroupBox();
            lblConfigMaxDuration = new Label();
            numConfigMaxDuration = new NumericUpDown();
            lblConfigPriority = new Label();
            numConfigPriority = new NumericUpDown();
            lblConfigOpening = new Label();
            txtConfigOpening = new TextBox();
            btnBrowseOpening = new Button();
            lblConfigInfraSpot = new Label();
            txtConfigInfraSpot = new TextBox();
            btnBrowseInfraSpot = new Button();
            lblConfigClosing = new Label();
            txtConfigClosing = new TextBox();
            btnBrowseClosing = new Button();
            panelSelection = new Panel();
            btnSelectAll = new Button();
            btnDeselectAll = new Button();
            dgvTimeSlots = new DataGridView();
            panelGridButtons = new Panel();
            btnDeleteSlots = new Button();
            btnSave = new Button();
            btnRefresh = new Button();
            panelBottom = new Panel();
            lblStatus = new Label();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            grpAddSlots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIncrementHours).BeginInit();
            grpConfigurations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numConfigMaxDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numConfigPriority).BeginInit();
            panelSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimeSlots).BeginInit();
            panelGridButtons.SuspendLayout();
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
            panelTop.Size = new Size(1454, 60);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(220, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🕐 Gestione Punti Orari";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 60);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.AutoScroll = true;
            splitMain.Panel1.BackColor = Color.FromArgb(250, 250, 250);
            splitMain.Panel1.Controls.Add(grpAddSlots);
            splitMain.Panel1.Controls.Add(grpConfigurations);
            splitMain.Panel1.Controls.Add(panelSelection);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.BackColor = Color.White;
            splitMain.Panel2.Controls.Add(dgvTimeSlots);
            splitMain.Panel2.Controls.Add(panelGridButtons);
            splitMain.Size = new Size(1454, 690);
            splitMain.SplitterDistance = 439;
            splitMain.TabIndex = 1;
            // 
            // grpAddSlots
            // 
            grpAddSlots.Controls.Add(lblStartTime);
            grpAddSlots.Controls.Add(dtpStartTime);
            grpAddSlots.Controls.Add(chkAutoIncrement);
            grpAddSlots.Controls.Add(lblIncrementHours);
            grpAddSlots.Controls.Add(numIncrementHours);
            grpAddSlots.Controls.Add(lblHours);
            grpAddSlots.Controls.Add(dtpEndTime);
            grpAddSlots.Controls.Add(btnAddSlots);
            grpAddSlots.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpAddSlots.Location = new Point(7, 10);
            grpAddSlots.Name = "grpAddSlots";
            grpAddSlots.Size = new Size(425, 200);
            grpAddSlots.TabIndex = 0;
            grpAddSlots.TabStop = false;
            grpAddSlots.Text = "📝 STEP 1: Inserimento Orari";
            // 
            // lblStartTime
            // 
            lblStartTime.AutoSize = true;
            lblStartTime.Font = new Font("Segoe UI", 9F);
            lblStartTime.Location = new Point(15, 35);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.Size = new Size(82, 15);
            lblStartTime.TabIndex = 0;
            lblStartTime.Text = "Orario iniziale:";
            // 
            // dtpStartTime
            // 
            dtpStartTime.CustomFormat = "HH:mm";
            dtpStartTime.Font = new Font("Segoe UI", 10F);
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.Location = new Point(120, 32);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.ShowUpDown = true;
            dtpStartTime.Size = new Size(100, 25);
            dtpStartTime.TabIndex = 1;
            // 
            // chkAutoIncrement
            // 
            chkAutoIncrement.AutoSize = true;
            chkAutoIncrement.Font = new Font("Segoe UI", 9F);
            chkAutoIncrement.Location = new Point(15, 75);
            chkAutoIncrement.Name = "chkAutoIncrement";
            chkAutoIncrement.Size = new Size(166, 19);
            chkAutoIncrement.TabIndex = 2;
            chkAutoIncrement.Text = "☑ Incremento automatico";
            chkAutoIncrement.UseVisualStyleBackColor = true;
            chkAutoIncrement.CheckedChanged += chkAutoIncrement_CheckedChanged;
            // 
            // lblIncrementHours
            // 
            lblIncrementHours.AutoSize = true;
            lblIncrementHours.Enabled = false;
            lblIncrementHours.Font = new Font("Segoe UI", 9F);
            lblIncrementHours.Location = new Point(15, 110);
            lblIncrementHours.Name = "lblIncrementHours";
            lblIncrementHours.Size = new Size(80, 15);
            lblIncrementHours.TabIndex = 3;
            lblIncrementHours.Text = "Incrementa di";
            // 
            // numIncrementHours
            // 
            numIncrementHours.Enabled = false;
            numIncrementHours.Font = new Font("Segoe UI", 9F);
            numIncrementHours.Location = new Point(121, 107);
            numIncrementHours.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            numIncrementHours.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIncrementHours.Name = "numIncrementHours";
            numIncrementHours.Size = new Size(60, 23);
            numIncrementHours.TabIndex = 4;
            numIncrementHours.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblHours
            // 
            lblHours.AutoSize = true;
            lblHours.Enabled = false;
            lblHours.Font = new Font("Segoe UI", 9F);
            lblHours.Location = new Point(187, 110);
            lblHours.Name = "lblHours";
            lblHours.Size = new Size(86, 15);
            lblHours.TabIndex = 5;
            lblHours.Text = "ora/e fino alle: ";
            // 
            // dtpEndTime
            // 
            dtpEndTime.CustomFormat = "HH: mm";
            dtpEndTime.Enabled = false;
            dtpEndTime.Font = new Font("Segoe UI", 9F);
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.Location = new Point(300, 107);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.ShowUpDown = true;
            dtpEndTime.Size = new Size(100, 23);
            dtpEndTime.TabIndex = 6;
            // 
            // btnAddSlots
            // 
            btnAddSlots.BackColor = Color.FromArgb(40, 167, 69);
            btnAddSlots.FlatStyle = FlatStyle.Flat;
            btnAddSlots.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddSlots.ForeColor = Color.White;
            btnAddSlots.Location = new Point(15, 150);
            btnAddSlots.Name = "btnAddSlots";
            btnAddSlots.Size = new Size(390, 35);
            btnAddSlots.TabIndex = 7;
            btnAddSlots.Text = "➕ Aggiungi Orario/i";
            btnAddSlots.UseVisualStyleBackColor = false;
            btnAddSlots.Click += btnAddSlots_Click;
            // 
            // grpConfigurations
            // 
            grpConfigurations.Controls.Add(lblConfigMaxDuration);
            grpConfigurations.Controls.Add(numConfigMaxDuration);
            grpConfigurations.Controls.Add(lblConfigPriority);
            grpConfigurations.Controls.Add(numConfigPriority);
            grpConfigurations.Controls.Add(lblConfigOpening);
            grpConfigurations.Controls.Add(txtConfigOpening);
            grpConfigurations.Controls.Add(btnBrowseOpening);
            grpConfigurations.Controls.Add(lblConfigInfraSpot);
            grpConfigurations.Controls.Add(txtConfigInfraSpot);
            grpConfigurations.Controls.Add(btnBrowseInfraSpot);
            grpConfigurations.Controls.Add(lblConfigClosing);
            grpConfigurations.Controls.Add(txtConfigClosing);
            grpConfigurations.Controls.Add(btnBrowseClosing);
            grpConfigurations.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpConfigurations.Location = new Point(7, 220);
            grpConfigurations.Name = "grpConfigurations";
            grpConfigurations.Size = new Size(425, 300);
            grpConfigurations.TabIndex = 1;
            grpConfigurations.TabStop = false;
            grpConfigurations.Text = "🔧 STEP 2: Configurazione Nuovi Slot";
            // 
            // lblConfigMaxDuration
            // 
            lblConfigMaxDuration.AutoSize = true;
            lblConfigMaxDuration.Font = new Font("Segoe UI", 9F);
            lblConfigMaxDuration.Location = new Point(15, 40);
            lblConfigMaxDuration.Name = "lblConfigMaxDuration";
            lblConfigMaxDuration.Size = new Size(87, 15);
            lblConfigMaxDuration.TabIndex = 0;
            lblConfigMaxDuration.Text = "Durata Max (s):";
            // 
            // numConfigMaxDuration
            // 
            numConfigMaxDuration.Font = new Font("Segoe UI", 9F);
            numConfigMaxDuration.Location = new Point(130, 38);
            numConfigMaxDuration.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            numConfigMaxDuration.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            numConfigMaxDuration.Name = "numConfigMaxDuration";
            numConfigMaxDuration.Size = new Size(100, 23);
            numConfigMaxDuration.TabIndex = 1;
            numConfigMaxDuration.Value = new decimal(new int[] { 320, 0, 0, 0 });
            // 
            // lblConfigPriority
            // 
            lblConfigPriority.AutoSize = true;
            lblConfigPriority.Font = new Font("Segoe UI", 9F);
            lblConfigPriority.Location = new Point(267, 40);
            lblConfigPriority.Name = "lblConfigPriority";
            lblConfigPriority.Size = new Size(48, 15);
            lblConfigPriority.TabIndex = 2;
            lblConfigPriority.Text = "Priorità:";
            // 
            // numConfigPriority
            // 
            numConfigPriority.Font = new Font("Segoe UI", 9F);
            numConfigPriority.Location = new Point(345, 38);
            numConfigPriority.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numConfigPriority.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numConfigPriority.Name = "numConfigPriority";
            numConfigPriority.Size = new Size(70, 23);
            numConfigPriority.TabIndex = 3;
            numConfigPriority.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // lblConfigOpening
            // 
            lblConfigOpening.AutoSize = true;
            lblConfigOpening.Font = new Font("Segoe UI", 9F);
            lblConfigOpening.Location = new Point(15, 85);
            lblConfigOpening.Name = "lblConfigOpening";
            lblConfigOpening.Size = new Size(77, 15);
            lblConfigOpening.TabIndex = 4;
            lblConfigOpening.Text = "File Opening:";
            // 
            // txtConfigOpening
            // 
            txtConfigOpening.Font = new Font("Segoe UI", 9F);
            txtConfigOpening.Location = new Point(15, 105);
            txtConfigOpening.Name = "txtConfigOpening";
            txtConfigOpening.ReadOnly = true;
            txtConfigOpening.Size = new Size(355, 23);
            txtConfigOpening.TabIndex = 5;
            // 
            // btnBrowseOpening
            // 
            btnBrowseOpening.Font = new Font("Segoe UI", 9F);
            btnBrowseOpening.Location = new Point(375, 105);
            btnBrowseOpening.Name = "btnBrowseOpening";
            btnBrowseOpening.Size = new Size(40, 23);
            btnBrowseOpening.TabIndex = 6;
            btnBrowseOpening.Text = "📁";
            btnBrowseOpening.UseVisualStyleBackColor = true;
            btnBrowseOpening.Click += btnBrowseOpening_Click;
            // 
            // lblConfigInfraSpot
            // 
            lblConfigInfraSpot.AutoSize = true;
            lblConfigInfraSpot.Font = new Font("Segoe UI", 9F);
            lblConfigInfraSpot.Location = new Point(15, 145);
            lblConfigInfraSpot.Name = "lblConfigInfraSpot";
            lblConfigInfraSpot.Size = new Size(79, 15);
            lblConfigInfraSpot.TabIndex = 7;
            lblConfigInfraSpot.Text = "File InfraSpot:";
            // 
            // txtConfigInfraSpot
            // 
            txtConfigInfraSpot.Font = new Font("Segoe UI", 9F);
            txtConfigInfraSpot.Location = new Point(15, 165);
            txtConfigInfraSpot.Name = "txtConfigInfraSpot";
            txtConfigInfraSpot.ReadOnly = true;
            txtConfigInfraSpot.Size = new Size(355, 23);
            txtConfigInfraSpot.TabIndex = 8;
            // 
            // btnBrowseInfraSpot
            // 
            btnBrowseInfraSpot.Font = new Font("Segoe UI", 9F);
            btnBrowseInfraSpot.Location = new Point(375, 165);
            btnBrowseInfraSpot.Name = "btnBrowseInfraSpot";
            btnBrowseInfraSpot.Size = new Size(40, 23);
            btnBrowseInfraSpot.TabIndex = 9;
            btnBrowseInfraSpot.Text = "📁";
            btnBrowseInfraSpot.UseVisualStyleBackColor = true;
            btnBrowseInfraSpot.Click += btnBrowseInfraSpot_Click;
            // 
            // lblConfigClosing
            // 
            lblConfigClosing.AutoSize = true;
            lblConfigClosing.Font = new Font("Segoe UI", 9F);
            lblConfigClosing.Location = new Point(15, 205);
            lblConfigClosing.Name = "lblConfigClosing";
            lblConfigClosing.Size = new Size(71, 15);
            lblConfigClosing.TabIndex = 10;
            lblConfigClosing.Text = "File Closing:";
            // 
            // txtConfigClosing
            // 
            txtConfigClosing.Font = new Font("Segoe UI", 9F);
            txtConfigClosing.Location = new Point(15, 225);
            txtConfigClosing.Name = "txtConfigClosing";
            txtConfigClosing.ReadOnly = true;
            txtConfigClosing.Size = new Size(355, 23);
            txtConfigClosing.TabIndex = 11;
            // 
            // btnBrowseClosing
            // 
            btnBrowseClosing.Font = new Font("Segoe UI", 9F);
            btnBrowseClosing.Location = new Point(375, 225);
            btnBrowseClosing.Name = "btnBrowseClosing";
            btnBrowseClosing.Size = new Size(40, 23);
            btnBrowseClosing.TabIndex = 12;
            btnBrowseClosing.Text = "📁";
            btnBrowseClosing.UseVisualStyleBackColor = true;
            btnBrowseClosing.Click += btnBrowseClosing_Click;
            // 
            // panelSelection
            // 
            panelSelection.Controls.Add(btnSelectAll);
            panelSelection.Controls.Add(btnDeselectAll);
            panelSelection.Location = new Point(7, 530);
            panelSelection.Name = "panelSelection";
            panelSelection.Size = new Size(425, 50);
            panelSelection.TabIndex = 2;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Font = new Font("Segoe UI", 9F);
            btnSelectAll.Location = new Point(15, 10);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(185, 32);
            btnSelectAll.TabIndex = 0;
            btnSelectAll.Text = "☑ Seleziona Tutti";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // btnDeselectAll
            // 
            btnDeselectAll.Font = new Font("Segoe UI", 9F);
            btnDeselectAll.Location = new Point(220, 10);
            btnDeselectAll.Name = "btnDeselectAll";
            btnDeselectAll.Size = new Size(185, 32);
            btnDeselectAll.TabIndex = 1;
            btnDeselectAll.Text = "☐ Deseleziona Tutti";
            btnDeselectAll.UseVisualStyleBackColor = true;
            btnDeselectAll.Click += btnDeselectAll_Click;
            // 
            // dgvTimeSlots
            // 
            dgvTimeSlots.BackgroundColor = Color.White;
            dgvTimeSlots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTimeSlots.Dock = DockStyle.Fill;
            dgvTimeSlots.Location = new Point(0, 0);
            dgvTimeSlots.Name = "dgvTimeSlots";
            dgvTimeSlots.RowTemplate.Height = 28;
            dgvTimeSlots.Size = new Size(1011, 630);
            dgvTimeSlots.TabIndex = 0;
            // 
            // panelGridButtons
            // 
            panelGridButtons.BackColor = Color.FromArgb(240, 240, 240);
            panelGridButtons.Controls.Add(btnDeleteSlots);
            panelGridButtons.Controls.Add(btnSave);
            panelGridButtons.Controls.Add(btnRefresh);
            panelGridButtons.Dock = DockStyle.Bottom;
            panelGridButtons.Location = new Point(0, 630);
            panelGridButtons.Name = "panelGridButtons";
            panelGridButtons.Size = new Size(1011, 60);
            panelGridButtons.TabIndex = 1;
            // 
            // btnDeleteSlots
            // 
            btnDeleteSlots.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteSlots.FlatStyle = FlatStyle.Flat;
            btnDeleteSlots.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeleteSlots.ForeColor = Color.White;
            btnDeleteSlots.Location = new Point(15, 15);
            btnDeleteSlots.Name = "btnDeleteSlots";
            btnDeleteSlots.Size = new Size(170, 32);
            btnDeleteSlots.TabIndex = 0;
            btnDeleteSlots.Text = "🗑️ Elimina Selezionati";
            btnDeleteSlots.UseVisualStyleBackColor = false;
            btnDeleteSlots.Click += btnDeleteSlots_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(40, 167, 69);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(700, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 32);
            btnSave.TabIndex = 1;
            btnSave.Text = "💾 Salva";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(820, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 32);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Aggiorna";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(240, 240, 240);
            panelBottom.Controls.Add(lblStatus);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 750);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1454, 30);
            panelBottom.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(20, 8);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(126, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Punti orari:  0 | Attivi: 0";
            // 
            // TimeSlotsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1454, 780);
            Controls.Add(splitMain);
            Controls.Add(panelTop);
            Controls.Add(panelBottom);
            MinimumSize = new Size(1200, 700);
            Name = "TimeSlotsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "🕐 Gestione Punti Orari";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            grpAddSlots.ResumeLayout(false);
            grpAddSlots.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIncrementHours).EndInit();
            grpConfigurations.ResumeLayout(false);
            grpConfigurations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numConfigMaxDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)numConfigPriority).EndInit();
            panelSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTimeSlots).EndInit();
            panelGridButtons.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}