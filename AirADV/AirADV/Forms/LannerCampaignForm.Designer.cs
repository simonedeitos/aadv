namespace AirADV.Forms
{
    partial class LannerCampaignForm
    {
        private System.ComponentModel.IContainer components = null;

        // Pannello superiore: Editor campagna
        private System.Windows.Forms.GroupBox grpEditPanel;
        private System.Windows.Forms.Label lblClientNameLabel;
        private System.Windows.Forms.TextBox txtClientName;
        private System.Windows.Forms.Label lblCampaignNameLabel;
        private System.Windows.Forms.TextBox txtCampaignName;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblDailySlots;
        private System.Windows.Forms.NumericUpDown numDailySlots;
        private System.Windows.Forms.Label lblDurationMinutes;
        private System.Windows.Forms.NumericUpDown numDurationMinutes;
        private System.Windows.Forms.Label lblSlotTimesLabel;
        private System.Windows.Forms.TextBox txtSlotTimes;
        private System.Windows.Forms.Label lblSlotTimesHint;
        private System.Windows.Forms.Button btnDefineSlotTimes;
        private System.Windows.Forms.Label lblImageLabel;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.PictureBox picImagePreview;

        // Bottoni azioni
        private System.Windows.Forms.Button btnNewCampaign;
        private System.Windows.Forms.Button btnSaveCampaign;
        private System.Windows.Forms.Button btnCancelEdit;

        // Griglia campagne
        private System.Windows.Forms.DataGridView dgvCampaigns;

        // Pannello inferiore
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblCampaignCount;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LannerCampaignForm));
            grpEditPanel = new GroupBox();
            lblClientNameLabel = new Label();
            txtClientName = new TextBox();
            lblCampaignNameLabel = new Label();
            txtCampaignName = new TextBox();
            lblStartDate = new Label();
            dtpStartDate = new DateTimePicker();
            lblEndDate = new Label();
            dtpEndDate = new DateTimePicker();
            lblDailySlots = new Label();
            numDailySlots = new NumericUpDown();
            lblDurationMinutes = new Label();
            numDurationMinutes = new NumericUpDown();
            lblSlotTimesLabel = new Label();
            txtSlotTimes = new TextBox();
            lblSlotTimesHint = new Label();
            btnDefineSlotTimes = new Button();
            lblImageLabel = new Label();
            txtImagePath = new TextBox();
            btnBrowseImage = new Button();
            picImagePreview = new PictureBox();
            btnNewCampaign = new Button();
            btnSaveCampaign = new Button();
            btnCancelEdit = new Button();
            dgvCampaigns = new DataGridView();
            panelBottom = new Panel();
            lblCampaignCount = new Label();
            grpEditPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDailySlots).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDurationMinutes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picImagePreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // grpEditPanel
            // 
            grpEditPanel.BackColor = Color.White;
            grpEditPanel.Controls.Add(lblClientNameLabel);
            grpEditPanel.Controls.Add(txtClientName);
            grpEditPanel.Controls.Add(lblCampaignNameLabel);
            grpEditPanel.Controls.Add(txtCampaignName);
            grpEditPanel.Controls.Add(lblStartDate);
            grpEditPanel.Controls.Add(dtpStartDate);
            grpEditPanel.Controls.Add(lblEndDate);
            grpEditPanel.Controls.Add(dtpEndDate);
            grpEditPanel.Controls.Add(lblDailySlots);
            grpEditPanel.Controls.Add(numDailySlots);
            grpEditPanel.Controls.Add(lblDurationMinutes);
            grpEditPanel.Controls.Add(numDurationMinutes);
            grpEditPanel.Controls.Add(lblSlotTimesLabel);
            grpEditPanel.Controls.Add(txtSlotTimes);
            grpEditPanel.Controls.Add(lblSlotTimesHint);
            grpEditPanel.Controls.Add(btnDefineSlotTimes);
            grpEditPanel.Controls.Add(lblImageLabel);
            grpEditPanel.Controls.Add(txtImagePath);
            grpEditPanel.Controls.Add(btnBrowseImage);
            grpEditPanel.Controls.Add(picImagePreview);
            grpEditPanel.Controls.Add(btnNewCampaign);
            grpEditPanel.Controls.Add(btnSaveCampaign);
            grpEditPanel.Controls.Add(btnCancelEdit);
            grpEditPanel.Dock = DockStyle.Top;
            grpEditPanel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpEditPanel.Location = new Point(0, 0);
            grpEditPanel.Name = "grpEditPanel";
            grpEditPanel.Size = new Size(1100, 280);
            grpEditPanel.TabIndex = 0;
            grpEditPanel.TabStop = false;
            grpEditPanel.Text = "📝 Campagna Lanner TV";
            // 
            // lblClientNameLabel
            // 
            lblClientNameLabel.AutoSize = true;
            lblClientNameLabel.Font = new Font("Segoe UI", 9F);
            lblClientNameLabel.Location = new Point(20, 35);
            lblClientNameLabel.Name = "lblClientNameLabel";
            lblClientNameLabel.Size = new Size(47, 15);
            lblClientNameLabel.TabIndex = 0;
            lblClientNameLabel.Text = "Cliente:";
            // 
            // txtClientName
            // 
            txtClientName.Font = new Font("Segoe UI", 10F);
            txtClientName.Location = new Point(110, 32);
            txtClientName.Name = "txtClientName";
            txtClientName.Size = new Size(250, 25);
            txtClientName.TabIndex = 0;
            // 
            // lblCampaignNameLabel
            // 
            lblCampaignNameLabel.AutoSize = true;
            lblCampaignNameLabel.Font = new Font("Segoe UI", 9F);
            lblCampaignNameLabel.Location = new Point(380, 35);
            lblCampaignNameLabel.Name = "lblCampaignNameLabel";
            lblCampaignNameLabel.Size = new Size(68, 15);
            lblCampaignNameLabel.TabIndex = 1;
            lblCampaignNameLabel.Text = "Campagna:";
            // 
            // txtCampaignName
            // 
            txtCampaignName.Font = new Font("Segoe UI", 10F);
            txtCampaignName.Location = new Point(470, 32);
            txtCampaignName.Name = "txtCampaignName";
            txtCampaignName.Size = new Size(250, 25);
            txtCampaignName.TabIndex = 1;
            // 
            // lblStartDate
            // 
            lblStartDate.AutoSize = true;
            lblStartDate.Font = new Font("Segoe UI", 9F);
            lblStartDate.Location = new Point(20, 75);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(24, 15);
            lblStartDate.TabIndex = 2;
            lblStartDate.Text = "Da:";
            // 
            // dtpStartDate
            // 
            dtpStartDate.Font = new Font("Segoe UI", 10F);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(110, 72);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(130, 25);
            dtpStartDate.TabIndex = 2;
            // 
            // lblEndDate
            // 
            lblEndDate.AutoSize = true;
            lblEndDate.Font = new Font("Segoe UI", 9F);
            lblEndDate.Location = new Point(260, 75);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(18, 15);
            lblEndDate.TabIndex = 3;
            lblEndDate.Text = "A:";
            // 
            // dtpEndDate
            // 
            dtpEndDate.Font = new Font("Segoe UI", 10F);
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(290, 72);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(130, 25);
            dtpEndDate.TabIndex = 3;
            // 
            // lblDailySlots
            // 
            lblDailySlots.AutoSize = true;
            lblDailySlots.Font = new Font("Segoe UI", 9F);
            lblDailySlots.Location = new Point(450, 75);
            lblDailySlots.Name = "lblDailySlots";
            lblDailySlots.Size = new Size(71, 15);
            lblDailySlots.TabIndex = 4;
            lblDailySlots.Text = "Slot/Giorno:";
            // 
            // numDailySlots
            // 
            numDailySlots.Font = new Font("Segoe UI", 10F);
            numDailySlots.Location = new Point(540, 72);
            numDailySlots.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numDailySlots.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDailySlots.Name = "numDailySlots";
            numDailySlots.ReadOnly = true;
            numDailySlots.Size = new Size(60, 25);
            numDailySlots.TabIndex = 4;
            numDailySlots.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDurationMinutes
            // 
            lblDurationMinutes.AutoSize = true;
            lblDurationMinutes.Font = new Font("Segoe UI", 9F);
            lblDurationMinutes.Location = new Point(620, 75);
            lblDurationMinutes.Name = "lblDurationMinutes";
            lblDurationMinutes.Size = new Size(77, 15);
            lblDurationMinutes.TabIndex = 5;
            lblDurationMinutes.Text = "Durata (min):";
            // 
            // numDurationMinutes
            // 
            numDurationMinutes.Font = new Font("Segoe UI", 10F);
            numDurationMinutes.Location = new Point(710, 72);
            numDurationMinutes.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            numDurationMinutes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDurationMinutes.Name = "numDurationMinutes";
            numDurationMinutes.Size = new Size(60, 25);
            numDurationMinutes.TabIndex = 5;
            numDurationMinutes.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lblSlotTimesLabel
            // 
            lblSlotTimesLabel.AutoSize = true;
            lblSlotTimesLabel.Font = new Font("Segoe UI", 9F);
            lblSlotTimesLabel.Location = new Point(20, 115);
            lblSlotTimesLabel.Name = "lblSlotTimesLabel";
            lblSlotTimesLabel.Size = new Size(36, 15);
            lblSlotTimesLabel.TabIndex = 6;
            lblSlotTimesLabel.Text = "Orari:";
            // 
            // txtSlotTimes
            // 
            txtSlotTimes.BackColor = Color.FromArgb(245, 245, 245);
            txtSlotTimes.Font = new Font("Segoe UI", 10F);
            txtSlotTimes.Location = new Point(110, 112);
            txtSlotTimes.Name = "txtSlotTimes";
            txtSlotTimes.ReadOnly = true;
            txtSlotTimes.Size = new Size(490, 25);
            txtSlotTimes.TabIndex = 6;
            // 
            // lblSlotTimesHint
            // 
            lblSlotTimesHint.AutoSize = true;
            lblSlotTimesHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblSlotTimesHint.ForeColor = Color.Gray;
            lblSlotTimesHint.Location = new Point(110, 140);
            lblSlotTimesHint.Name = "lblSlotTimesHint";
            lblSlotTimesHint.Size = new Size(272, 13);
            lblSlotTimesHint.TabIndex = 7;
            lblSlotTimesHint.Text = "Clicca 'Definisci Orari' per impostare gli orari giornalieri";
            // 
            // btnDefineSlotTimes
            // 
            btnDefineSlotTimes.BackColor = Color.FromArgb(156, 39, 176);
            btnDefineSlotTimes.FlatStyle = FlatStyle.Flat;
            btnDefineSlotTimes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDefineSlotTimes.ForeColor = Color.White;
            btnDefineSlotTimes.Location = new Point(620, 110);
            btnDefineSlotTimes.Name = "btnDefineSlotTimes";
            btnDefineSlotTimes.Size = new Size(150, 29);
            btnDefineSlotTimes.TabIndex = 7;
            btnDefineSlotTimes.Text = "🕐 Definisci Orari";
            btnDefineSlotTimes.UseVisualStyleBackColor = false;
            btnDefineSlotTimes.Click += btnDefineSlotTimes_Click;
            // 
            // lblImageLabel
            // 
            lblImageLabel.AutoSize = true;
            lblImageLabel.Font = new Font("Segoe UI", 9F);
            lblImageLabel.Location = new Point(20, 170);
            lblImageLabel.Name = "lblImageLabel";
            lblImageLabel.Size = new Size(64, 15);
            lblImageLabel.TabIndex = 8;
            lblImageLabel.Text = "Immagine:";
            // 
            // txtImagePath
            // 
            txtImagePath.BackColor = Color.FromArgb(245, 245, 245);
            txtImagePath.Font = new Font("Segoe UI", 9F);
            txtImagePath.Location = new Point(110, 167);
            txtImagePath.Name = "txtImagePath";
            txtImagePath.ReadOnly = true;
            txtImagePath.Size = new Size(490, 23);
            txtImagePath.TabIndex = 8;
            // 
            // btnBrowseImage
            // 
            btnBrowseImage.BackColor = Color.FromArgb(0, 123, 255);
            btnBrowseImage.FlatStyle = FlatStyle.Flat;
            btnBrowseImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBrowseImage.ForeColor = Color.White;
            btnBrowseImage.Location = new Point(620, 164);
            btnBrowseImage.Name = "btnBrowseImage";
            btnBrowseImage.Size = new Size(100, 27);
            btnBrowseImage.TabIndex = 9;
            btnBrowseImage.Text = "📁 Sfoglia...";
            btnBrowseImage.UseVisualStyleBackColor = false;
            btnBrowseImage.Click += btnBrowseImage_Click;
            // 
            // picImagePreview
            // 
            picImagePreview.BackColor = Color.FromArgb(245, 245, 245);
            picImagePreview.BorderStyle = BorderStyle.FixedSingle;
            picImagePreview.Location = new Point(800, 30);
            picImagePreview.Name = "picImagePreview";
            picImagePreview.Size = new Size(270, 170);
            picImagePreview.SizeMode = PictureBoxSizeMode.Zoom;
            picImagePreview.TabIndex = 20;
            picImagePreview.TabStop = false;
            // 
            // btnNewCampaign
            // 
            btnNewCampaign.BackColor = Color.FromArgb(40, 167, 69);
            btnNewCampaign.FlatStyle = FlatStyle.Flat;
            btnNewCampaign.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewCampaign.ForeColor = Color.White;
            btnNewCampaign.Location = new Point(20, 220);
            btnNewCampaign.Name = "btnNewCampaign";
            btnNewCampaign.Size = new Size(180, 35);
            btnNewCampaign.TabIndex = 10;
            btnNewCampaign.Text = "➕ Nuova Campagna";
            btnNewCampaign.UseVisualStyleBackColor = false;
            btnNewCampaign.Click += btnNewCampaign_Click;
            // 
            // btnSaveCampaign
            // 
            btnSaveCampaign.BackColor = Color.FromArgb(40, 167, 69);
            btnSaveCampaign.Enabled = false;
            btnSaveCampaign.FlatStyle = FlatStyle.Flat;
            btnSaveCampaign.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveCampaign.ForeColor = Color.White;
            btnSaveCampaign.Location = new Point(220, 220);
            btnSaveCampaign.Name = "btnSaveCampaign";
            btnSaveCampaign.Size = new Size(120, 35);
            btnSaveCampaign.TabIndex = 11;
            btnSaveCampaign.Text = "💾 Salva";
            btnSaveCampaign.UseVisualStyleBackColor = false;
            btnSaveCampaign.Click += btnSaveCampaign_Click;
            // 
            // btnCancelEdit
            // 
            btnCancelEdit.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelEdit.Enabled = false;
            btnCancelEdit.FlatStyle = FlatStyle.Flat;
            btnCancelEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelEdit.ForeColor = Color.White;
            btnCancelEdit.Location = new Point(350, 220);
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new Size(120, 35);
            btnCancelEdit.TabIndex = 12;
            btnCancelEdit.Text = "✖ Annulla";
            btnCancelEdit.UseVisualStyleBackColor = false;
            btnCancelEdit.Click += btnCancelEdit_Click;
            // 
            // dgvCampaigns
            // 
            dgvCampaigns.AllowUserToAddRows = false;
            dgvCampaigns.AllowUserToDeleteRows = false;
            dgvCampaigns.BackgroundColor = Color.White;
            dgvCampaigns.BorderStyle = BorderStyle.None;
            dgvCampaigns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCampaigns.Dock = DockStyle.Fill;
            dgvCampaigns.Font = new Font("Segoe UI", 9F);
            dgvCampaigns.Location = new Point(0, 280);
            dgvCampaigns.MultiSelect = false;
            dgvCampaigns.Name = "dgvCampaigns";
            dgvCampaigns.ReadOnly = true;
            dgvCampaigns.RowHeadersVisible = false;
            dgvCampaigns.RowTemplate.Height = 35;
            dgvCampaigns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCampaigns.Size = new Size(1100, 370);
            dgvCampaigns.TabIndex = 1;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(45, 45, 48);
            panelBottom.Controls.Add(lblCampaignCount);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 650);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1100, 35);
            panelBottom.TabIndex = 2;
            // 
            // lblCampaignCount
            // 
            lblCampaignCount.AutoSize = true;
            lblCampaignCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCampaignCount.ForeColor = Color.White;
            lblCampaignCount.Location = new Point(15, 10);
            lblCampaignCount.Name = "lblCampaignCount";
            lblCampaignCount.Size = new Size(311, 15);
            lblCampaignCount.TabIndex = 0;
            lblCampaignCount.Text = "Totale: 0  |  \U0001f7e2 In corso: 0  |  🔵 Future: 0  |  🔴 Passate: 0";
            // 
            // LannerCampaignForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 685);
            Controls.Add(dgvCampaigns);
            Controls.Add(panelBottom);
            Controls.Add(grpEditPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(900, 600);
            Name = "LannerCampaignForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📺 Lanner TV - Programmazione";
            grpEditPanel.ResumeLayout(false);
            grpEditPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDailySlots).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDurationMinutes).EndInit();
            ((System.ComponentModel.ISupportInitialize)picImagePreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCampaigns).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}