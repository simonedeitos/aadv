namespace AirADV.Forms
{
    partial class CategoriesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch; // ✅ NUOVO
        private System.Windows.Forms.DataGridView dgvCategories;
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
            panelToolbar = new Panel();
            btnAdd = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            txtSearch = new TextBox();
            btnSave = new Button();
            dgvCategories = new DataGridView();
            panelBottom = new Panel();
            lblStatus = new Label();
            panelTop.SuspendLayout();
            panelToolbar.SuspendLayout();
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
            panelTop.Size = new Size(922, 50);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(15, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(261, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🏷️ Categorie Merceologiche";
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.FromArgb(240, 240, 240);
            panelToolbar.Controls.Add(btnAdd);
            panelToolbar.Controls.Add(btnDelete);
            panelToolbar.Controls.Add(btnRefresh);
            panelToolbar.Controls.Add(txtSearch);
            panelToolbar.Controls.Add(btnSave);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 50);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(922, 50);
            panelToolbar.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(15, 10);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(160, 32);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "➕ Aggiungi Categoria";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(185, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 32);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "🗑️ Elimina";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(315, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 32);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Aggiorna";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Location = new Point(455, 13);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(280, 25);
            txtSearch.TabIndex = 3;
            txtSearch.Text = "🔍 Cerca categoria...";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 123, 255);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(790, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 32);
            btnSave.TabIndex = 4;
            btnSave.Text = "💾 Salva";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // dgvCategories
            // 
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategories.Dock = DockStyle.Fill;
            dgvCategories.Location = new Point(0, 100);
            dgvCategories.Name = "dgvCategories";
            dgvCategories.RowTemplate.Height = 30;
            dgvCategories.Size = new Size(922, 470);
            dgvCategories.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(240, 240, 240);
            panelBottom.Controls.Add(lblStatus);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 570);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(922, 30);
            panelBottom.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(15, 8);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(137, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Categorie configurate:  0";
            // 
            // CategoriesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 600);
            Controls.Add(dgvCategories);
            Controls.Add(panelBottom);
            Controls.Add(panelToolbar);
            Controls.Add(panelTop);
            MinimumSize = new Size(800, 500);
            Name = "CategoriesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🏷️ Categorie Merceologiche";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelToolbar.ResumeLayout(false);
            panelToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}