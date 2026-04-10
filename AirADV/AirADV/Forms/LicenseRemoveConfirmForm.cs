using System;
using System.Drawing;
using System.Windows.Forms;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class LicenseRemoveConfirmForm : Form
    {
        private Panel _contentPanel = null!;
        private Label _warningLabel = null!;
        private Label _messageLabel = null!;
        private Button _yesButton = null!;
        private Button _noButton = null!;

        public LicenseRemoveConfirmForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = LanguageManager.GetString("LicenseRemove.Title", "Confirm License Removal");
            this.ClientSize = new Size(440, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            _warningLabel = new Label
            {
                Text = "⚠",
                Font = new Font("Segoe UI", 36),
                ForeColor = Color.FromArgb(255, 165, 0),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            _messageLabel = new Label
            {
                Text = LanguageManager.GetString("LicenseRemove.Message",
                    "Are you sure you want to remove the license?\nThe application will close after removal."),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(40, 40, 50),
                AutoSize = false,
                Size = new Size(330, 80),
                Location = new Point(90, 24)
            };

            _yesButton = new Button
            {
                Text = LanguageManager.GetString("LicenseRemove.Yes", "Yes, Remove"),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 38),
                Location = new Point(160, 158),
                Cursor = Cursors.Hand
            };
            _yesButton.FlatAppearance.BorderSize = 0;
            _yesButton.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };

            _noButton = new Button
            {
                Text = LanguageManager.GetString("LicenseRemove.No", "Cancel"),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(230, 230, 235),
                ForeColor = Color.FromArgb(40, 40, 50),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
                Location = new Point(308, 158),
                Cursor = Cursors.Hand
            };
            _noButton.FlatAppearance.BorderSize = 0;
            _noButton.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };

            this.Controls.Add(_warningLabel);
            this.Controls.Add(_messageLabel);
            this.Controls.Add(_yesButton);
            this.Controls.Add(_noButton);

            this.CancelButton = _noButton;
        }
    }
}
