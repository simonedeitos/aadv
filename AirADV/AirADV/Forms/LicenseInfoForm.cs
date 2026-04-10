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
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private Panel _contentPanel = null!;
        private Label _serialValueLabel = null!;
        private Label _ownerValueLabel = null!;
        private Label _activatedOnValueLabel = null!;
        private Label _machineIdValueLabel = null!;
        private Button _removeButton = null!;
        private Button _closeButton = null!;

        public LicenseInfoForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadLicenseInfo();
        }

        private void InitializeCustomComponents()
        {
            this.Text = LanguageManager.GetString("LicenseInfo.Title", "License Information");
            this.ClientSize = new Size(540, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(0, 120, 212)
            };

            _titleLabel = new Label
            {
                Text = LanguageManager.GetString("LicenseInfo.Title", "License Information"),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 18)
            };
            _headerPanel.Controls.Add(_titleLabel);

            _contentPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(20, 90),
                Size = new Size(500, 220)
            };

            AddInfoRow(_contentPanel, LanguageManager.GetString("LicenseInfo.Serial", "Serial Key:"), out _serialValueLabel, 10);
            AddInfoRow(_contentPanel, LanguageManager.GetString("LicenseInfo.Owner", "Owner:"), out _ownerValueLabel, 60);
            AddInfoRow(_contentPanel, LanguageManager.GetString("LicenseInfo.ActivatedOn", "Activated On:"), out _activatedOnValueLabel, 110);
            AddInfoRow(_contentPanel, LanguageManager.GetString("LicenseInfo.MachineID", "Machine ID:"), out _machineIdValueLabel, 160);

            _removeButton = new Button
            {
                Text = LanguageManager.GetString("LicenseInfo.Remove", "Remove License"),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 38),
                Location = new Point(20, 330),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            _removeButton.FlatAppearance.BorderSize = 0;
            _removeButton.Click += RemoveButton_Click;

            _closeButton = new Button
            {
                Text = LanguageManager.GetString("LicenseInfo.Close", "Close"),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(230, 230, 235),
                ForeColor = Color.FromArgb(40, 40, 50),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
                Location = new Point(420, 330),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.Click += (s, e) => this.Close();

            this.Controls.Add(_headerPanel);
            this.Controls.Add(_contentPanel);
            this.Controls.Add(_removeButton);
            this.Controls.Add(_closeButton);
        }

        private void AddInfoRow(Panel parent, string labelText, out Label valueLabel, int y)
        {
            var label = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                Location = new Point(16, y)
            };
            valueLabel = new Label
            {
                Text = string.Empty,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(30, 30, 40),
                AutoSize = false,
                Size = new Size(460, 22),
                Location = new Point(16, y + 18)
            };
            parent.Controls.Add(label);
            parent.Controls.Add(valueLabel);
        }

        private void LoadLicenseInfo()
        {
            var license = LicenseManager.GetCurrentLicense();
            if (license == null) return;

            _serialValueLabel.Text = license.SerialKey;
            _ownerValueLabel.Text = license.OwnerName;
            _activatedOnValueLabel.Text = license.ActivatedOn.ToString("dd/MM/yyyy HH:mm");
            _machineIdValueLabel.Text = license.MachineID;
        }

        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            using var confirm = new LicenseRemoveConfirmForm();
            if (confirm.ShowDialog(this) != DialogResult.Yes) return;

            bool ok = LicenseManager.RemoveLicense(out string errorMessage);
            if (ok)
            {
                MessageBox.Show(
                    LanguageManager.GetString("LicenseInfo.RemoveSuccess", "License removed successfully. The application will close."),
                    LanguageManager.GetString("LicenseInfo.Title", "License Information"),
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
