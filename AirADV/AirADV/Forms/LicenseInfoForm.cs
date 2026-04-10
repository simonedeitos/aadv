using System;
using System.Drawing;
using System.Windows.Forms;
using AirADV.Models;
using AirADV.Services.Localization;
using AirADV.Services.Licensing;

namespace AirADV.Forms
{
    public partial class LicenseInfoForm : Form
    {
        public bool LicenseRemoved { get; private set; } = false;

        public LicenseInfoForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = LanguageManager.GetString("LicenseInfo.Title", "License Management");
            this.Size = new Size(520, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30);

            // ── Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            var lblHeaderIcon = new Label
            {
                Text = "🔑",
                Font = new Font("Segoe UI Emoji", 18F),
                ForeColor = Color.White,
                Location = new Point(16, 12),
                Size = new Size(40, 36),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblHeaderIcon);

            var lblHeaderTitle = new Label
            {
                Text = LanguageManager.GetString("LicenseInfo.Header", "License Management"),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(64, 16),
                Size = new Size(420, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblHeaderTitle);
            this.Controls.Add(pnlHeader);

            // ── Active badge
            var lblBadge = new Label
            {
                Text = "✅ " + LanguageManager.GetString("LicenseInfo.Active", "LICENSE ACTIVE"),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 140, 60),
                Location = new Point(20, 76),
                Size = new Size(460, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblBadge);

            // ── License details card
            var pnlCard = new Panel
            {
                Location = new Point(20, 120),
                Size = new Size(460, 180),
                BackColor = Color.FromArgb(40, 40, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            var license = LicenseManager.GetCurrentLicense();
            string owner = license?.OwnerName ?? "-";
            string serial = license?.SerialKey ?? "-";
            string activatedOn = license?.ActivatedOn.ToString("dd/MM/yyyy HH:mm") ?? "-";
            string machineId = license?.MachineID ?? "-";

            AddCardRow(pnlCard, "👤 " + LanguageManager.GetString("LicenseInfo.Owner", "Owner"), owner, 12);
            AddCardRow(pnlCard, "🔑 " + LanguageManager.GetString("LicenseInfo.Serial", "Serial Code"), serial, 52);
            AddCardRow(pnlCard, "📅 " + LanguageManager.GetString("LicenseInfo.ActivatedOn", "Activated On"), activatedOn, 92);
            AddCardRow(pnlCard, "🖥️ " + LanguageManager.GetString("LicenseInfo.MachineId", "Machine ID"), machineId, 132);

            this.Controls.Add(pnlCard);

            // ── Remove button
            var btnRemove = new Button
            {
                Text = "🗑 " + LanguageManager.GetString("LicenseInfo.BtnRemove", "Remove License"),
                Location = new Point(20, 320),
                Size = new Size(200, 42),
                BackColor = Color.FromArgb(180, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += BtnRemove_Click;
            this.Controls.Add(btnRemove);

            // ── Close button
            var btnClose = new Button
            {
                Text = LanguageManager.GetString("Common.Close", "Close"),
                Location = new Point(360, 320),
                Size = new Size(120, 42),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnClose);

            this.CancelButton = btnClose;
        }

        private void AddCardRow(Panel panel, string label, string value, int top)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 160),
                Location = new Point(12, top),
                Size = new Size(160, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblLabel);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                Location = new Point(180, top),
                Size = new Size(268, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblValue);
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            using var confirm = new LicenseRemoveConfirmForm();
            if (confirm.ShowDialog(this) != DialogResult.Yes) return;

            bool ok = LicenseManager.RemoveLicense(out string errorMessage);
            if (ok)
            {
                LicenseRemoved = true;
                MessageBox.Show(
                    LanguageManager.GetString("LicenseInfo.RemoveSuccess", "License removed successfully. The application will close."),
                    LanguageManager.GetString("LicenseInfo.Title", "License Management"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show(errorMessage,
                    LanguageManager.GetString("LicenseInfo.RemoveError", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
