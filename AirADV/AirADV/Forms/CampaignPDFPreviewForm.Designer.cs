namespace AirADV.Forms
{
    partial class CampaignPDFPreviewForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSavePDF;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        // ✅ FIX:  Variabile membro rinominata per coerenza
        private System.Windows.Forms.PrintPreviewControl _previewControl;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignPDFPreviewForm));
            panelTop = new Panel();
            lblTitle = new Label();
            panelButtons = new Panel();
            btnSavePDF = new Button();
            btnPrint = new Button();
            btnFirstPage = new Button();
            btnPrevPage = new Button();
            lblPageInfo = new Label();
            btnNextPage = new Button();
            btnLastPage = new Button();
            btnZoomIn = new Button();
            btnZoomOut = new Button();
            btnClose = new Button();
            _previewControl = new PrintPreviewControl();
            panelTop.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(0, 123, 255);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1200, 60);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1200, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📄 Anteprima Programmazione Campagna Pubblicitaria";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(240, 240, 240);
            panelButtons.BorderStyle = BorderStyle.FixedSingle;
            panelButtons.Controls.Add(btnSavePDF);
            panelButtons.Controls.Add(btnPrint);
            panelButtons.Controls.Add(btnFirstPage);
            panelButtons.Controls.Add(btnPrevPage);
            panelButtons.Controls.Add(lblPageInfo);
            panelButtons.Controls.Add(btnNextPage);
            panelButtons.Controls.Add(btnLastPage);
            panelButtons.Controls.Add(btnZoomIn);
            panelButtons.Controls.Add(btnZoomOut);
            panelButtons.Controls.Add(btnClose);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 770);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1200, 50);
            panelButtons.TabIndex = 1;
            // 
            // btnSavePDF
            // 
            btnSavePDF.BackColor = Color.FromArgb(40, 167, 69);
            btnSavePDF.FlatAppearance.BorderSize = 0;
            btnSavePDF.FlatStyle = FlatStyle.Flat;
            btnSavePDF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSavePDF.ForeColor = Color.White;
            btnSavePDF.Location = new Point(30, 8);
            btnSavePDF.Name = "btnSavePDF";
            btnSavePDF.Size = new Size(140, 34);
            btnSavePDF.TabIndex = 0;
            btnSavePDF.Text = "💾 Salva PDF";
            btnSavePDF.UseVisualStyleBackColor = false;
            btnSavePDF.Click += btnSavePDF_Click;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(0, 123, 255);
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrint.ForeColor = Color.White;
            btnPrint.Location = new Point(190, 8);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(140, 34);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "🖨️ Stampa";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnFirstPage
            // 
            btnFirstPage.BackColor = Color.FromArgb(108, 117, 125);
            btnFirstPage.FlatAppearance.BorderSize = 0;
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnFirstPage.ForeColor = Color.White;
            btnFirstPage.Location = new Point(440, 8);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(50, 34);
            btnFirstPage.TabIndex = 5;
            btnFirstPage.Text = "⏮";
            btnFirstPage.UseVisualStyleBackColor = false;
            btnFirstPage.Click += btnFirstPage_Click;
            // 
            // btnPrevPage
            // 
            btnPrevPage.BackColor = Color.FromArgb(108, 117, 125);
            btnPrevPage.FlatAppearance.BorderSize = 0;
            btnPrevPage.FlatStyle = FlatStyle.Flat;
            btnPrevPage.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnPrevPage.ForeColor = Color.White;
            btnPrevPage.Location = new Point(500, 8);
            btnPrevPage.Name = "btnPrevPage";
            btnPrevPage.Size = new Size(50, 34);
            btnPrevPage.TabIndex = 6;
            btnPrevPage.Text = "◀";
            btnPrevPage.UseVisualStyleBackColor = false;
            btnPrevPage.Click += btnPrevPage_Click;
            // 
            // lblPageInfo
            // 
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.ForeColor = Color.FromArgb(50, 50, 50);
            lblPageInfo.Location = new Point(560, 8);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(150, 34);
            lblPageInfo.TabIndex = 7;
            lblPageInfo.Text = "Pagina 1 / 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnNextPage
            // 
            btnNextPage.BackColor = Color.FromArgb(108, 117, 125);
            btnNextPage.FlatAppearance.BorderSize = 0;
            btnNextPage.FlatStyle = FlatStyle.Flat;
            btnNextPage.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnNextPage.ForeColor = Color.White;
            btnNextPage.Location = new Point(720, 8);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(50, 34);
            btnNextPage.TabIndex = 8;
            btnNextPage.Text = "▶";
            btnNextPage.UseVisualStyleBackColor = false;
            btnNextPage.Click += btnNextPage_Click;
            // 
            // btnLastPage
            // 
            btnLastPage.BackColor = Color.FromArgb(108, 117, 125);
            btnLastPage.FlatAppearance.BorderSize = 0;
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLastPage.ForeColor = Color.White;
            btnLastPage.Location = new Point(780, 8);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(50, 34);
            btnLastPage.TabIndex = 9;
            btnLastPage.Text = "⏭";
            btnLastPage.UseVisualStyleBackColor = false;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // btnZoomIn
            // 
            btnZoomIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomIn.BackColor = Color.FromArgb(108, 117, 125);
            btnZoomIn.FlatAppearance.BorderSize = 0;
            btnZoomIn.FlatStyle = FlatStyle.Flat;
            btnZoomIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnZoomIn.ForeColor = Color.White;
            btnZoomIn.Location = new Point(930, 8);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(44, 34);
            btnZoomIn.TabIndex = 2;
            btnZoomIn.Text = "🔍+";
            btnZoomIn.UseVisualStyleBackColor = false;
            btnZoomIn.Click += btnZoomIn_Click;
            // 
            // btnZoomOut
            // 
            btnZoomOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomOut.BackColor = Color.FromArgb(108, 117, 125);
            btnZoomOut.FlatAppearance.BorderSize = 0;
            btnZoomOut.FlatStyle = FlatStyle.Flat;
            btnZoomOut.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnZoomOut.ForeColor = Color.White;
            btnZoomOut.Location = new Point(984, 8);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(44, 34);
            btnZoomOut.TabIndex = 3;
            btnZoomOut.Text = "🔍-";
            btnZoomOut.UseVisualStyleBackColor = false;
            btnZoomOut.Click += btnZoomOut_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(220, 53, 69);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1070, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 34);
            btnClose.TabIndex = 4;
            btnClose.Text = "✖ Chiudi";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // _previewControl
            // 
            _previewControl.AutoZoom = false;
            _previewControl.BackColor = Color.FromArgb(64, 64, 64);
            _previewControl.Dock = DockStyle.Fill;
            _previewControl.Location = new Point(0, 60);
            _previewControl.Name = "_previewControl";
            _previewControl.Size = new Size(1200, 710);
            _previewControl.TabIndex = 2;
            _previewControl.UseAntiAlias = true;
            _previewControl.Zoom = 1D;
            // 
            // CampaignPDFPreviewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 820);
            Controls.Add(_previewControl);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1000, 700);
            Name = "CampaignPDFPreviewForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Anteprima PDF - Programmazione Campagna";
            WindowState = FormWindowState.Maximized;
            panelTop.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}