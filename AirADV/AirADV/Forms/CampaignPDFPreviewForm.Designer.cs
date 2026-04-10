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
            panelTop = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            panelButtons = new System.Windows.Forms.Panel();
            btnSavePDF = new System.Windows.Forms.Button();
            btnPrint = new System.Windows.Forms.Button();
            btnZoomIn = new System.Windows.Forms.Button();
            btnZoomOut = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            // ✅ FIX:  Usa il nome corretto
            _previewControl = new System.Windows.Forms.PrintPreviewControl();

            panelTop.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();

            // 
            // panelTop
            // 
            panelTop.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1200, 60);
            panelTop.TabIndex = 0;

            // 
            // lblTitle
            // 
            lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(1200, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📄 Anteprima Programmazione Campagna Pubblicitaria";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // panelButtons
            // 
            panelButtons.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelButtons.Controls.Add(btnSavePDF);
            panelButtons.Controls.Add(btnPrint);
            panelButtons.Controls.Add(btnZoomIn);
            panelButtons.Controls.Add(btnZoomOut);
            panelButtons.Controls.Add(btnClose);
            panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelButtons.Location = new System.Drawing.Point(0, 740);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new System.Drawing.Size(1200, 80);
            panelButtons.TabIndex = 1;

            // 
            // btnSavePDF
            // 
            btnSavePDF.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnSavePDF.FlatAppearance.BorderSize = 0;
            btnSavePDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSavePDF.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnSavePDF.ForeColor = System.Drawing.Color.White;
            btnSavePDF.Location = new System.Drawing.Point(30, 15);
            btnSavePDF.Name = "btnSavePDF";
            btnSavePDF.Size = new System.Drawing.Size(180, 50);
            btnSavePDF.TabIndex = 0;
            btnSavePDF.Text = "💾 Salva PDF";
            btnSavePDF.UseVisualStyleBackColor = false;
            btnSavePDF.Click += btnSavePDF_Click;

            // 
            // btnPrint
            // 
            btnPrint.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPrint.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnPrint.ForeColor = System.Drawing.Color.White;
            btnPrint.Location = new System.Drawing.Point(230, 15);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(180, 50);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "🖨️ Stampa";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;

            // 
            // btnZoomIn
            // 
            btnZoomIn.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnZoomIn.FlatAppearance.BorderSize = 0;
            btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnZoomIn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            btnZoomIn.ForeColor = System.Drawing.Color.White;
            btnZoomIn.Location = new System.Drawing.Point(830, 15);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new System.Drawing.Size(60, 50);
            btnZoomIn.TabIndex = 2;
            btnZoomIn.Text = "🔍+";
            btnZoomIn.UseVisualStyleBackColor = false;
            btnZoomIn.Click += btnZoomIn_Click;

            // 
            // btnZoomOut
            // 
            btnZoomOut.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnZoomOut.FlatAppearance.BorderSize = 0;
            btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnZoomOut.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            btnZoomOut.ForeColor = System.Drawing.Color.White;
            btnZoomOut.Location = new System.Drawing.Point(900, 15);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new System.Drawing.Size(60, 50);
            btnZoomOut.TabIndex = 3;
            btnZoomOut.Text = "🔍-";
            btnZoomOut.UseVisualStyleBackColor = false;
            btnZoomOut.Click += btnZoomOut_Click;

            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(1010, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(160, 50);
            btnClose.TabIndex = 4;
            btnClose.Text = "✖ Chiudi";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // 
            // _previewControl
            // 
            _previewControl.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            _previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _previewControl.Location = new System.Drawing.Point(0, 60);
            _previewControl.Name = "_previewControl";
            _previewControl.Size = new System.Drawing.Size(1200, 680);
            _previewControl.TabIndex = 2;
            _previewControl.UseAntiAlias = true;
            _previewControl.Zoom = 1.0;

            // 
            // CampaignPDFPreviewForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 820);
            Controls.Add(_previewControl);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            MinimumSize = new System.Drawing.Size(1000, 700);
            Name = "CampaignPDFPreviewForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Anteprima PDF - Programmazione Campagna";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;

            panelTop.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}