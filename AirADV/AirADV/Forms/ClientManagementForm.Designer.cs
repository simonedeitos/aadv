namespace AirADV.Forms
{
    partial class ClientManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        // Split Container Principale
        private System.Windows.Forms.SplitContainer splitMain;

        // LEFT PANEL - Lista Clienti
        private System.Windows.Forms.Panel panelClientsTop;
        private System.Windows.Forms.Label lblClientsTitle;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.Button btnDeleteClient;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.Panel panelClientsBottom;
        private System.Windows.Forms.Label lblClientsCount;

        // RIGHT PANEL - Dettagli Cliente
        private System.Windows.Forms.TabControl tabClientDetails;

        // TAB 1: Dati Anagrafici
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.Label lblClientCode;
        private System.Windows.Forms.TextBox txtClientCode;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.TextBox txtClientName;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblVATNumber;
        private System.Windows.Forms.TextBox txtVATNumber;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.CheckBox chkClientActive;
        private System.Windows.Forms.Button btnSaveClient;

        // TAB 2: Spot
        private System.Windows.Forms.TabPage tabSpots;
        private System.Windows.Forms.Panel panelSpotsTop;
        private System.Windows.Forms.Button btnAddSpot;
        private System.Windows.Forms.Button btnImportSpots;
        private System.Windows.Forms.Button btnDeleteSpot;
        private System.Windows.Forms.Button btnSaveSpots;
        private System.Windows.Forms.DataGridView dgvSpots;
        private System.Windows.Forms.Panel panelSpotsBottom;
        private System.Windows.Forms.Label lblPlayerStatus;
        private System.Windows.Forms.Button btnPlaySpot;
        private System.Windows.Forms.Label lblSpotsCount;

        // ✅ NUOVO: Video Preview con LibVLCSharp
        private System.Windows.Forms.Panel pnlVideoPreview;
        private System.Windows.Forms.Label lblVideoTitle;
        private LibVLCSharp.WinForms.VideoView vlcVideoView;

        // TAB 3: Campagne
        private System.Windows.Forms.TabPage tabCampaigns;
        private System.Windows.Forms.Panel panelCampaignsTop;
        private System.Windows.Forms.Button btnAddCampaign;
        private System.Windows.Forms.Button btnEditCampaign;
        private System.Windows.Forms.Button btnDeleteCampaign;
        private System.Windows.Forms.DataGridView dgvCampaigns;
        private System.Windows.Forms.Panel panelCampaignsBottom;
        private System.Windows.Forms.Label lblCampaignsCount;

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
            splitMain = new SplitContainer();
            dgvClients = new DataGridView();
            panelClientsBottom = new Panel();
            lblClientsCount = new Label();
            panelClientsTop = new Panel();
            lblClientsTitle = new Label();
            btnAddClient = new Button();
            btnDeleteClient = new Button();
            tabClientDetails = new TabControl();
            tabInfo = new TabPage();
            btnSaveClient = new Button();
            chkClientActive = new CheckBox();
            txtNotes = new TextBox();
            lblNotes = new Label();
            txtVATNumber = new TextBox();
            lblVATNumber = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtPhone = new TextBox();
            lblPhone = new Label();
            txtPostalCode = new TextBox();
            lblPostalCode = new Label();
            txtCity = new TextBox();
            lblCity = new Label();
            txtAddress = new TextBox();
            lblAddress = new Label();
            txtCompanyName = new TextBox();
            lblCompanyName = new Label();
            txtClientName = new TextBox();
            lblClientName = new Label();
            txtClientCode = new TextBox();
            lblClientCode = new Label();
            tabSpots = new TabPage();
            dgvSpots = new DataGridView();
            panelSpotsBottom = new Panel();
            pnlVideoPreview = new Panel();
            vlcVideoView = new LibVLCSharp.WinForms.VideoView();
            lblVideoTitle = new Label();
            lblPlayerStatus = new Label();
            btnPlaySpot = new Button();
            lblSpotsCount = new Label();
            panelSpotsTop = new Panel();
            btnAddSpot = new Button();
            btnImportSpots = new Button();
            btnDeleteSpot = new Button();
            btnSaveSpots = new Button();
            tabCampaigns = new TabPage();
            dgvCampaigns = new DataGridView();
            panelCampaignsBottom = new Panel();
            lblCampaignsCount = new Label();
            panelCampaignsTop = new Panel();
            btnAddCampaign = new Button();
            btnEditCampaign = new Button();
            btnDeleteCampaign = new Button();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
            panelClientsBottom.SuspendLayout();
            panelClientsTop.SuspendLayout();
            tabClientDetails.SuspendLayout();
            tabInfo.SuspendLayout();
            tabSpots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSpots).BeginInit();
            panelSpotsBottom.SuspendLayout();
            pnlVideoPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)vlcVideoView).BeginInit();
            panelSpotsTop.SuspendLayout();
            tabCampaigns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).BeginInit();
            panelCampaignsBottom.SuspendLayout();
            panelCampaignsTop.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.BackColor = Color.FromArgb(250, 250, 250);
            splitMain.Panel1.Controls.Add(dgvClients);
            splitMain.Panel1.Controls.Add(panelClientsBottom);
            splitMain.Panel1.Controls.Add(panelClientsTop);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.BackColor = Color.White;
            splitMain.Panel2.Controls.Add(tabClientDetails);
            splitMain.Size = new Size(1645, 800);
            splitMain.SplitterDistance = 470;
            splitMain.TabIndex = 0;
            // 
            // dgvClients
            // 
            dgvClients.AllowUserToAddRows = false;
            dgvClients.AllowUserToDeleteRows = false;
            dgvClients.BackgroundColor = Color.White;
            dgvClients.BorderStyle = BorderStyle.None;
            dgvClients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClients.Dock = DockStyle.Fill;
            dgvClients.Location = new Point(0, 60);
            dgvClients.MultiSelect = false;
            dgvClients.Name = "dgvClients";
            dgvClients.ReadOnly = true;
            dgvClients.RowHeadersVisible = false;
            dgvClients.RowTemplate.Height = 30;
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.Size = new Size(470, 710);
            dgvClients.TabIndex = 1;
            // 
            // panelClientsBottom
            // 
            panelClientsBottom.BackColor = Color.FromArgb(240, 240, 240);
            panelClientsBottom.Controls.Add(lblClientsCount);
            panelClientsBottom.Dock = DockStyle.Bottom;
            panelClientsBottom.Location = new Point(0, 770);
            panelClientsBottom.Name = "panelClientsBottom";
            panelClientsBottom.Size = new Size(470, 30);
            panelClientsBottom.TabIndex = 2;
            // 
            // lblClientsCount
            // 
            lblClientsCount.AutoSize = true;
            lblClientsCount.Font = new Font("Segoe UI", 9F);
            lblClientsCount.Location = new Point(15, 8);
            lblClientsCount.Name = "lblClientsCount";
            lblClientsCount.Size = new Size(53, 15);
            lblClientsCount.TabIndex = 0;
            lblClientsCount.Text = "Clienti: 0";
            // 
            // panelClientsTop
            // 
            panelClientsTop.BackColor = Color.FromArgb(45, 45, 48);
            panelClientsTop.Controls.Add(lblClientsTitle);
            panelClientsTop.Controls.Add(btnAddClient);
            panelClientsTop.Controls.Add(btnDeleteClient);
            panelClientsTop.Dock = DockStyle.Top;
            panelClientsTop.Location = new Point(0, 0);
            panelClientsTop.Name = "panelClientsTop";
            panelClientsTop.Size = new Size(470, 60);
            panelClientsTop.TabIndex = 0;
            // 
            // lblClientsTitle
            // 
            lblClientsTitle.AutoSize = true;
            lblClientsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblClientsTitle.ForeColor = Color.White;
            lblClientsTitle.Location = new Point(15, 18);
            lblClientsTitle.Name = "lblClientsTitle";
            lblClientsTitle.Size = new Size(142, 21);
            lblClientsTitle.TabIndex = 0;
            lblClientsTitle.Text = "👥 Elenco Clienti";
            // 
            // btnAddClient
            // 
            btnAddClient.BackColor = Color.FromArgb(40, 167, 69);
            btnAddClient.FlatStyle = FlatStyle.Flat;
            btnAddClient.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddClient.ForeColor = Color.White;
            btnAddClient.Location = new Point(225, 15);
            btnAddClient.Name = "btnAddClient";
            btnAddClient.Size = new Size(131, 30);
            btnAddClient.TabIndex = 1;
            btnAddClient.Text = "➕ Nuovo";
            btnAddClient.UseVisualStyleBackColor = false;
            btnAddClient.Click += btnAddClient_Click;
            // 
            // btnDeleteClient
            // 
            btnDeleteClient.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteClient.FlatStyle = FlatStyle.Flat;
            btnDeleteClient.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeleteClient.ForeColor = Color.White;
            btnDeleteClient.Location = new Point(362, 15);
            btnDeleteClient.Name = "btnDeleteClient";
            btnDeleteClient.Size = new Size(95, 30);
            btnDeleteClient.TabIndex = 2;
            btnDeleteClient.Text = "🗑️ Elimina";
            btnDeleteClient.UseVisualStyleBackColor = false;
            btnDeleteClient.Click += btnDeleteClient_Click;
            // 
            // tabClientDetails
            // 
            tabClientDetails.Controls.Add(tabInfo);
            tabClientDetails.Controls.Add(tabSpots);
            tabClientDetails.Controls.Add(tabCampaigns);
            tabClientDetails.Dock = DockStyle.Fill;
            tabClientDetails.Font = new Font("Segoe UI", 10F);
            tabClientDetails.Location = new Point(0, 0);
            tabClientDetails.Name = "tabClientDetails";
            tabClientDetails.SelectedIndex = 0;
            tabClientDetails.Size = new Size(1171, 800);
            tabClientDetails.TabIndex = 0;
            // 
            // tabInfo
            // 
            tabInfo.BackColor = Color.White;
            tabInfo.Controls.Add(btnSaveClient);
            tabInfo.Controls.Add(chkClientActive);
            tabInfo.Controls.Add(txtNotes);
            tabInfo.Controls.Add(lblNotes);
            tabInfo.Controls.Add(txtVATNumber);
            tabInfo.Controls.Add(lblVATNumber);
            tabInfo.Controls.Add(txtEmail);
            tabInfo.Controls.Add(lblEmail);
            tabInfo.Controls.Add(txtPhone);
            tabInfo.Controls.Add(lblPhone);
            tabInfo.Controls.Add(txtPostalCode);
            tabInfo.Controls.Add(lblPostalCode);
            tabInfo.Controls.Add(txtCity);
            tabInfo.Controls.Add(lblCity);
            tabInfo.Controls.Add(txtAddress);
            tabInfo.Controls.Add(lblAddress);
            tabInfo.Controls.Add(txtCompanyName);
            tabInfo.Controls.Add(lblCompanyName);
            tabInfo.Controls.Add(txtClientName);
            tabInfo.Controls.Add(lblClientName);
            tabInfo.Controls.Add(txtClientCode);
            tabInfo.Controls.Add(lblClientCode);
            tabInfo.Location = new Point(4, 26);
            tabInfo.Name = "tabInfo";
            tabInfo.Padding = new Padding(20);
            tabInfo.Size = new Size(1163, 770);
            tabInfo.TabIndex = 0;
            tabInfo.Text = "📋 Dati Anagrafici";
            // 
            // btnSaveClient
            // 
            btnSaveClient.BackColor = Color.FromArgb(40, 167, 69);
            btnSaveClient.FlatStyle = FlatStyle.Flat;
            btnSaveClient.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveClient.ForeColor = Color.White;
            btnSaveClient.Location = new Point(200, 590);
            btnSaveClient.Name = "btnSaveClient";
            btnSaveClient.Size = new Size(150, 40);
            btnSaveClient.TabIndex = 21;
            btnSaveClient.Text = "💾 Salva Cliente";
            btnSaveClient.UseVisualStyleBackColor = false;
            btnSaveClient.Click += btnSaveClient_Click;
            // 
            // chkClientActive
            // 
            chkClientActive.AutoSize = true;
            chkClientActive.Checked = true;
            chkClientActive.CheckState = CheckState.Checked;
            chkClientActive.Location = new Point(200, 540);
            chkClientActive.Name = "chkClientActive";
            chkClientActive.Size = new Size(79, 23);
            chkClientActive.TabIndex = 20;
            chkClientActive.Text = "✓ Attivo";
            chkClientActive.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(200, 427);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(380, 80);
            txtNotes.TabIndex = 19;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(30, 430);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(42, 19);
            lblNotes.TabIndex = 18;
            lblNotes.Text = "Note:";
            // 
            // txtVATNumber
            // 
            txtVATNumber.Location = new Point(200, 377);
            txtVATNumber.Name = "txtVATNumber";
            txtVATNumber.Size = new Size(200, 25);
            txtVATNumber.TabIndex = 17;
            // 
            // lblVATNumber
            // 
            lblVATNumber.AutoSize = true;
            lblVATNumber.Location = new Point(30, 380);
            lblVATNumber.Name = "lblVATNumber";
            lblVATNumber.Size = new Size(77, 19);
            lblVATNumber.TabIndex = 16;
            lblVATNumber.Text = "Partita IVA:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(200, 327);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(380, 25);
            txtEmail.TabIndex = 15;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 330);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(44, 19);
            lblEmail.TabIndex = 14;
            lblEmail.Text = "Email:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(200, 277);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(200, 25);
            txtPhone.TabIndex = 13;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(30, 280);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(63, 19);
            lblPhone.TabIndex = 12;
            lblPhone.Text = "Telefono:";
            // 
            // txtPostalCode
            // 
            txtPostalCode.Location = new Point(510, 227);
            txtPostalCode.Name = "txtPostalCode";
            txtPostalCode.Size = new Size(70, 25);
            txtPostalCode.TabIndex = 11;
            // 
            // lblPostalCode
            // 
            lblPostalCode.AutoSize = true;
            lblPostalCode.Location = new Point(421, 230);
            lblPostalCode.Name = "lblPostalCode";
            lblPostalCode.Size = new Size(38, 19);
            lblPostalCode.TabIndex = 10;
            lblPostalCode.Text = "CAP:";
            // 
            // txtCity
            // 
            txtCity.Location = new Point(200, 227);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(200, 25);
            txtCity.TabIndex = 9;
            // 
            // lblCity
            // 
            lblCity.AutoSize = true;
            lblCity.Location = new Point(30, 230);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(41, 19);
            lblCity.TabIndex = 8;
            lblCity.Text = "Città:";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(200, 177);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(380, 25);
            txtAddress.TabIndex = 7;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(30, 180);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(67, 19);
            lblAddress.TabIndex = 6;
            lblAddress.Text = "Indirizzo: ";
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new Point(200, 127);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(380, 25);
            txtCompanyName.TabIndex = 5;
            // 
            // lblCompanyName
            // 
            lblCompanyName.AutoSize = true;
            lblCompanyName.Location = new Point(30, 130);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new Size(106, 19);
            lblCompanyName.TabIndex = 4;
            lblCompanyName.Text = "Ragione Sociale:";
            // 
            // txtClientName
            // 
            txtClientName.Location = new Point(200, 77);
            txtClientName.Name = "txtClientName";
            txtClientName.Size = new Size(380, 25);
            txtClientName.TabIndex = 3;
            // 
            // lblClientName
            // 
            lblClientName.AutoSize = true;
            lblClientName.Location = new Point(30, 80);
            lblClientName.Name = "lblClientName";
            lblClientName.Size = new Size(95, 19);
            lblClientName.TabIndex = 2;
            lblClientName.Text = "Nome Cliente:";
            // 
            // txtClientCode
            // 
            txtClientCode.Enabled = false;
            txtClientCode.Location = new Point(200, 27);
            txtClientCode.Name = "txtClientCode";
            txtClientCode.Size = new Size(123, 25);
            txtClientCode.TabIndex = 1;
            // 
            // lblClientCode
            // 
            lblClientCode.AutoSize = true;
            lblClientCode.Location = new Point(30, 30);
            lblClientCode.Name = "lblClientCode";
            lblClientCode.Size = new Size(99, 19);
            lblClientCode.TabIndex = 0;
            lblClientCode.Text = "Codice Cliente:";
            // 
            // tabSpots
            // 
            tabSpots.BackColor = Color.White;
            tabSpots.Controls.Add(dgvSpots);
            tabSpots.Controls.Add(panelSpotsBottom);
            tabSpots.Controls.Add(panelSpotsTop);
            tabSpots.Location = new Point(4, 26);
            tabSpots.Name = "tabSpots";
            tabSpots.Size = new Size(1163, 770);
            tabSpots.TabIndex = 1;
            tabSpots.Text = "🎵 Spot";
            // 
            // dgvSpots
            // 
            dgvSpots.AllowUserToAddRows = false;
            dgvSpots.BackgroundColor = Color.White;
            dgvSpots.BorderStyle = BorderStyle.None;
            dgvSpots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSpots.Dock = DockStyle.Fill;
            dgvSpots.Location = new Point(0, 50);
            dgvSpots.Name = "dgvSpots";
            dgvSpots.RowHeadersVisible = false;
            dgvSpots.RowTemplate.Height = 30;
            dgvSpots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSpots.Size = new Size(1163, 548);
            dgvSpots.TabIndex = 1;
            // 
            // panelSpotsBottom
            // 
            panelSpotsBottom.BackColor = Color.FromArgb(250, 250, 250);
            panelSpotsBottom.Controls.Add(pnlVideoPreview);
            panelSpotsBottom.Controls.Add(lblPlayerStatus);
            panelSpotsBottom.Controls.Add(btnPlaySpot);
            panelSpotsBottom.Controls.Add(lblSpotsCount);
            panelSpotsBottom.Dock = DockStyle.Bottom;
            panelSpotsBottom.Location = new Point(0, 598);
            panelSpotsBottom.Name = "panelSpotsBottom";
            panelSpotsBottom.Size = new Size(1163, 172);
            panelSpotsBottom.TabIndex = 2;
            // 
            // pnlVideoPreview
            // 
            pnlVideoPreview.BackColor = Color.Black;
            pnlVideoPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlVideoPreview.Controls.Add(vlcVideoView);
            pnlVideoPreview.Controls.Add(lblVideoTitle);
            pnlVideoPreview.Location = new Point(15, 6);
            pnlVideoPreview.Name = "pnlVideoPreview";
            pnlVideoPreview.Size = new Size(270, 160);
            pnlVideoPreview.TabIndex = 3;
            pnlVideoPreview.Visible = false;
            // 
            // vlcVideoView
            // 
            vlcVideoView.BackColor = Color.Black;
            vlcVideoView.Location = new Point(0, 20);
            vlcVideoView.MediaPlayer = null;
            vlcVideoView.Name = "vlcVideoView";
            vlcVideoView.Size = new Size(268, 138);
            vlcVideoView.TabIndex = 1;
            // 
            // lblVideoTitle
            // 
            lblVideoTitle.BackColor = Color.FromArgb(30, 30, 30);
            lblVideoTitle.Dock = DockStyle.Top;
            lblVideoTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblVideoTitle.ForeColor = Color.White;
            lblVideoTitle.Location = new Point(0, 0);
            lblVideoTitle.Name = "lblVideoTitle";
            lblVideoTitle.Padding = new Padding(4, 0, 0, 0);
            lblVideoTitle.Size = new Size(268, 20);
            lblVideoTitle.TabIndex = 0;
            lblVideoTitle.Text = "📺 Anteprima Video";
            lblVideoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPlayerStatus
            // 
            lblPlayerStatus.AutoSize = true;
            lblPlayerStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPlayerStatus.ForeColor = Color.FromArgb(0, 123, 255);
            lblPlayerStatus.Location = new Point(400, 48);
            lblPlayerStatus.Name = "lblPlayerStatus";
            lblPlayerStatus.Size = new Size(139, 19);
            lblPlayerStatus.TabIndex = 1;
            lblPlayerStatus.Text = "Nessun file caricato";
            // 
            // btnPlaySpot
            // 
            btnPlaySpot.BackColor = Color.FromArgb(40, 167, 69);
            btnPlaySpot.Enabled = false;
            btnPlaySpot.FlatStyle = FlatStyle.Flat;
            btnPlaySpot.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPlaySpot.ForeColor = Color.White;
            btnPlaySpot.Location = new Point(300, 40);
            btnPlaySpot.Name = "btnPlaySpot";
            btnPlaySpot.Size = new Size(40, 40);
            btnPlaySpot.TabIndex = 0;
            btnPlaySpot.Text = "▶";
            btnPlaySpot.UseVisualStyleBackColor = false;
            btnPlaySpot.FlatAppearance.BorderSize = 0;
            btnPlaySpot.Click += btnPlaySpot_Click;
            // 
            // lblSpotsCount
            // 
            lblSpotsCount.AutoSize = true;
            lblSpotsCount.Font = new Font("Segoe UI", 9F);
            lblSpotsCount.Location = new Point(948, 12);
            lblSpotsCount.Name = "lblSpotsCount";
            lblSpotsCount.Size = new Size(46, 15);
            lblSpotsCount.TabIndex = 2;
            lblSpotsCount.Text = "Spot:  0";
            // 
            // panelSpotsTop
            // 
            panelSpotsTop.BackColor = Color.FromArgb(240, 240, 240);
            panelSpotsTop.Controls.Add(btnAddSpot);
            panelSpotsTop.Controls.Add(btnImportSpots);
            panelSpotsTop.Controls.Add(btnDeleteSpot);
            panelSpotsTop.Controls.Add(btnSaveSpots);
            panelSpotsTop.Dock = DockStyle.Top;
            panelSpotsTop.Location = new Point(0, 0);
            panelSpotsTop.Name = "panelSpotsTop";
            panelSpotsTop.Size = new Size(1163, 50);
            panelSpotsTop.TabIndex = 0;
            // 
            // btnAddSpot
            // 
            btnAddSpot.BackColor = Color.FromArgb(40, 167, 69);
            btnAddSpot.FlatStyle = FlatStyle.Flat;
            btnAddSpot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddSpot.ForeColor = Color.White;
            btnAddSpot.Location = new Point(15, 10);
            btnAddSpot.Name = "btnAddSpot";
            btnAddSpot.Size = new Size(120, 32);
            btnAddSpot.TabIndex = 0;
            btnAddSpot.Text = "➕ Aggiungi";
            btnAddSpot.UseVisualStyleBackColor = false;
            btnAddSpot.Click += btnAddSpot_Click;
            // 
            // btnImportSpots
            // 
            btnImportSpots.BackColor = Color.FromArgb(0, 123, 255);
            btnImportSpots.FlatStyle = FlatStyle.Flat;
            btnImportSpots.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnImportSpots.ForeColor = Color.White;
            btnImportSpots.Location = new Point(145, 10);
            btnImportSpots.Name = "btnImportSpots";
            btnImportSpots.Size = new Size(140, 32);
            btnImportSpots.TabIndex = 1;
            btnImportSpots.Text = "📥 Importa File";
            btnImportSpots.UseVisualStyleBackColor = false;
            btnImportSpots.Click += btnImportSpots_Click;
            // 
            // btnDeleteSpot
            // 
            btnDeleteSpot.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteSpot.FlatStyle = FlatStyle.Flat;
            btnDeleteSpot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeleteSpot.ForeColor = Color.White;
            btnDeleteSpot.Location = new Point(295, 10);
            btnDeleteSpot.Name = "btnDeleteSpot";
            btnDeleteSpot.Size = new Size(120, 32);
            btnDeleteSpot.TabIndex = 2;
            btnDeleteSpot.Text = "🗑️ Elimina";
            btnDeleteSpot.UseVisualStyleBackColor = false;
            btnDeleteSpot.Click += btnDeleteSpot_Click;
            // 
            // btnSaveSpots
            // 
            btnSaveSpots.BackColor = Color.FromArgb(40, 167, 69);
            btnSaveSpots.FlatStyle = FlatStyle.Flat;
            btnSaveSpots.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveSpots.ForeColor = Color.White;
            btnSaveSpots.Location = new Point(840, 10);
            btnSaveSpots.Name = "btnSaveSpots";
            btnSaveSpots.Size = new Size(130, 32);
            btnSaveSpots.TabIndex = 3;
            btnSaveSpots.Text = "💾 Salva";
            btnSaveSpots.UseVisualStyleBackColor = false;
            btnSaveSpots.Click += btnSaveSpots_Click;
            // 
            // tabCampaigns
            // 
            tabCampaigns.BackColor = Color.White;
            tabCampaigns.Controls.Add(dgvCampaigns);
            tabCampaigns.Controls.Add(panelCampaignsBottom);
            tabCampaigns.Controls.Add(panelCampaignsTop);
            tabCampaigns.Location = new Point(4, 26);
            tabCampaigns.Name = "tabCampaigns";
            tabCampaigns.Size = new Size(1163, 770);
            tabCampaigns.TabIndex = 2;
            tabCampaigns.Text = "📅 Campagne";
            // 
            // dgvCampaigns
            // 
            dgvCampaigns.AllowUserToAddRows = false;
            dgvCampaigns.AllowUserToDeleteRows = false;
            dgvCampaigns.BackgroundColor = Color.White;
            dgvCampaigns.BorderStyle = BorderStyle.None;
            dgvCampaigns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCampaigns.Dock = DockStyle.Fill;
            dgvCampaigns.Location = new Point(0, 50);
            dgvCampaigns.Name = "dgvCampaigns";
            dgvCampaigns.ReadOnly = true;
            dgvCampaigns.RowHeadersVisible = false;
            dgvCampaigns.RowTemplate.Height = 30;
            dgvCampaigns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCampaigns.Size = new Size(1163, 690);
            dgvCampaigns.TabIndex = 1;
            // 
            // panelCampaignsBottom
            // 
            panelCampaignsBottom.BackColor = Color.FromArgb(240, 240, 240);
            panelCampaignsBottom.Controls.Add(lblCampaignsCount);
            panelCampaignsBottom.Dock = DockStyle.Bottom;
            panelCampaignsBottom.Location = new Point(0, 740);
            panelCampaignsBottom.Name = "panelCampaignsBottom";
            panelCampaignsBottom.Size = new Size(1163, 30);
            panelCampaignsBottom.TabIndex = 2;
            // 
            // lblCampaignsCount
            // 
            lblCampaignsCount.AutoSize = true;
            lblCampaignsCount.Font = new Font("Segoe UI", 9F);
            lblCampaignsCount.Location = new Point(15, 8);
            lblCampaignsCount.Name = "lblCampaignsCount";
            lblCampaignsCount.Size = new Size(77, 15);
            lblCampaignsCount.TabIndex = 0;
            lblCampaignsCount.Text = "Campagne: 0";
            // 
            // panelCampaignsTop
            // 
            panelCampaignsTop.BackColor = Color.FromArgb(240, 240, 240);
            panelCampaignsTop.Controls.Add(btnAddCampaign);
            panelCampaignsTop.Controls.Add(btnEditCampaign);
            panelCampaignsTop.Controls.Add(btnDeleteCampaign);
            panelCampaignsTop.Dock = DockStyle.Top;
            panelCampaignsTop.Location = new Point(0, 0);
            panelCampaignsTop.Name = "panelCampaignsTop";
            panelCampaignsTop.Size = new Size(1163, 50);
            panelCampaignsTop.TabIndex = 0;
            // 
            // btnAddCampaign
            // 
            btnAddCampaign.BackColor = Color.FromArgb(40, 167, 69);
            btnAddCampaign.FlatStyle = FlatStyle.Flat;
            btnAddCampaign.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddCampaign.ForeColor = Color.White;
            btnAddCampaign.Location = new Point(15, 10);
            btnAddCampaign.Name = "btnAddCampaign";
            btnAddCampaign.Size = new Size(170, 32);
            btnAddCampaign.TabIndex = 0;
            btnAddCampaign.Text = "➕ Nuova Campagna";
            btnAddCampaign.UseVisualStyleBackColor = false;
            btnAddCampaign.Click += btnAddCampaign_Click;
            // 
            // btnEditCampaign
            // 
            btnEditCampaign.BackColor = Color.FromArgb(0, 123, 255);
            btnEditCampaign.FlatStyle = FlatStyle.Flat;
            btnEditCampaign.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditCampaign.ForeColor = Color.White;
            btnEditCampaign.Location = new Point(195, 10);
            btnEditCampaign.Name = "btnEditCampaign";
            btnEditCampaign.Size = new Size(120, 32);
            btnEditCampaign.TabIndex = 1;
            btnEditCampaign.Text = "✏️ Modifica";
            btnEditCampaign.UseVisualStyleBackColor = false;
            btnEditCampaign.Click += btnEditCampaign_Click;
            // 
            // btnDeleteCampaign
            // 
            btnDeleteCampaign.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteCampaign.FlatStyle = FlatStyle.Flat;
            btnDeleteCampaign.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeleteCampaign.ForeColor = Color.White;
            btnDeleteCampaign.Location = new Point(325, 10);
            btnDeleteCampaign.Name = "btnDeleteCampaign";
            btnDeleteCampaign.Size = new Size(120, 32);
            btnDeleteCampaign.TabIndex = 2;
            btnDeleteCampaign.Text = "🗑️ Elimina";
            btnDeleteCampaign.UseVisualStyleBackColor = false;
            btnDeleteCampaign.Click += btnDeleteCampaign_Click;
            // 
            // ClientManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1645, 800);
            Controls.Add(splitMain);
            MinimumSize = new Size(1200, 700);
            Name = "ClientManagementForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "👥 Gestione Clienti";
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
            panelClientsBottom.ResumeLayout(false);
            panelClientsBottom.PerformLayout();
            panelClientsTop.ResumeLayout(false);
            panelClientsTop.PerformLayout();
            tabClientDetails.ResumeLayout(false);
            tabInfo.ResumeLayout(false);
            tabInfo.PerformLayout();
            tabSpots.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSpots).EndInit();
            panelSpotsBottom.ResumeLayout(false);
            panelSpotsBottom.PerformLayout();
            pnlVideoPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)vlcVideoView).EndInit();
            panelSpotsTop.ResumeLayout(false);
            tabCampaigns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).EndInit();
            panelCampaignsBottom.ResumeLayout(false);
            panelCampaignsBottom.PerformLayout();
            panelCampaignsTop.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}