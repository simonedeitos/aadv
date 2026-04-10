using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class SlotTimesEditorForm : Form
    {
        private List<string> _slotTimes = new List<string>();

        /// <summary>
        /// Gli orari risultanti dopo la conferma (OK)
        /// </summary>
        public List<string> SlotTimes => _slotTimes;

        // Controlli
        private DataGridView dgvTimes;
        private DateTimePicker dtpTime;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnOK;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblCount;
        private Panel panelTop;
        private Panel panelBottom;

        public SlotTimesEditorForm(List<string> existingTimes)
        {
            InitializeComponents();

            if (existingTimes != null)
            {
                _slotTimes = new List<string>(existingTimes);
            }

            this.Load += (s, e) =>
            {
                RefreshGrid();
                ApplyLanguage();
            };
        }

        private void InitializeComponents()
        {
            this.Text = "🕐 Definisci Orari";
            this.ClientSize = new Size(380, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // ═══════════════════════════════════════════════════════════
            // PANNELLO SUPERIORE: Aggiunta orario
            // ═══════════════════════════════════════════════════════════

            panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(240, 240, 245),
                Padding = new Padding(15)
            };

            lblTitle = new Label
            {
                Text = "🕐 Aggiungi orario:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(15, 12),
                AutoSize = true
            };

            dtpTime = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 12F),
                Location = new Point(15, 42),
                Size = new Size(100, 30),
                Value = DateTime.Today.AddHours(8) // Default 08:00
            };

            btnAdd = new Button
            {
                Text = "➕ Aggiungi",
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(130, 40),
                Size = new Size(120, 35),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Elimina",
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(260, 40),
                Size = new Size(100, 35),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(dtpTime);
            panelTop.Controls.Add(btnAdd);
            panelTop.Controls.Add(btnDelete);

            // ═══════════════════════════════════════════════════════════
            // GRIGLIA ORARI (centro)
            // ═══════════════════════════════════════════════════════════

            dgvTimes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11F),
                ReadOnly = false
            };
            dgvTimes.RowTemplate.Height = 32;

            dgvTimes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "Orario (HH:mm)",
                FillWeight = 70
            });

            dgvTimes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrder",
                HeaderText = "#",
                FillWeight = 15,
                ReadOnly = true
            });

            dgvTimes.CellEndEdit += DgvTimes_CellEndEdit;

            // ═══════════════════════════════════════════════════════════
            // PANNELLO INFERIORE: Bottoni Sposta + OK/Annulla
            // ═══════════════════════════════════════════════════════════

            panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 245)
            };

            btnMoveUp = new Button
            {
                Text = "🔼",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(15, 12),
                Size = new Size(40, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMoveUp.FlatAppearance.BorderColor = Color.LightGray;
            btnMoveUp.Click += BtnMoveUp_Click;

            btnMoveDown = new Button
            {
                Text = "🔽",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(60, 12),
                Size = new Size(40, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMoveDown.FlatAppearance.BorderColor = Color.LightGray;
            btnMoveDown.Click += BtnMoveDown_Click;

            lblCount = new Label
            {
                Text = "Orari: 0",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(115, 20),
                AutoSize = true
            };

            btnOK = new Button
            {
                Text = "✓ OK",
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(220, 12),
                Size = new Size(70, 35),
                DialogResult = DialogResult.OK,
                Cursor = Cursors.Hand
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "✖ Annulla",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(295, 12),
                Size = new Size(70, 35),
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            panelBottom.Controls.Add(btnMoveUp);
            panelBottom.Controls.Add(btnMoveDown);
            panelBottom.Controls.Add(lblCount);
            panelBottom.Controls.Add(btnOK);
            panelBottom.Controls.Add(btnCancel);

            // ═══════════════════════════════════════════════════════════
            // Aggiunta al form
            // ═══════════════════════════════════════════════════════════

            this.Controls.Add(dgvTimes);
            this.Controls.Add(panelBottom);
            this.Controls.Add(panelTop);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        // ═══════════════════════════════════════════════════════════
        // EVENTI
        // ═══════════════════════════════════════════════════════════

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string newTime = dtpTime.Value.ToString("HH:mm");

            // Controlla duplicati
            if (_slotTimes.Contains(newTime))
            {
                MessageBox.Show(
                    $"L'orario {newTime} è già presente!",
                    "Duplicato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _slotTimes.Add(newTime);
            SortTimes();
            RefreshGrid();

            // Seleziona l'orario appena aggiunto
            SelectTimeInGrid(newTime);

            // Avanza di 30 minuti per il prossimo inserimento rapido
            dtpTime.Value = dtpTime.Value.AddMinutes(30);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTimes.SelectedRows.Count == 0) return;

            int idx = dgvTimes.SelectedRows[0].Index;
            if (idx >= 0 && idx < _slotTimes.Count)
            {
                _slotTimes.RemoveAt(idx);
                RefreshGrid();
            }
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvTimes.SelectedRows.Count == 0) return;
            int idx = dgvTimes.SelectedRows[0].Index;
            if (idx <= 0) return;

            // Scambia
            string temp = _slotTimes[idx];
            _slotTimes[idx] = _slotTimes[idx - 1];
            _slotTimes[idx - 1] = temp;

            RefreshGrid();
            dgvTimes.ClearSelection();
            dgvTimes.Rows[idx - 1].Selected = true;
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvTimes.SelectedRows.Count == 0) return;
            int idx = dgvTimes.SelectedRows[0].Index;
            if (idx >= _slotTimes.Count - 1) return;

            // Scambia
            string temp = _slotTimes[idx];
            _slotTimes[idx] = _slotTimes[idx + 1];
            _slotTimes[idx + 1] = temp;

            RefreshGrid();
            dgvTimes.ClearSelection();
            dgvTimes.Rows[idx + 1].Selected = true;
        }

        /// <summary>
        /// Modifica inline: quando l'utente modifica un orario nella cella
        /// </summary>
        private void DgvTimes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != dgvTimes.Columns["colTime"].Index) return;
            if (e.RowIndex < 0 || e.RowIndex >= _slotTimes.Count) return;

            string newValue = dgvTimes.Rows[e.RowIndex].Cells["colTime"].Value?.ToString()?.Trim() ?? "";

            // Valida formato HH:mm
            if (TimeSpan.TryParse(newValue, out TimeSpan ts))
            {
                string formatted = ts.ToString(@"hh\:mm");

                // Controlla duplicati (escluso se stesso)
                for (int i = 0; i < _slotTimes.Count; i++)
                {
                    if (i != e.RowIndex && _slotTimes[i] == formatted)
                    {
                        MessageBox.Show($"L'orario {formatted} è già presente!",
                            "Duplicato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Ripristina valore precedente
                        dgvTimes.Rows[e.RowIndex].Cells["colTime"].Value = _slotTimes[e.RowIndex];
                        return;
                    }
                }

                _slotTimes[e.RowIndex] = formatted;
                dgvTimes.Rows[e.RowIndex].Cells["colTime"].Value = formatted;
                UpdateCount();
            }
            else
            {
                MessageBox.Show("Formato orario non valido!\nUsa HH:mm (es: 08:30, 14:00)",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Ripristina
                dgvTimes.Rows[e.RowIndex].Cells["colTime"].Value = _slotTimes[e.RowIndex];
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // Riordina prima di restituire
            SortTimes();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ═══════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════��═══════

        private void SortTimes()
        {
            _slotTimes = _slotTimes
                .OrderBy(t =>
                {
                    TimeSpan.TryParse(t, out TimeSpan ts);
                    return ts;
                })
                .ToList();
        }

        private void RefreshGrid()
        {
            dgvTimes.Rows.Clear();

            for (int i = 0; i < _slotTimes.Count; i++)
            {
                int idx = dgvTimes.Rows.Add();
                dgvTimes.Rows[idx].Cells["colTime"].Value = _slotTimes[i];
                dgvTimes.Rows[idx].Cells["colOrder"].Value = (i + 1).ToString();
            }

            UpdateCount();
        }

        private void UpdateCount()
        {
            lblCount.Text = $"Orari: {_slotTimes.Count}";
        }

        private void SelectTimeInGrid(string time)
        {
            foreach (DataGridViewRow row in dgvTimes.Rows)
            {
                if (row.Cells["colTime"].Value?.ToString() == time)
                {
                    dgvTimes.ClearSelection();
                    row.Selected = true;
                    dgvTimes.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.Get("SlotTimesEditor.Title", "🕐 Definisci Orari");
            lblTitle.Text = LanguageManager.Get("SlotTimesEditor.LblTitle", "🕐 Aggiungi orario:");
            btnAdd.Text = LanguageManager.Get("SlotTimesEditor.BtnAdd", "➕ Aggiungi");
            btnDelete.Text = LanguageManager.Get("SlotTimesEditor.BtnDelete", "🗑️ Elimina");
            btnOK.Text = LanguageManager.Get("Common.OK", "✓ OK");
            btnCancel.Text = LanguageManager.Get("Common.Cancel", "✖ Annulla");

            if (dgvTimes.Columns.Count > 0)
            {
                dgvTimes.Columns["colTime"].HeaderText = LanguageManager.Get("SlotTimesEditor.ColTime", "Orario (HH:mm)");
                dgvTimes.Columns["colOrder"].HeaderText = "#";
            }
        }
    }
}