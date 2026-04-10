namespace AirADV.Forms
{
	partial class StationManagerForm
	{
		private System.ComponentModel.IContainer components = null;

		// Top:  Lista emittenti
		private System.Windows.Forms.GroupBox grpStationList;
		private System.Windows.Forms.ListView lstStations;
		private System.Windows.Forms.ColumnHeader colID;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colFrequency;
		private System.Windows.Forms.ColumnHeader colPath;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnSelectClose;
		private System.Windows.Forms.Label lblStationsCount;

		// Bottom: Editor
		private System.Windows.Forms.GroupBox grpEditor;
		private System.Windows.Forms.Label lblStationName;
		private System.Windows.Forms.TextBox txtStationName;
		private System.Windows.Forms.Label lblFrequency;
		private System.Windows.Forms.TextBox txtFrequency;
		private System.Windows.Forms.CheckBox chkActive;

		private System.Windows.Forms.Label lblDatabasePath;
		private System.Windows.Forms.TextBox txtDatabasePath;
		private System.Windows.Forms.Button btnBrowseDatabasePath;
		private System.Windows.Forms.Label lblMediaPath;
		private System.Windows.Forms.TextBox txtMediaPath;
		private System.Windows.Forms.Button btnBrowseMediaPath;
		private System.Windows.Forms.Label lblReportsPath;
		private System.Windows.Forms.TextBox txtReportsPath;
		private System.Windows.Forms.Button btnBrowseReportsPath;
		private System.Windows.Forms.Button btnAutoSetup;

		private System.Windows.Forms.PictureBox picLogo;
		private System.Windows.Forms.Button btnBrowseLogo;
		private System.Windows.Forms.Button btnRemoveLogo;

		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEditSave;

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
            grpStationList = new GroupBox();
            lstStations = new ListView();
            colID = new ColumnHeader();
            colName = new ColumnHeader();
            colFrequency = new ColumnHeader();
            colPath = new ColumnHeader();
            btnSelect = new Button();
            btnSelectClose = new Button();
            lblStationsCount = new Label();
            grpEditor = new GroupBox();
            lblStationName = new Label();
            txtStationName = new TextBox();
            lblFrequency = new Label();
            txtFrequency = new TextBox();
            chkActive = new CheckBox();
            lblDatabasePath = new Label();
            txtDatabasePath = new TextBox();
            btnBrowseDatabasePath = new Button();
            lblMediaPath = new Label();
            txtMediaPath = new TextBox();
            btnBrowseMediaPath = new Button();
            lblReportsPath = new Label();
            txtReportsPath = new TextBox();
            btnBrowseReportsPath = new Button();
            btnAutoSetup = new Button();
            picLogo = new PictureBox();
            btnBrowseLogo = new Button();
            btnRemoveLogo = new Button();
            btnNew = new Button();
            btnDelete = new Button();
            btnEditSave = new Button();
            grpStationList.SuspendLayout();
            grpEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // grpStationList
            // 
            grpStationList.Controls.Add(lstStations);
            grpStationList.Controls.Add(btnSelect);
            grpStationList.Controls.Add(btnSelectClose);
            grpStationList.Controls.Add(lblStationsCount);
            grpStationList.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpStationList.Location = new Point(20, 20);
            grpStationList.Name = "grpStationList";
            grpStationList.Size = new Size(850, 280);
            grpStationList.TabIndex = 0;
            grpStationList.TabStop = false;
            grpStationList.Text = "📻 Emittenti Configurate";
            // 
            // lstStations
            // 
            lstStations.Columns.AddRange(new ColumnHeader[] { colID, colName, colFrequency, colPath });
            lstStations.FullRowSelect = true;
            lstStations.GridLines = true;
            lstStations.Location = new Point(20, 30);
            lstStations.MultiSelect = false;
            lstStations.Name = "lstStations";
            lstStations.Size = new Size(810, 180);
            lstStations.TabIndex = 0;
            lstStations.UseCompatibleStateImageBehavior = false;
            lstStations.View = View.Details;
            lstStations.SelectedIndexChanged += lstStations_SelectedIndexChanged;
            // 
            // colID
            // 
            colID.Text = "ID";
            colID.Width = 50;
            // 
            // colName
            // 
            colName.Text = "Nome Emittente";
            colName.Width = 250;
            // 
            // colFrequency
            // 
            colFrequency.Text = "Frequenza";
            colFrequency.Width = 200;
            // 
            // colPath
            // 
            colPath.Text = "Percorso Database";
            colPath.Width = 300;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = Color.FromArgb(40, 167, 69);
            btnSelect.Enabled = false;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSelect.ForeColor = Color.White;
            btnSelect.Location = new Point(20, 225);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(234, 40);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "✅ Seleziona e Cambia";
            btnSelect.UseVisualStyleBackColor = false;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnSelectClose
            // 
            btnSelectClose.BackColor = Color.FromArgb(108, 117, 125);
            btnSelectClose.FlatStyle = FlatStyle.Flat;
            btnSelectClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSelectClose.ForeColor = Color.White;
            btnSelectClose.Location = new Point(700, 225);
            btnSelectClose.Name = "btnSelectClose";
            btnSelectClose.Size = new Size(130, 40);
            btnSelectClose.TabIndex = 2;
            btnSelectClose.Text = "✖ Chiudi";
            btnSelectClose.UseVisualStyleBackColor = false;
            btnSelectClose.Click += btnSelectClose_Click;
            // 
            // lblStationsCount
            // 
            lblStationsCount.AutoSize = true;
            lblStationsCount.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblStationsCount.ForeColor = Color.Gray;
            lblStationsCount.Location = new Point(276, 238);
            lblStationsCount.Name = "lblStationsCount";
            lblStationsCount.Size = new Size(129, 15);
            lblStationsCount.TabIndex = 3;
            lblStationsCount.Text = "Emittenti configurate: 0";
            // 
            // grpEditor
            // 
            grpEditor.Controls.Add(lblStationName);
            grpEditor.Controls.Add(txtStationName);
            grpEditor.Controls.Add(lblFrequency);
            grpEditor.Controls.Add(txtFrequency);
            grpEditor.Controls.Add(chkActive);
            grpEditor.Controls.Add(lblDatabasePath);
            grpEditor.Controls.Add(txtDatabasePath);
            grpEditor.Controls.Add(btnBrowseDatabasePath);
            grpEditor.Controls.Add(lblMediaPath);
            grpEditor.Controls.Add(txtMediaPath);
            grpEditor.Controls.Add(btnBrowseMediaPath);
            grpEditor.Controls.Add(lblReportsPath);
            grpEditor.Controls.Add(txtReportsPath);
            grpEditor.Controls.Add(btnBrowseReportsPath);
            grpEditor.Controls.Add(btnAutoSetup);
            grpEditor.Controls.Add(picLogo);
            grpEditor.Controls.Add(btnBrowseLogo);
            grpEditor.Controls.Add(btnRemoveLogo);
            grpEditor.Controls.Add(btnNew);
            grpEditor.Controls.Add(btnDelete);
            grpEditor.Controls.Add(btnEditSave);
            grpEditor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpEditor.Location = new Point(20, 310);
            grpEditor.Name = "grpEditor";
            grpEditor.Size = new Size(850, 270);
            grpEditor.TabIndex = 1;
            grpEditor.TabStop = false;
            grpEditor.Text = "✏️ Editor Emittente";
            // 
            // lblStationName
            // 
            lblStationName.AutoSize = true;
            lblStationName.Font = new Font("Segoe UI", 9F);
            lblStationName.ImageAlign = ContentAlignment.MiddleRight;
            lblStationName.Location = new Point(126, 30);
            lblStationName.Name = "lblStationName";
            lblStationName.Size = new Size(90, 15);
            lblStationName.TabIndex = 0;
            lblStationName.Text = "Nome Stazione:";
            lblStationName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtStationName
            // 
            txtStationName.Font = new Font("Segoe UI", 9F);
            txtStationName.Location = new Point(229, 27);
            txtStationName.Name = "txtStationName";
            txtStationName.Size = new Size(300, 23);
            txtStationName.TabIndex = 1;
            // 
            // lblFrequency
            // 
            lblFrequency.AutoSize = true;
            lblFrequency.Font = new Font("Segoe UI", 9F);
            lblFrequency.Location = new Point(552, 30);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(119, 15);
            lblFrequency.TabIndex = 2;
            lblFrequency.Text = "Frequenza / SitoWeb:";
            lblFrequency.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtFrequency
            // 
            txtFrequency.Font = new Font("Segoe UI", 9F);
            txtFrequency.Location = new Point(677, 27);
            txtFrequency.Name = "txtFrequency";
            txtFrequency.Size = new Size(164, 23);
            txtFrequency.TabIndex = 3;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Font = new Font("Segoe UI", 9F);
            chkActive.Location = new Point(749, 177);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(57, 19);
            chkActive.TabIndex = 4;
            chkActive.Text = "Attiva";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // lblDatabasePath
            // 
            lblDatabasePath.AutoSize = true;
            lblDatabasePath.Font = new Font("Segoe UI", 9F);
            lblDatabasePath.ImageAlign = ContentAlignment.MiddleRight;
            lblDatabasePath.Location = new Point(126, 71);
            lblDatabasePath.Name = "lblDatabasePath";
            lblDatabasePath.Size = new Size(58, 15);
            lblDatabasePath.TabIndex = 5;
            lblDatabasePath.Text = "Database:";
            lblDatabasePath.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtDatabasePath
            // 
            txtDatabasePath.Font = new Font("Segoe UI", 9F);
            txtDatabasePath.Location = new Point(229, 67);
            txtDatabasePath.Name = "txtDatabasePath";
            txtDatabasePath.ReadOnly = true;
            txtDatabasePath.Size = new Size(472, 23);
            txtDatabasePath.TabIndex = 6;
            // 
            // btnBrowseDatabasePath
            // 
            btnBrowseDatabasePath.Font = new Font("Segoe UI", 9F);
            btnBrowseDatabasePath.Location = new Point(711, 67);
            btnBrowseDatabasePath.Name = "btnBrowseDatabasePath";
            btnBrowseDatabasePath.Size = new Size(130, 23);
            btnBrowseDatabasePath.TabIndex = 7;
            btnBrowseDatabasePath.Text = "📁 Sfoglia";
            btnBrowseDatabasePath.UseVisualStyleBackColor = true;
            btnBrowseDatabasePath.Click += btnBrowseDatabasePath_Click;
            // 
            // lblMediaPath
            // 
            lblMediaPath.AutoSize = true;
            lblMediaPath.Font = new Font("Segoe UI", 9F);
            lblMediaPath.ImageAlign = ContentAlignment.MiddleRight;
            lblMediaPath.Location = new Point(126, 105);
            lblMediaPath.Name = "lblMediaPath";
            lblMediaPath.Size = new Size(43, 15);
            lblMediaPath.TabIndex = 8;
            lblMediaPath.Text = "Media:";
            lblMediaPath.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtMediaPath
            // 
            txtMediaPath.Font = new Font("Segoe UI", 9F);
            txtMediaPath.Location = new Point(229, 102);
            txtMediaPath.Name = "txtMediaPath";
            txtMediaPath.ReadOnly = true;
            txtMediaPath.Size = new Size(472, 23);
            txtMediaPath.TabIndex = 9;
            // 
            // btnBrowseMediaPath
            // 
            btnBrowseMediaPath.Font = new Font("Segoe UI", 9F);
            btnBrowseMediaPath.Location = new Point(711, 102);
            btnBrowseMediaPath.Name = "btnBrowseMediaPath";
            btnBrowseMediaPath.Size = new Size(130, 23);
            btnBrowseMediaPath.TabIndex = 10;
            btnBrowseMediaPath.Text = "📁 Sfoglia";
            btnBrowseMediaPath.UseVisualStyleBackColor = true;
            btnBrowseMediaPath.Click += btnBrowseMediaPath_Click;
            // 
            // lblReportsPath
            // 
            lblReportsPath.AutoSize = true;
            lblReportsPath.Font = new Font("Segoe UI", 9F);
            lblReportsPath.ImageAlign = ContentAlignment.MiddleRight;
            lblReportsPath.Location = new Point(126, 142);
            lblReportsPath.Name = "lblReportsPath";
            lblReportsPath.Size = new Size(45, 15);
            lblReportsPath.TabIndex = 11;
            lblReportsPath.Text = "Report:";
            lblReportsPath.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtReportsPath
            // 
            txtReportsPath.Font = new Font("Segoe UI", 9F);
            txtReportsPath.Location = new Point(229, 137);
            txtReportsPath.Name = "txtReportsPath";
            txtReportsPath.ReadOnly = true;
            txtReportsPath.Size = new Size(472, 23);
            txtReportsPath.TabIndex = 12;
            // 
            // btnBrowseReportsPath
            // 
            btnBrowseReportsPath.Font = new Font("Segoe UI", 9F);
            btnBrowseReportsPath.Location = new Point(711, 137);
            btnBrowseReportsPath.Name = "btnBrowseReportsPath";
            btnBrowseReportsPath.Size = new Size(130, 23);
            btnBrowseReportsPath.TabIndex = 13;
            btnBrowseReportsPath.Text = "📁 Sfoglia";
            btnBrowseReportsPath.UseVisualStyleBackColor = true;
            btnBrowseReportsPath.Click += btnBrowseReportsPath_Click;
            // 
            // btnAutoSetup
            // 
            btnAutoSetup.BackColor = Color.FromArgb(40, 167, 69);
            btnAutoSetup.FlatStyle = FlatStyle.Flat;
            btnAutoSetup.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAutoSetup.ForeColor = Color.White;
            btnAutoSetup.Location = new Point(229, 170);
            btnAutoSetup.Name = "btnAutoSetup";
            btnAutoSetup.Size = new Size(472, 30);
            btnAutoSetup.TabIndex = 14;
            btnAutoSetup.Text = "⚡ Setup Automatico Cartelle";
            btnAutoSetup.UseVisualStyleBackColor = false;
            btnAutoSetup.Click += btnAutoSetup_Click;
            // 
            // picLogo
            // 
            picLogo.BackColor = Color.White;
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.Location = new Point(20, 187);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(120, 70);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 15;
            picLogo.TabStop = false;
            // 
            // btnBrowseLogo
            // 
            btnBrowseLogo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBrowseLogo.Location = new Point(146, 228);
            btnBrowseLogo.Name = "btnBrowseLogo";
            btnBrowseLogo.Size = new Size(89, 29);
            btnBrowseLogo.TabIndex = 16;
            btnBrowseLogo.Text = "📁 Logo...";
            btnBrowseLogo.UseVisualStyleBackColor = true;
            btnBrowseLogo.Click += btnBrowseLogo_Click;
            // 
            // btnRemoveLogo
            // 
            btnRemoveLogo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRemoveLogo.Location = new Point(241, 228);
            btnRemoveLogo.Name = "btnRemoveLogo";
            btnRemoveLogo.Size = new Size(89, 30);
            btnRemoveLogo.TabIndex = 17;
            btnRemoveLogo.Text = "🗑️ Rimuovi";
            btnRemoveLogo.UseVisualStyleBackColor = true;
            btnRemoveLogo.Click += btnRemoveLogo_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.FromArgb(0, 123, 255);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNew.ForeColor = Color.White;
            btnNew.Location = new Point(423, 222);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(132, 35);
            btnNew.TabIndex = 18;
            btnNew.Text = "➕ Nuova";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(561, 222);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(132, 35);
            btnDelete.TabIndex = 19;
            btnDelete.Text = "🗑️ Elimina";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEditSave
            // 
            btnEditSave.BackColor = Color.FromArgb(40, 167, 69);
            btnEditSave.FlatStyle = FlatStyle.Flat;
            btnEditSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEditSave.ForeColor = Color.White;
            btnEditSave.Location = new Point(699, 222);
            btnEditSave.Name = "btnEditSave";
            btnEditSave.Size = new Size(142, 35);
            btnEditSave.TabIndex = 20;
            btnEditSave.Text = "💾 Salva";
            btnEditSave.UseVisualStyleBackColor = false;
            btnEditSave.Click += btnEditSave_Click;
            // 
            // StationManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 588);
            Controls.Add(grpEditor);
            Controls.Add(grpStationList);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StationManagerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "🔄 Gestione Emittenti";
            grpStationList.ResumeLayout(false);
            grpStationList.PerformLayout();
            grpEditor.ResumeLayout(false);
            grpEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }
    }
}