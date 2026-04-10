using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AirADV.Services.Localization;
using AirADV.Services.Licensing;

namespace AirADV.Forms
{
    public partial class LicenseForm : Form
    {
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private Label _subtitleLabel = null!;
        private Panel _contentPanel = null!;
        private Label _serialLabel = null!;
        private TextBox _serialTextBox = null!;
        private Label _ownerLabel = null!;
        private TextBox _ownerTextBox = null!;
        private Label _infoLabel = null!;
        private Button _activateButton = null!;
        private Button _cancelButton = null!;
        private Label _statusLabel = null!;

        public LicenseForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = LanguageManager.GetString("License.Title", "License Activation");
            this.ClientSize = new Size(620, 480);
            this.MinimumSize = new Size(640, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Header panel
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 120, 212)
            };

            _titleLabel = new Label
            {
                Text = "AirADV",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(24, 12)
            };

            _subtitleLabel = new Label
            {
                Text = LanguageManager.GetString("License.Subtitle", "License Activation"),
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 230, 255),
                AutoSize = true,
                Location = new Point(26, 46)
            };

            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Controls.Add(_subtitleLabel);

            // Content panel
            _contentPanel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(20, 100),
                Size = new Size(580, 290),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            _serialLabel = new Label
            {
                Text = LanguageManager.GetString("License.SerialKey", "Serial Key:"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 50),
                AutoSize = true,
                Location = new Point(20, 24)
            };

            _serialTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(20, 50),
                Size = new Size(540, 30),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "AAD-XXXX-XXXX-XXXX",
                CharacterCasing = CharacterCasing.Upper
            };

            _ownerLabel = new Label
            {
                Text = LanguageManager.GetString("License.OwnerName", "Owner Name (optional):"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 50),
                AutoSize = true,
                Location = new Point(20, 100)
            };

            _ownerTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(20, 126),
                Size = new Size(540, 30),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = LanguageManager.GetString("License.OwnerPlaceholder", "Your name or company")
            };

            _infoLabel = new Label
            {
                Text = LanguageManager.GetString("License.InfoText",
                    "To purchase a license for AirADV, visit store.airdirector.app"),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(20, 180),
                Size = new Size(540, 40),
                AutoSize = false
            };

            _statusLabel = new Label
            {
                Text = string.Empty,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Red,
                Location = new Point(20, 230),
                Size = new Size(540, 40),
                AutoSize = false
            };

            _contentPanel.Controls.Add(_serialLabel);
            _contentPanel.Controls.Add(_serialTextBox);
            _contentPanel.Controls.Add(_ownerLabel);
            _contentPanel.Controls.Add(_ownerTextBox);
            _contentPanel.Controls.Add(_infoLabel);
            _contentPanel.Controls.Add(_statusLabel);

            // Buttons
            _activateButton = new Button
            {
                Text = LanguageManager.GetString("License.Activate", "Activate"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 40),
                Location = new Point(460, 410),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            _activateButton.FlatAppearance.BorderSize = 0;
            _activateButton.Click += ActivateButton_Click;

            _cancelButton = new Button
            {
                Text = LanguageManager.GetString("License.Cancel", "Cancel"),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(230, 230, 235),
                ForeColor = Color.FromArgb(40, 40, 50),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 40),
                Location = new Point(320, 410),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            _cancelButton.FlatAppearance.BorderSize = 0;
            _cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(_headerPanel);
            this.Controls.Add(_contentPanel);
            this.Controls.Add(_activateButton);
            this.Controls.Add(_cancelButton);

            this.AcceptButton = _activateButton;
            this.CancelButton = _cancelButton;
        }

        private void ActivateButton_Click(object? sender, EventArgs e)
        {
            string serial = _serialTextBox.Text.Trim();
            string owner = _ownerTextBox.Text.Trim();

            _statusLabel.ForeColor = Color.Red;
            _statusLabel.Text = string.Empty;
            _activateButton.Enabled = false;

            try
            {
                bool ok = LicenseManager.ActivateLicense(serial, owner, out string errorMessage);
                if (ok)
                {
                    _statusLabel.ForeColor = Color.FromArgb(40, 167, 69);
                    _statusLabel.Text = LanguageManager.GetString("License.ActivationSuccess", "License activated successfully!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    _statusLabel.Text = errorMessage;
                    _activateButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _statusLabel.Text = ex.Message;
                _activateButton.Enabled = true;
            }
        }
    }
}
