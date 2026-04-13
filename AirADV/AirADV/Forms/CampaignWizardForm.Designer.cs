namespace AirADV.Forms
{
    partial class CampaignWizardForm
    {
        private System.ComponentModel.IContainer components = null;

        // STEP 1 - Dati Campagna
        private System.Windows.Forms.Panel pnlStep1;
        private System.Windows.Forms.Label lblStep1Title;
        private System.Windows.Forms.Label lblCampaignName;
        private System.Windows.Forms.TextBox txtCampaignName;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.Label lblSpots;
        private System.Windows.Forms.ComboBox cmbSpotSelector;
        private System.Windows.Forms.Button btnAddSpot;
        private System.Windows.Forms.ListBox lstSelectedSpots;
        private System.Windows.Forms.Button btnRemoveSpot;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Label lblSpotCount;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.GroupBox grpPeriod;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;

        // STEP 2 - Schedulazione
        private System.Windows.Forms.Panel pnlStep2;
        private System.Windows.Forms.Label lblStep2Title;
        private System.Windows.Forms.GroupBox grpDistribution;
        private System.Windows.Forms.RadioButton rdAutoBalanced;
        private System.Windows.Forms.RadioButton rdAutoAudience;
        private System.Windows.Forms.RadioButton rdManual;
        private System.Windows.Forms.GroupBox grpAutoConfig;
        private System.Windows.Forms.Label lblDailyPasses;
        private System.Windows.Forms.NumericUpDown numDailyPasses;
        private System.Windows.Forms.CheckBox chkTimeFilter;
        private System.Windows.Forms.Label lblTimeFrom;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label lblTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.GroupBox grpDays;
        private System.Windows.Forms.CheckBox chkMonday;
        private System.Windows.Forms.CheckBox chkTuesday;
        private System.Windows.Forms.CheckBox chkWednesday;
        private System.Windows.Forms.CheckBox chkThursday;
        private System.Windows.Forms.CheckBox chkFriday;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.GroupBox grpManualSlots;
        private System.Windows.Forms.FlowLayoutPanel flowManualSlots;
        private System.Windows.Forms.Button btnSelectAllSlots;
        private System.Windows.Forms.Button btnDeselectAllSlots;
        private System.Windows.Forms.Label lblManualDailyPasses;
        private System.Windows.Forms.NumericUpDown numManualDailyPasses;
        private System.Windows.Forms.Label lblManualSlotCount;

        // STEP 3 - Revisione (✅ AGGIORNATO)
        private System.Windows.Forms.Panel pnlStep3;
        private System.Windows.Forms.Label lblStep3Title;
        private System.Windows.Forms.ProgressBar progressDays;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnConfirmAll;

        // ✅ NUOVO: Container principale con scroll
        private System.Windows.Forms.Panel pnlMainContainer;

        // Bottoni navigazione
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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
            pnlMainContainer = new Panel();
            pnlStep1 = new Panel();
            lblStep1Title = new Label();
            lblCampaignName = new Label();
            txtCampaignName = new TextBox();
            lblClient = new Label();
            cmbClient = new ComboBox();
            lblSpots = new Label();
            cmbSpotSelector = new ComboBox();
            btnAddSpot = new Button();
            lstSelectedSpots = new ListBox();
            btnRemoveSpot = new Button();
            btnMoveUp = new Button();
            btnMoveDown = new Button();
            lblSpotCount = new Label();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            btnAddCategory = new Button();
            grpPeriod = new GroupBox();
            lblStartDate = new Label();
            dtpStartDate = new DateTimePicker();
            lblEndDate = new Label();
            dtpEndDate = new DateTimePicker();
            pnlStep2 = new Panel();
            lblStep2Title = new Label();
            grpDistribution = new GroupBox();
            rdAutoBalanced = new RadioButton();
            rdAutoAudience = new RadioButton();
            rdManual = new RadioButton();
            grpAutoConfig = new GroupBox();
            lblDailyPasses = new Label();
            numDailyPasses = new NumericUpDown();
            chkTimeFilter = new CheckBox();
            lblTimeFrom = new Label();
            dtpTimeFrom = new DateTimePicker();
            lblTimeTo = new Label();
            dtpTimeTo = new DateTimePicker();
            grpDays = new GroupBox();
            chkMonday = new CheckBox();
            chkTuesday = new CheckBox();
            chkWednesday = new CheckBox();
            chkThursday = new CheckBox();
            chkFriday = new CheckBox();
            chkSaturday = new CheckBox();
            chkSunday = new CheckBox();
            grpManualSlots = new GroupBox();
            flowManualSlots = new FlowLayoutPanel();
            btnSelectAllSlots = new Button();
            btnDeselectAllSlots = new Button();
            lblManualDailyPasses = new Label();
            numManualDailyPasses = new NumericUpDown();
            lblManualSlotCount = new Label();
            pnlStep3 = new Panel();
            lblStep3Title = new Label();
            progressDays = new ProgressBar();
            lblProgress = new Label();
            btnConfirmAll = new Button();
            panelButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            pnlMainContainer.SuspendLayout();
            pnlStep1.SuspendLayout();
            grpPeriod.SuspendLayout();
            pnlStep2.SuspendLayout();
            grpDistribution.SuspendLayout();
            grpAutoConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDailyPasses).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numManualDailyPasses).BeginInit();
            grpDays.SuspendLayout();
            grpManualSlots.SuspendLayout();
            pnlStep3.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMainContainer
            // 
            pnlMainContainer.AutoScroll = true;
            pnlMainContainer.BackColor = Color.FromArgb(245, 245, 245);
            pnlMainContainer.Controls.Add(pnlStep1);
            pnlMainContainer.Controls.Add(pnlStep2);
            pnlMainContainer.Controls.Add(pnlStep3);
            pnlMainContainer.Dock = DockStyle.Fill;
            pnlMainContainer.Location = new Point(0, 0);
            pnlMainContainer.Name = "pnlMainContainer";
            pnlMainContainer.Padding = new Padding(20);
            pnlMainContainer.Size = new Size(1131, 700);
            pnlMainContainer.TabIndex = 0;
            // 
            // pnlStep1
            // 
            pnlStep1.BackColor = Color.White;
            pnlStep1.BorderStyle = BorderStyle.FixedSingle;
            pnlStep1.Controls.Add(lblStep1Title);
            pnlStep1.Controls.Add(lblCampaignName);
            pnlStep1.Controls.Add(txtCampaignName);
            pnlStep1.Controls.Add(lblClient);
            pnlStep1.Controls.Add(cmbClient);
            pnlStep1.Controls.Add(lblSpots);
            pnlStep1.Controls.Add(cmbSpotSelector);
            pnlStep1.Controls.Add(btnAddSpot);
            pnlStep1.Controls.Add(lstSelectedSpots);
            pnlStep1.Controls.Add(btnRemoveSpot);
            pnlStep1.Controls.Add(btnMoveUp);
            pnlStep1.Controls.Add(btnMoveDown);
            pnlStep1.Controls.Add(lblSpotCount);
            pnlStep1.Controls.Add(lblCategory);
            pnlStep1.Controls.Add(cmbCategory);
            pnlStep1.Controls.Add(btnAddCategory);
            pnlStep1.Controls.Add(grpPeriod);
            pnlStep1.Location = new Point(20, 20);
            pnlStep1.Name = "pnlStep1";
            pnlStep1.Size = new Size(1071, 339);
            pnlStep1.TabIndex = 0;
            // 
            // lblStep1Title
            // 
            lblStep1Title.BackColor = Color.FromArgb(0, 123, 255);
            lblStep1Title.Dock = DockStyle.Top;
            lblStep1Title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStep1Title.ForeColor = Color.White;
            lblStep1Title.Location = new Point(0, 0);
            lblStep1Title.Name = "lblStep1Title";
            lblStep1Title.Padding = new Padding(15, 12, 0, 0);
            lblStep1Title.Size = new Size(1069, 45);
            lblStep1Title.TabIndex = 0;
            lblStep1Title.Text = "📋 STEP 1: Dati Campagna";
            // 
            // lblCampaignName
            // 
            lblCampaignName.AutoSize = true;
            lblCampaignName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCampaignName.Location = new Point(49, 65);
            lblCampaignName.Name = "lblCampaignName";
            lblCampaignName.Size = new Size(122, 17);
            lblCampaignName.TabIndex = 1;
            lblCampaignName.Text = "Nome Campagna: ";
            // 
            // txtCampaignName
            // 
            txtCampaignName.Font = new Font("Segoe UI", 10F);
            txtCampaignName.Location = new Point(204, 62);
            txtCampaignName.Name = "txtCampaignName";
            txtCampaignName.Size = new Size(400, 25);
            txtCampaignName.TabIndex = 2;
            // 
            // lblClient
            // 
            lblClient.AutoSize = true;
            lblClient.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblClient.Location = new Point(49, 105);
            lblClient.Name = "lblClient";
            lblClient.Size = new Size(55, 17);
            lblClient.TabIndex = 3;
            lblClient.Text = "Cliente:";
            // 
            // cmbClient
            // 
            cmbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClient.Font = new Font("Segoe UI", 10F);
            cmbClient.FormattingEnabled = true;
            cmbClient.Location = new Point(204, 102);
            cmbClient.Name = "cmbClient";
            cmbClient.Size = new Size(350, 25);
            cmbClient.TabIndex = 4;
            cmbClient.SelectedIndexChanged += cmbClient_SelectedIndexChanged;
            // 
            // lblSpots
            // 
            lblSpots.AutoSize = true;
            lblSpots.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSpots.Location = new Point(49, 145);
            lblSpots.Name = "lblSpots";
            lblSpots.Size = new Size(173, 17);
            lblSpots.TabIndex = 5;
            lblSpots.Text = "Spot (sequenza rotazione):";
            // 
            // cmbSpotSelector
            // 
            cmbSpotSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpotSelector.Font = new Font("Segoe UI", 10F);
            cmbSpotSelector.FormattingEnabled = true;
            cmbSpotSelector.Location = new Point(49, 167);
            cmbSpotSelector.Name = "cmbSpotSelector";
            cmbSpotSelector.Size = new Size(400, 25);
            cmbSpotSelector.TabIndex = 6;
            // 
            // btnAddSpot
            // 
            btnAddSpot.BackColor = Color.FromArgb(40, 167, 69);
            btnAddSpot.FlatStyle = FlatStyle.Flat;
            btnAddSpot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddSpot.ForeColor = Color.White;
            btnAddSpot.Location = new Point(459, 167);
            btnAddSpot.Name = "btnAddSpot";
            btnAddSpot.Size = new Size(95, 25);
            btnAddSpot.TabIndex = 7;
            btnAddSpot.Text = "➕ Aggiungi";
            btnAddSpot.UseVisualStyleBackColor = false;
            btnAddSpot.Click += btnAddSpot_Click;
            // 
            // lstSelectedSpots
            // 
            lstSelectedSpots.Font = new Font("Consolas", 9F);
            lstSelectedSpots.FormattingEnabled = true;
            lstSelectedSpots.Location = new Point(49, 202);
            lstSelectedSpots.Name = "lstSelectedSpots";
            lstSelectedSpots.Size = new Size(505, 102);
            lstSelectedSpots.TabIndex = 8;
            // 
            // btnRemoveSpot
            // 
            btnRemoveSpot.BackColor = Color.FromArgb(220, 53, 69);
            btnRemoveSpot.FlatStyle = FlatStyle.Flat;
            btnRemoveSpot.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnRemoveSpot.ForeColor = Color.White;
            btnRemoveSpot.Location = new Point(564, 202);
            btnRemoveSpot.Name = "btnRemoveSpot";
            btnRemoveSpot.Size = new Size(101, 28);
            btnRemoveSpot.TabIndex = 9;
            btnRemoveSpot.Text = "➖ Rimuovi";
            btnRemoveSpot.UseVisualStyleBackColor = false;
            btnRemoveSpot.Click += btnRemoveSpot_Click;
            // 
            // btnMoveUp
            // 
            btnMoveUp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMoveUp.Location = new Point(564, 237);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(80, 28);
            btnMoveUp.TabIndex = 10;
            btnMoveUp.Text = "⬆️";
            btnMoveUp.UseVisualStyleBackColor = true;
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMoveDown.Location = new Point(564, 269);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(80, 28);
            btnMoveDown.TabIndex = 11;
            btnMoveDown.Text = "⬇️";
            btnMoveDown.UseVisualStyleBackColor = true;
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // lblSpotCount
            // 
            lblSpotCount.AutoSize = true;
            lblSpotCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblSpotCount.ForeColor = Color.Gray;
            lblSpotCount.Location = new Point(49, 309);
            lblSpotCount.Name = "lblSpotCount";
            lblSpotCount.Size = new Size(103, 15);
            lblSpotCount.TabIndex = 12;
            lblSpotCount.Text = "Spot selezionati:  0";
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCategory.Location = new Point(709, 82);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(71, 17);
            lblCategory.TabIndex = 13;
            lblCategory.Text = "Categoria:";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Font = new Font("Segoe UI", 10F);
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(709, 102);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(265, 25);
            cmbCategory.TabIndex = 14;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // btnAddCategory
            // 
            btnAddCategory.BackColor = Color.FromArgb(40, 167, 69);
            btnAddCategory.FlatAppearance.BorderSize = 0;
            btnAddCategory.FlatStyle = FlatStyle.Flat;
            btnAddCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddCategory.ForeColor = Color.White;
            btnAddCategory.Location = new Point(977, 102);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(32, 25);
            btnAddCategory.TabIndex = 15;
            btnAddCategory.Text = "➕";
            btnAddCategory.UseVisualStyleBackColor = false;
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // grpPeriod
            // 
            grpPeriod.Controls.Add(lblStartDate);
            grpPeriod.Controls.Add(dtpStartDate);
            grpPeriod.Controls.Add(lblEndDate);
            grpPeriod.Controls.Add(dtpEndDate);
            grpPeriod.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpPeriod.Location = new Point(709, 167);
            grpPeriod.Name = "grpPeriod";
            grpPeriod.Size = new Size(287, 123);
            grpPeriod.TabIndex = 15;
            grpPeriod.TabStop = false;
            grpPeriod.Text = "📅 Periodo Campagna";
            grpPeriod.Enter += grpPeriod_Enter;
            // 
            // lblStartDate
            // 
            lblStartDate.AutoSize = true;
            lblStartDate.Font = new Font("Segoe UI", 9F);
            lblStartDate.Location = new Point(32, 46);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(65, 15);
            lblStartDate.TabIndex = 0;
            lblStartDate.Text = "Data Inizio:";
            // 
            // dtpStartDate
            // 
            dtpStartDate.Font = new Font("Segoe UI", 9F);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(129, 40);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(120, 23);
            dtpStartDate.TabIndex = 1;
            // 
            // lblEndDate
            // 
            lblEndDate.AutoSize = true;
            lblEndDate.Font = new Font("Segoe UI", 9F);
            lblEndDate.Location = new Point(32, 75);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(59, 15);
            lblEndDate.TabIndex = 2;
            lblEndDate.Text = "Data Fine:";
            // 
            // dtpEndDate
            // 
            dtpEndDate.Font = new Font("Segoe UI", 9F);
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(129, 69);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(120, 23);
            dtpEndDate.TabIndex = 3;
            // 
            // pnlStep2
            // 
            pnlStep2.BackColor = Color.White;
            pnlStep2.BorderStyle = BorderStyle.FixedSingle;
            pnlStep2.Controls.Add(lblStep2Title);
            pnlStep2.Controls.Add(grpDistribution);
            pnlStep2.Controls.Add(grpAutoConfig);
            pnlStep2.Controls.Add(grpManualSlots);
            pnlStep2.Controls.Add(grpDays);
            pnlStep2.Location = new Point(20, 378);
            pnlStep2.Name = "pnlStep2";
            pnlStep2.Size = new Size(1070, 310);
            pnlStep2.TabIndex = 1;
            // 
            // lblStep2Title
            // 
            lblStep2Title.BackColor = Color.FromArgb(255, 193, 7);
            lblStep2Title.Dock = DockStyle.Top;
            lblStep2Title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStep2Title.ForeColor = Color.Black;
            lblStep2Title.Location = new Point(0, 0);
            lblStep2Title.Name = "lblStep2Title";
            lblStep2Title.Padding = new Padding(15, 12, 0, 0);
            lblStep2Title.Size = new Size(1068, 45);
            lblStep2Title.TabIndex = 0;
            lblStep2Title.Text = "⚙️ STEP 2: Configurazione Schedulazione";
            // 
            // grpDistribution
            // 
            grpDistribution.Controls.Add(rdAutoBalanced);
            grpDistribution.Controls.Add(rdAutoAudience);
            grpDistribution.Controls.Add(rdManual);
            grpDistribution.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpDistribution.Location = new Point(49, 64);
            grpDistribution.Name = "grpDistribution";
            grpDistribution.Size = new Size(990, 60);
            grpDistribution.TabIndex = 1;
            grpDistribution.TabStop = false;
            grpDistribution.Text = "Modalità Distribuzione";
            // 
            // rdAutoBalanced
            // 
            rdAutoBalanced.AutoSize = true;
            rdAutoBalanced.Checked = true;
            rdAutoBalanced.Font = new Font("Segoe UI", 9F);
            rdAutoBalanced.Location = new Point(20, 28);
            rdAutoBalanced.Name = "rdAutoBalanced";
            rdAutoBalanced.Size = new Size(149, 19);
            rdAutoBalanced.TabIndex = 0;
            rdAutoBalanced.TabStop = true;
            rdAutoBalanced.Text = "Automatica - Bilanciata";
            rdAutoBalanced.CheckedChanged += rdAutoBalanced_CheckedChanged;
            // 
            // rdAutoAudience
            // 
            rdAutoAudience.AutoSize = true;
            rdAutoAudience.Font = new Font("Segoe UI", 9F);
            rdAutoAudience.Location = new Point(260, 28);
            rdAutoAudience.Name = "rdAutoAudience";
            rdAutoAudience.Size = new Size(221, 19);
            rdAutoAudience.TabIndex = 1;
            rdAutoAudience.Text = "Automatica - Affollamento Audience";
            rdAutoAudience.CheckedChanged += rdAutoAudience_CheckedChanged;
            // 
            // rdManual
            // 
            rdManual.AutoSize = true;
            rdManual.Font = new Font("Segoe UI", 9F);
            rdManual.Location = new Point(570, 28);
            rdManual.Name = "rdManual";
            rdManual.Size = new Size(191, 19);
            rdManual.TabIndex = 2;
            rdManual.Text = "Manuale - Selezione Punti Orari";
            rdManual.CheckedChanged += rdManual_CheckedChanged;
            // 
            // grpAutoConfig
            // 
            grpAutoConfig.Controls.Add(lblDailyPasses);
            grpAutoConfig.Controls.Add(numDailyPasses);
            grpAutoConfig.Controls.Add(chkTimeFilter);
            grpAutoConfig.Controls.Add(lblTimeFrom);
            grpAutoConfig.Controls.Add(dtpTimeFrom);
            grpAutoConfig.Controls.Add(lblTimeTo);
            grpAutoConfig.Controls.Add(dtpTimeTo);
            grpAutoConfig.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpAutoConfig.Location = new Point(49, 139);
            grpAutoConfig.Name = "grpAutoConfig";
            grpAutoConfig.Size = new Size(990, 110);
            grpAutoConfig.TabIndex = 2;
            grpAutoConfig.TabStop = false;
            grpAutoConfig.Text = "Configurazione Automatica";
            // 
            // lblDailyPasses
            // 
            lblDailyPasses.AutoSize = true;
            lblDailyPasses.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDailyPasses.Location = new Point(15, 28);
            lblDailyPasses.Name = "lblDailyPasses";
            lblDailyPasses.Size = new Size(107, 15);
            lblDailyPasses.TabIndex = 0;
            lblDailyPasses.Text = "Passaggi al giorno:";
            // 
            // numDailyPasses
            // 
            numDailyPasses.Font = new Font("Segoe UI", 10F);
            numDailyPasses.Location = new Point(173, 23);
            numDailyPasses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDailyPasses.Name = "numDailyPasses";
            numDailyPasses.Size = new Size(55, 25);
            numDailyPasses.TabIndex = 1;
            numDailyPasses.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // chkTimeFilter
            // 
            chkTimeFilter.AutoSize = true;
            chkTimeFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkTimeFilter.Location = new Point(15, 77);
            chkTimeFilter.Name = "chkTimeFilter";
            chkTimeFilter.Size = new Size(147, 19);
            chkTimeFilter.TabIndex = 2;
            chkTimeFilter.Text = "☑ Abilita Filtro Orario";
            chkTimeFilter.UseVisualStyleBackColor = true;
            chkTimeFilter.CheckedChanged += chkTimeFilter_CheckedChanged;
            // 
            // lblTimeFrom
            // 
            lblTimeFrom.AutoSize = true;
            lblTimeFrom.Enabled = false;
            lblTimeFrom.Font = new Font("Segoe UI", 9F);
            lblTimeFrom.Location = new Point(196, 78);
            lblTimeFrom.Name = "lblTimeFrom";
            lblTimeFrom.Size = new Size(24, 15);
            lblTimeFrom.TabIndex = 3;
            lblTimeFrom.Text = "Da:";
            // 
            // dtpTimeFrom
            // 
            dtpTimeFrom.CustomFormat = "HH:mm";
            dtpTimeFrom.Enabled = false;
            dtpTimeFrom.Font = new Font("Segoe UI", 9F);
            dtpTimeFrom.Format = DateTimePickerFormat.Custom;
            dtpTimeFrom.Location = new Point(226, 75);
            dtpTimeFrom.Name = "dtpTimeFrom";
            dtpTimeFrom.ShowUpDown = true;
            dtpTimeFrom.Size = new Size(80, 23);
            dtpTimeFrom.TabIndex = 4;
            // 
            // lblTimeTo
            // 
            lblTimeTo.AutoSize = true;
            lblTimeTo.Enabled = false;
            lblTimeTo.Font = new Font("Segoe UI", 9F);
            lblTimeTo.Location = new Point(336, 78);
            lblTimeTo.Name = "lblTimeTo";
            lblTimeTo.Size = new Size(18, 15);
            lblTimeTo.TabIndex = 5;
            lblTimeTo.Text = "A:";
            // 
            // dtpTimeTo
            // 
            dtpTimeTo.CustomFormat = "HH:mm";
            dtpTimeTo.Enabled = false;
            dtpTimeTo.Font = new Font("Segoe UI", 9F);
            dtpTimeTo.Format = DateTimePickerFormat.Custom;
            dtpTimeTo.Location = new Point(363, 75);
            dtpTimeTo.Name = "dtpTimeTo";
            dtpTimeTo.ShowUpDown = true;
            dtpTimeTo.Size = new Size(80, 23);
            dtpTimeTo.TabIndex = 6;
            // 
            // grpDays
            // 
            grpDays.Controls.Add(chkMonday);
            grpDays.Controls.Add(chkTuesday);
            grpDays.Controls.Add(chkWednesday);
            grpDays.Controls.Add(chkThursday);
            grpDays.Controls.Add(chkFriday);
            grpDays.Controls.Add(chkSaturday);
            grpDays.Controls.Add(chkSunday);
            grpDays.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpDays.Location = new Point(49, 254);
            grpDays.Name = "grpDays";
            grpDays.Size = new Size(990, 40);
            grpDays.TabIndex = 7;
            grpDays.TabStop = false;
            grpDays.Text = "Giorni Settimana";
            // 
            // chkMonday
            // 
            chkMonday.AutoSize = true;
            chkMonday.Checked = true;
            chkMonday.CheckState = CheckState.Checked;
            chkMonday.Font = new Font("Segoe UI", 8.5F);
            chkMonday.Location = new Point(15, 18);
            chkMonday.Name = "chkMonday";
            chkMonday.Size = new Size(46, 19);
            chkMonday.TabIndex = 0;
            chkMonday.Text = "Lun";
            // 
            // chkTuesday
            // 
            chkTuesday.AutoSize = true;
            chkTuesday.Checked = true;
            chkTuesday.CheckState = CheckState.Checked;
            chkTuesday.Font = new Font("Segoe UI", 8.5F);
            chkTuesday.Location = new Point(120, 18);
            chkTuesday.Name = "chkTuesday";
            chkTuesday.Size = new Size(47, 19);
            chkTuesday.TabIndex = 1;
            chkTuesday.Text = "Mar";
            // 
            // chkWednesday
            // 
            chkWednesday.AutoSize = true;
            chkWednesday.Checked = true;
            chkWednesday.CheckState = CheckState.Checked;
            chkWednesday.Font = new Font("Segoe UI", 8.5F);
            chkWednesday.Location = new Point(225, 18);
            chkWednesday.Name = "chkWednesday";
            chkWednesday.Size = new Size(47, 19);
            chkWednesday.TabIndex = 2;
            chkWednesday.Text = "Mer";
            // 
            // chkThursday
            // 
            chkThursday.AutoSize = true;
            chkThursday.Checked = true;
            chkThursday.CheckState = CheckState.Checked;
            chkThursday.Font = new Font("Segoe UI", 8.5F);
            chkThursday.Location = new Point(330, 18);
            chkThursday.Name = "chkThursday";
            chkThursday.Size = new Size(44, 19);
            chkThursday.TabIndex = 3;
            chkThursday.Text = "Gio";
            // 
            // chkFriday
            // 
            chkFriday.AutoSize = true;
            chkFriday.Checked = true;
            chkFriday.CheckState = CheckState.Checked;
            chkFriday.Font = new Font("Segoe UI", 8.5F);
            chkFriday.Location = new Point(435, 18);
            chkFriday.Name = "chkFriday";
            chkFriday.Size = new Size(45, 19);
            chkFriday.TabIndex = 4;
            chkFriday.Text = "Ven";
            // 
            // chkSaturday
            // 
            chkSaturday.AutoSize = true;
            chkSaturday.Checked = true;
            chkSaturday.CheckState = CheckState.Checked;
            chkSaturday.Font = new Font("Segoe UI", 8.5F);
            chkSaturday.Location = new Point(540, 18);
            chkSaturday.Name = "chkSaturday";
            chkSaturday.Size = new Size(45, 19);
            chkSaturday.TabIndex = 5;
            chkSaturday.Text = "Sab";
            // 
            // chkSunday
            // 
            chkSunday.AutoSize = true;
            chkSunday.Checked = true;
            chkSunday.CheckState = CheckState.Checked;
            chkSunday.Font = new Font("Segoe UI", 8.5F);
            chkSunday.Location = new Point(645, 18);
            chkSunday.Name = "chkSunday";
            chkSunday.Size = new Size(52, 19);
            chkSunday.TabIndex = 6;
            chkSunday.Text = "Dom";
            // 
            // grpManualSlots
            // 
            grpManualSlots.Controls.Add(flowManualSlots);
            grpManualSlots.Controls.Add(btnSelectAllSlots);
            grpManualSlots.Controls.Add(btnDeselectAllSlots);
            grpManualSlots.Controls.Add(lblManualDailyPasses);
            grpManualSlots.Controls.Add(numManualDailyPasses);
            grpManualSlots.Controls.Add(lblManualSlotCount);
            grpManualSlots.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpManualSlots.Location = new Point(49, 139);
            grpManualSlots.Name = "grpManualSlots";
            grpManualSlots.Size = new Size(990, 220);
            grpManualSlots.TabIndex = 3;
            grpManualSlots.TabStop = false;
            grpManualSlots.Text = "Selezione Manuale Punti Orari";
            grpManualSlots.Visible = false;
            // 
            // flowManualSlots
            // 
            flowManualSlots.AutoScroll = true;
            flowManualSlots.BorderStyle = BorderStyle.FixedSingle;
            flowManualSlots.Location = new Point(15, 25);
            flowManualSlots.Name = "flowManualSlots";
            flowManualSlots.Size = new Size(700, 160);
            flowManualSlots.TabIndex = 0;
            // 
            // btnSelectAllSlots
            // 
            btnSelectAllSlots.Font = new Font("Segoe UI", 8.5F);
            btnSelectAllSlots.Location = new Point(730, 25);
            btnSelectAllSlots.Name = "btnSelectAllSlots";
            btnSelectAllSlots.Size = new Size(130, 26);
            btnSelectAllSlots.TabIndex = 1;
            btnSelectAllSlots.Text = "✓ Seleziona Tutti";
            btnSelectAllSlots.UseVisualStyleBackColor = true;
            btnSelectAllSlots.Click += btnSelectAllSlots_Click;
            // 
            // btnDeselectAllSlots
            // 
            btnDeselectAllSlots.Font = new Font("Segoe UI", 8.5F);
            btnDeselectAllSlots.Location = new Point(730, 57);
            btnDeselectAllSlots.Name = "btnDeselectAllSlots";
            btnDeselectAllSlots.Size = new Size(135, 26);
            btnDeselectAllSlots.TabIndex = 2;
            btnDeselectAllSlots.Text = "✖ Deseleziona Tutti";
            btnDeselectAllSlots.UseVisualStyleBackColor = true;
            btnDeselectAllSlots.Click += btnDeselectAllSlots_Click;
            // 
            // lblManualDailyPasses
            // 
            lblManualDailyPasses.AutoSize = true;
            lblManualDailyPasses.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblManualDailyPasses.Location = new Point(730, 100);
            lblManualDailyPasses.Name = "lblManualDailyPasses";
            lblManualDailyPasses.TabIndex = 3;
            lblManualDailyPasses.Text = "Passaggi al giorno:";
            // 
            // numManualDailyPasses
            // 
            numManualDailyPasses.Font = new Font("Segoe UI", 10F);
            numManualDailyPasses.Location = new Point(730, 119);
            numManualDailyPasses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numManualDailyPasses.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numManualDailyPasses.Name = "numManualDailyPasses";
            numManualDailyPasses.Size = new Size(60, 25);
            numManualDailyPasses.TabIndex = 4;
            numManualDailyPasses.Value = new decimal(new int[] { 8, 0, 0, 0 });
            numManualDailyPasses.ValueChanged += numManualDailyPasses_ValueChanged;
            // 
            // lblManualSlotCount
            // 
            lblManualSlotCount.AutoSize = true;
            lblManualSlotCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblManualSlotCount.ForeColor = Color.FromArgb(0, 123, 255);
            lblManualSlotCount.Location = new Point(730, 155);
            lblManualSlotCount.Name = "lblManualSlotCount";
            lblManualSlotCount.TabIndex = 5;
            lblManualSlotCount.Text = "Slot selezionati: 0 / 8";
            // 
            // pnlStep3
            // 
            pnlStep3.BackColor = Color.White;
            pnlStep3.BorderStyle = BorderStyle.FixedSingle;
            pnlStep3.Controls.Add(lblStep3Title);
            pnlStep3.Controls.Add(progressDays);
            pnlStep3.Controls.Add(lblProgress);
            pnlStep3.Controls.Add(btnConfirmAll);
            pnlStep3.Location = new Point(20, 703);
            pnlStep3.Name = "pnlStep3";
            pnlStep3.Size = new Size(1069, 480);
            pnlStep3.TabIndex = 2;
            pnlStep3.Paint += pnlStep3_Paint;
            // 
            // lblStep3Title
            // 
            lblStep3Title.BackColor = Color.FromArgb(40, 167, 69);
            lblStep3Title.Dock = DockStyle.Top;
            lblStep3Title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStep3Title.ForeColor = Color.White;
            lblStep3Title.Location = new Point(0, 0);
            lblStep3Title.Name = "lblStep3Title";
            lblStep3Title.Padding = new Padding(15, 12, 0, 0);
            lblStep3Title.Size = new Size(1067, 45);
            lblStep3Title.TabIndex = 0;
            lblStep3Title.Text = "✅ STEP 3: Revisione Schedulazione";
            // 
            // progressDays
            // 
            progressDays.Location = new Point(180, 433);
            progressDays.Name = "progressDays";
            progressDays.Size = new Size(485, 15);
            progressDays.TabIndex = 1;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Segoe UI", 9F);
            lblProgress.Location = new Point(671, 433);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(156, 15);
            lblProgress.TabIndex = 2;
            lblProgress.Text = "Giorni confermati: 0 / 0 (0%)";
            // 
            // btnConfirmAll
            // 
            btnConfirmAll.BackColor = Color.FromArgb(40, 167, 69);
            btnConfirmAll.FlatStyle = FlatStyle.Flat;
            btnConfirmAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConfirmAll.ForeColor = Color.White;
            btnConfirmAll.Location = new Point(25, 422);
            btnConfirmAll.Name = "btnConfirmAll";
            btnConfirmAll.Size = new Size(145, 37);
            btnConfirmAll.TabIndex = 3;
            btnConfirmAll.Text = "⏭️ Conferma Tutti";
            btnConfirmAll.UseVisualStyleBackColor = false;
            btnConfirmAll.Click += btnConfirmAll_Click;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(240, 240, 240);
            panelButtons.BorderStyle = BorderStyle.FixedSingle;
            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 700);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1131, 70);
            panelButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(40, 167, 69);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(834, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = "💾 Salva";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(960, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "✖ Annulla";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // CampaignWizardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1131, 770);
            Controls.Add(pnlMainContainer);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CampaignWizardForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📋 Nuova Campagna Pubblicitaria";
            pnlMainContainer.ResumeLayout(false);
            pnlStep1.ResumeLayout(false);
            pnlStep1.PerformLayout();
            grpPeriod.ResumeLayout(false);
            grpPeriod.PerformLayout();
            pnlStep2.ResumeLayout(false);
            grpDistribution.ResumeLayout(false);
            grpDistribution.PerformLayout();
            grpAutoConfig.ResumeLayout(false);
            grpAutoConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDailyPasses).EndInit();
            ((System.ComponentModel.ISupportInitialize)numManualDailyPasses).EndInit();
            grpDays.ResumeLayout(false);
            grpDays.PerformLayout();
            grpManualSlots.ResumeLayout(false);
            grpManualSlots.PerformLayout();
            pnlStep3.ResumeLayout(false);
            pnlStep3.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}