using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
	public partial class TimeSlotsForm : Form
	{
		private int _stationID = 0;
		private List<DbcManager.TimeSlot> _timeSlots = new List<DbcManager.TimeSlot>();
		private bool _isDirty = false;

		public TimeSlotsForm(int stationID)
		{
			InitializeComponent();
			_stationID = stationID;
			this.Load += TimeSlotsForm_Load;
		}

		private void TimeSlotsForm_Load(object sender, EventArgs e)
		{
			try
			{
				SetupDataGridView();
				ApplyLanguage();
				LoadTimeSlots();

				LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("Messages.InitializationError", "Errore inizializzazione")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void LanguageManager_LanguageChanged(object sender, EventArgs e)
		{
			Console.WriteLine("[TimeSlotsForm] 🔄 Cambio lingua rilevato");
			ApplyLanguage();
		}

		/// <summary>
		/// ✅ AGGIORNATO: DataGridView con colonne editabili per i file
		/// </summary>
		private void SetupDataGridView()
		{
			dgvTimeSlots.AutoGenerateColumns = false;
			dgvTimeSlots.AllowUserToAddRows = false;
			dgvTimeSlots.AllowUserToDeleteRows = false;
			dgvTimeSlots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvTimeSlots.MultiSelect = true;
			dgvTimeSlots.RowHeadersVisible = false;
			dgvTimeSlots.BackgroundColor = Color.White;
			dgvTimeSlots.BorderStyle = BorderStyle.Fixed3D;
			dgvTimeSlots.RowTemplate.Height = 28;

			// ✅ Checkbox selezione
			var colSelect = new DataGridViewCheckBoxColumn
			{
				Name = "colSelect",
				HeaderText = "☑",
				ReadOnly = false
			};
			dgvTimeSlots.Columns.Add(colSelect);

			// ✅ Orario (readonly)
			var colTime = new DataGridViewTextBoxColumn
			{
				Name = "colTime",
				HeaderText = "Orario",
				DataPropertyName = "SlotTime",
				ReadOnly = true
			};
			dgvTimeSlots.Columns.Add(colTime);

			// ✅ Durata Max (readonly)
			var colMaxDuration = new DataGridViewTextBoxColumn
			{
				Name = "colMaxDuration",
				HeaderText = "Max (s)",
				DataPropertyName = "MaxDuration",
				ReadOnly = true
			};
			dgvTimeSlots.Columns.Add(colMaxDuration);

			// ✅ Priorità (readonly)
			var colPriority = new DataGridViewTextBoxColumn
			{
				Name = "colPriority",
				HeaderText = "P",
				DataPropertyName = "Priority",
				ReadOnly = true
			};
			dgvTimeSlots.Columns.Add(colPriority);

			// ✅ Opening (editabile)
			var colOpening = new DataGridViewTextBoxColumn
			{
				Name = "colOpening",
				HeaderText = "Opening",
				DataPropertyName = "OpeningFile",
				ReadOnly = false
			};
			dgvTimeSlots.Columns.Add(colOpening);

			// ✅ InfraSpot (editabile)
			var colInfraSpot = new DataGridViewTextBoxColumn
			{
				Name = "colInfraSpot",
				HeaderText = "InfraSpot",
				DataPropertyName = "InfraSpotFile",
				ReadOnly = false
			};
			dgvTimeSlots.Columns.Add(colInfraSpot);

			// ✅ Closing (editabile)
			var colClosing = new DataGridViewTextBoxColumn
			{
				Name = "colClosing",
				HeaderText = "Closing",
				DataPropertyName = "ClosingFile",
				ReadOnly = false
			};
			dgvTimeSlots.Columns.Add(colClosing);

			// ✅ Attivo
			var colActive = new DataGridViewCheckBoxColumn
			{
				Name = "colActive",
				HeaderText = "Attivo",
				DataPropertyName = "IsActive"
			};
			dgvTimeSlots.Columns.Add(colActive);

			// ✅ NUOVO: Configura auto-sizing
			ConfigureColumnAutoSize();

			// ✅ Eventi modifica
			dgvTimeSlots.CellValueChanged += DgvTimeSlots_CellValueChanged;
			dgvTimeSlots.CurrentCellDirtyStateChanged += DgvTimeSlots_CurrentCellDirtyStateChanged;
		}

		/// <summary>
		/// ✅ NUOVO: Configura colonne auto-adattabili
		/// </summary>
		private void ConfigureColumnAutoSize()
		{
			// ✅ Colonne fisse (non si espandono)
			dgvTimeSlots.Columns["colSelect"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dgvTimeSlots.Columns["colSelect"].Width = 40;

			dgvTimeSlots.Columns["colTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dgvTimeSlots.Columns["colTime"].Width = 80;

			dgvTimeSlots.Columns["colMaxDuration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dgvTimeSlots.Columns["colMaxDuration"].Width = 70;

			dgvTimeSlots.Columns["colPriority"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dgvTimeSlots.Columns["colPriority"].Width = 40;

			dgvTimeSlots.Columns["colActive"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dgvTimeSlots.Columns["colActive"].Width = 60;

			// ✅ Colonne file - auto espandibili (riempiono spazio rimanente)
			dgvTimeSlots.Columns["colOpening"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgvTimeSlots.Columns["colOpening"].MinimumWidth = 200;
			dgvTimeSlots.Columns["colOpening"].FillWeight = 33;

			dgvTimeSlots.Columns["colInfraSpot"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgvTimeSlots.Columns["colInfraSpot"].MinimumWidth = 200;
			dgvTimeSlots.Columns["colInfraSpot"].FillWeight = 33;

			dgvTimeSlots.Columns["colClosing"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgvTimeSlots.Columns["colClosing"].MinimumWidth = 200;
			dgvTimeSlots.Columns["colClosing"].FillWeight = 34;
		}

		private void DgvTimeSlots_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (dgvTimeSlots.IsCurrentCellDirty)
			{
				dgvTimeSlots.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}

		private void DgvTimeSlots_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			var row = dgvTimeSlots.Rows[e.RowIndex];
			var slot = row.Tag as DbcManager.TimeSlot;

			if (slot != null)
			{
				// ✅ Aggiorna oggetto TimeSlot in base alla colonna modificata
				string columnName = dgvTimeSlots.Columns[e.ColumnIndex].Name;

				switch (columnName)
				{
					case "colOpening":
						slot.OpeningFile = row.Cells["colOpening"].Value?.ToString() ?? "";
						_isDirty = true;
						Console.WriteLine($"[TimeSlotsForm] Opening modificato: {slot.SlotTime} -> {slot.OpeningFile}");
						break;

					case "colInfraSpot":
						slot.InfraSpotFile = row.Cells["colInfraSpot"].Value?.ToString() ?? "";
						_isDirty = true;
						Console.WriteLine($"[TimeSlotsForm] InfraSpot modificato:  {slot.SlotTime} -> {slot.InfraSpotFile}");
						break;

					case "colClosing":
						slot.ClosingFile = row.Cells["colClosing"].Value?.ToString() ?? "";
						_isDirty = true;
						Console.WriteLine($"[TimeSlotsForm] Closing modificato:  {slot.SlotTime} -> {slot.ClosingFile}");
						break;

					case "colActive":
						slot.IsActive = Convert.ToBoolean(row.Cells["colActive"].Value);
						_isDirty = true;
						break;
				}
			}
		}

		private void LoadTimeSlots()
		{
			try
			{
				_timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
					.Where(t => t.StationID == _stationID)
					.OrderBy(t => t.SlotTime)
					.ToList();

				RefreshGrid();

				_isDirty = false;
				UpdateStatusLabel();

				Console.WriteLine($"[TimeSlotsForm] Caricati {_timeSlots.Count} punti orari");
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("Messages.LoadError", "Errore caricamento")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void RefreshGrid()
		{
			dgvTimeSlots.Rows.Clear();

			foreach (var slot in _timeSlots)
			{
				int rowIndex = dgvTimeSlots.Rows.Add();
				var row = dgvTimeSlots.Rows[rowIndex];

				row.Cells["colSelect"].Value = false;
				row.Cells["colTime"].Value = slot.SlotTime;
				row.Cells["colMaxDuration"].Value = slot.MaxDuration;
				row.Cells["colPriority"].Value = slot.Priority;

				// ✅ Mostra percorso completo (editabile)
				row.Cells["colOpening"].Value = slot.OpeningFile;
				row.Cells["colInfraSpot"].Value = slot.InfraSpotFile;
				row.Cells["colClosing"].Value = slot.ClosingFile;

				row.Cells["colActive"].Value = slot.IsActive;
				row.Tag = slot;
			}
		}

		private void UpdateStatusLabel()
		{
			int activeSlots = _timeSlots.Count(t => t.IsActive);
			lblStatus.Text = string.Format(
				LanguageManager.Get("TimeSlots.StatusLabel", "Punti orari:  {0} | Attivi: {1}"),
				_timeSlots.Count,
				activeSlots
			);
		}

		/// <summary>
		/// ✅ COMPLETO: Tutte le traduzioni
		/// </summary>
		private void ApplyLanguage()
		{
			try
			{
				Console.WriteLine($"[TimeSlotsForm] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

				// ✅ Titolo
				this.Text = LanguageManager.Get("TimeSlots.WindowTitle", "🕐 Gestione Punti Orari");
				lblTitle.Text = LanguageManager.Get("TimeSlots.WindowTitle", "🕐 Gestione Punti Orari");

				// ✅ STEP 1: Inserimento Orari
				grpAddSlots.Text = LanguageManager.Get("TimeSlots.GrpAddSlots", "📝 STEP 1: Inserimento Orari");
				lblStartTime.Text = LanguageManager.Get("TimeSlots.LblStartTime", "Orario iniziale:");
				chkAutoIncrement.Text = LanguageManager.Get("TimeSlots.ChkAutoIncrement", "☑ Incremento automatico");
				lblIncrementHours.Text = LanguageManager.Get("TimeSlots.LblIncrementHours", "Incrementa di");
				lblHours.Text = LanguageManager.Get("TimeSlots.LblHours", "ora/e fino alle:");
				btnAddSlots.Text = LanguageManager.Get("TimeSlots.BtnAddSlots", "➕ Aggiungi Orario/i");

				// ✅ STEP 2: Configurazione durante creazione
				grpConfigurations.Text = LanguageManager.Get("TimeSlots.GrpConfigurations", "🔧 STEP 2: Configurazione Nuovi Slot");
				lblConfigMaxDuration.Text = LanguageManager.Get("TimeSlots.LblConfigMaxDuration", "Durata Max (s):");
				lblConfigPriority.Text = LanguageManager.Get("TimeSlots.LblConfigPriority", "Priorità:");
				lblConfigOpening.Text = LanguageManager.Get("TimeSlots.LblConfigOpening", "File Opening:");
				lblConfigInfraSpot.Text = LanguageManager.Get("TimeSlots.LblConfigInfraSpot", "File InfraSpot:");
				lblConfigClosing.Text = LanguageManager.Get("TimeSlots.LblConfigClosing", "File Closing:");

				// ✅ Bottoni
				btnDeleteSlots.Text = LanguageManager.Get("TimeSlots.BtnDeleteSlots", "🗑️ Elimina Selezionati");
				btnSave.Text = LanguageManager.Get("Common.Save", "💾 Salva");
				btnRefresh.Text = LanguageManager.Get("Common.Refresh", "🔄 Aggiorna");
				btnSelectAll.Text = LanguageManager.Get("TimeSlots.BtnSelectAll", "☑ Seleziona Tutti");
				btnDeselectAll.Text = LanguageManager.Get("TimeSlots.BtnDeselectAll", "☐ Deseleziona Tutti");
				btnApplyToSelected.Text = LanguageManager.Get("TimeSlots.BtnApply", "✓ Applica ai Selezionati");

				// ✅ Colonne DataGridView
				dgvTimeSlots.Columns["colTime"].HeaderText = LanguageManager.Get("TimeSlots.ColTime", "Orario");
				dgvTimeSlots.Columns["colMaxDuration"].HeaderText = LanguageManager.Get("TimeSlots.ColMaxDuration", "Max (s)");
				dgvTimeSlots.Columns["colPriority"].HeaderText = LanguageManager.Get("TimeSlots.ColPriority", "P");
				dgvTimeSlots.Columns["colOpening"].HeaderText = LanguageManager.Get("TimeSlots.ColOpening", "Opening");
				dgvTimeSlots.Columns["colInfraSpot"].HeaderText = LanguageManager.Get("TimeSlots.ColInfraSpot", "InfraSpot");
				dgvTimeSlots.Columns["colClosing"].HeaderText = LanguageManager.Get("TimeSlots.ColClosing", "Closing");
				dgvTimeSlots.Columns["colActive"].HeaderText = LanguageManager.Get("TimeSlots.ColActive", "Attivo");

				// ✅ Refresh
				this.Refresh();
				UpdateStatusLabel();

				Console.WriteLine("[TimeSlotsForm] ✅ Traduzioni applicate");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[TimeSlotsForm] ❌ Errore ApplyLanguage: {ex.Message}");
			}
		}

		// ═══════════════════════════════════════════════════════════
		// STEP 1: INSERIMENTO ORARI
		// ═══════════════════════════════════════════════════════════

		private void chkAutoIncrement_CheckedChanged(object sender, EventArgs e)
		{
			bool enabled = chkAutoIncrement.Checked;
			numIncrementHours.Enabled = enabled;
			dtpEndTime.Enabled = enabled;
			lblIncrementHours.Enabled = enabled;
			lblHours.Enabled = enabled;
		}

		private void btnAddSlots_Click(object sender, EventArgs e)
		{
			try
			{
				if (!chkAutoIncrement.Checked)
				{
					AddSingleSlot(dtpStartTime.Value);
				}
				else
				{
					AddMultipleSlotsWithIncrement();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("TimeSlots.AddError", "Errore aggiunta orari")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void AddSingleSlot(DateTime time)
		{
			string slotTime = $"{time.Hour:D2}:{time.Minute:D2}: 00";

			if (_timeSlots.Any(t => t.SlotTime == slotTime))
			{
				MessageBox.Show(
					string.Format(
						LanguageManager.Get("TimeSlots.SlotAlreadyExists", "L'orario {0} è già presente!"),
						slotTime
					),
					LanguageManager.Get("Common.Warning", "Attenzione"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			// ✅ Usa configurazione STEP 2 per nuovi slot
			var newSlot = new DbcManager.TimeSlot
			{
				StationID = _stationID,
				SlotTime = slotTime,
				MaxDuration = (int)numConfigMaxDuration.Value,
				Priority = (int)numConfigPriority.Value,
				OpeningFile = txtConfigOpening.Text.Trim(),
				InfraSpotFile = txtConfigInfraSpot.Text.Trim(),
				ClosingFile = txtConfigClosing.Text.Trim(),
				IsActive = true
			};

			_timeSlots.Add(newSlot);
			_timeSlots = _timeSlots.OrderBy(t => t.SlotTime).ToList();

			RefreshGrid();
			_isDirty = true;
			UpdateStatusLabel();

			Console.WriteLine($"[TimeSlotsForm] Aggiunto orario: {slotTime}");
		}

		private void AddMultipleSlotsWithIncrement()
		{
			DateTime startTime = dtpStartTime.Value;
			DateTime endTime = dtpEndTime.Value;
			int incrementHours = (int)numIncrementHours.Value;

			if (incrementHours <= 0)
			{
				MessageBox.Show(
					LanguageManager.Get("TimeSlots.InvalidIncrement", "L'incremento deve essere maggiore di 0!"),
					LanguageManager.Get("Common.Warning", "Attenzione"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			DateTime current = new DateTime(2000, 1, 1, startTime.Hour, startTime.Minute, 0);
			DateTime end = new DateTime(2000, 1, 1, endTime.Hour, endTime.Minute, 0);

			if (end <= current)
			{
				end = end.AddDays(1);
			}

			int addedCount = 0;
			int skippedCount = 0;

			// ✅ Leggi configurazione STEP 2
			int maxDuration = (int)numConfigMaxDuration.Value;
			int priority = (int)numConfigPriority.Value;
			string opening = txtConfigOpening.Text.Trim();
			string infraSpot = txtConfigInfraSpot.Text.Trim();
			string closing = txtConfigClosing.Text.Trim();

			while (current <= end)
			{
				string slotTime = $"{current.Hour:D2}:{current.Minute:D2}:00";

				if (!_timeSlots.Any(t => t.SlotTime == slotTime))
				{
					var newSlot = new DbcManager.TimeSlot
					{
						StationID = _stationID,
						SlotTime = slotTime,
						MaxDuration = maxDuration,
						Priority = priority,
						OpeningFile = opening,
						InfraSpotFile = infraSpot,
						ClosingFile = closing,
						IsActive = true
					};

					_timeSlots.Add(newSlot);
					addedCount++;
				}
				else
				{
					skippedCount++;
				}

				current = current.AddHours(incrementHours);

				if (addedCount + skippedCount > 100)
					break;
			}

			_timeSlots = _timeSlots.OrderBy(t => t.SlotTime).ToList();

			RefreshGrid();
			_isDirty = true;
			UpdateStatusLabel();

			string message = string.Format(
				LanguageManager.Get("TimeSlots.SlotsAdded", "✅ Aggiunti {0} orari"),
				addedCount
			);

			if (skippedCount > 0)
			{
				message += "\n" + string.Format(
					LanguageManager.Get("TimeSlots.SlotsSkipped", "⚠️ Saltati {0} orari già esistenti"),
					skippedCount
				);
			}

			MessageBox.Show(
				message,
				LanguageManager.Get("Common.Success", "Completato"),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);

			Console.WriteLine($"[TimeSlotsForm] Aggiunti {addedCount} orari con incremento di {incrementHours}h");
		}

		// ═══════════════════════════════════════════════════════════
		// STEP 2: BROWSE FILE
		// ═══════════════════════════════════════════════════════════

		private void btnBrowseOpening_Click(object sender, EventArgs e)
		{
			BrowseFile(txtConfigOpening, "Opening");
		}

		private void btnBrowseInfraSpot_Click(object sender, EventArgs e)
		{
			BrowseFile(txtConfigInfraSpot, "InfraSpot");
		}

		private void btnBrowseClosing_Click(object sender, EventArgs e)
		{
			BrowseFile(txtConfigClosing, "Closing");
		}

		private void BrowseFile(TextBox textBox, string fileType)
		{
			using (var dialog = new OpenFileDialog())
			{
				dialog.Title = string.Format(
					LanguageManager.Get("TimeSlots.SelectFile", "Seleziona file {0}"),
					fileType
				);
				dialog.Filter = LanguageManager.Get("TimeSlots.AudioFilter", "Audio/Video Files|*.mp3;*.wav;*.flac;*.m4a;*.wma;*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.ts;*.mts;*.m2ts;*.webm|Audio Files|*.mp3;*.wav;*.flac;*.m4a;*.wma|Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.ts;*.mts;*.m2ts;*.webm|All Files|*.*");

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					textBox.Text = dialog.FileName;
				}
			}
		}

		// ═══════════════════════════════════════════════════════════
		// AZIONI SELEZIONE
		// ═══════════════════════════════════════════════════════════

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvTimeSlots.Rows)
			{
				row.Cells["colSelect"].Value = true;
			}
		}

		private void btnDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvTimeSlots.Rows)
			{
				row.Cells["colSelect"].Value = false;
			}
		}

		private void btnApplyToSelected_Click(object sender, EventArgs e)
		{
			try
			{
				var selectedSlots = new List<DbcManager.TimeSlot>();

				foreach (DataGridViewRow row in dgvTimeSlots.Rows)
				{
					if (Convert.ToBoolean(row.Cells["colSelect"].Value))
					{
						var slot = row.Tag as DbcManager.TimeSlot;
						if (slot != null)
							selectedSlots.Add(slot);
					}
				}

				if (selectedSlots.Count == 0)
				{
					MessageBox.Show(
						LanguageManager.Get("Common.Warning", "Attenzione"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return;
				}

				// Applica i valori della configurazione a tutti gli slot selezionati
				string opening = txtConfigOpening.Text.Trim();
				string infraSpot = txtConfigInfraSpot.Text.Trim();
				string closing = txtConfigClosing.Text.Trim();
				int maxDuration = (int)numConfigMaxDuration.Value;
				int priority = (int)numConfigPriority.Value;

				foreach (var slot in selectedSlots)
				{
						slot.OpeningFile = opening;
						slot.InfraSpotFile = infraSpot;
						slot.ClosingFile = closing;
					slot.MaxDuration = maxDuration;
					slot.Priority = priority;
				}

				RefreshGrid();
				_isDirty = true;

				MessageBox.Show(
					string.Format(
						selectedSlots.Count
					),
					LanguageManager.Get("Common.Success", "Successo"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("TimeSlots.ApplyError", "Errore applicazione configurazione")}:
{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void btnDeleteSlots_Click(object sender, EventArgs e)
		{
			try
			{
				var toDelete = new List<DbcManager.TimeSlot>();

				foreach (DataGridViewRow row in dgvTimeSlots.Rows)
				{
					if (Convert.ToBoolean(row.Cells["colSelect"].Value))
					{
						var slot = row.Tag as DbcManager.TimeSlot;
						if (slot != null)
							toDelete.Add(slot);
					}
				}

				if (toDelete.Count == 0)
				{
					MessageBox.Show(
						LanguageManager.Get("TimeSlots.NoSlotsSelected", "Nessun punto orario selezionato!"),
						LanguageManager.Get("Common.Warning", "Attenzione"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return;
				}

				var result = MessageBox.Show(
					string.Format(
						LanguageManager.Get("TimeSlots.ConfirmDelete", "Eliminare {0} punti orari selezionati?"),
						toDelete.Count
					),
					LanguageManager.Get("Common.Confirm", "Conferma"),
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);

				if (result == DialogResult.Yes)
				{
					foreach (var slot in toDelete)
					{
						_timeSlots.Remove(slot);
					}

					RefreshGrid();
					_isDirty = true;
					UpdateStatusLabel();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("TimeSlots.DeleteError", "Errore eliminazione")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (!_isDirty)
				{
					MessageBox.Show(
						LanguageManager.Get("Messages.NoChanges", "Nessuna modifica da salvare."),
						LanguageManager.Get("Common.Information", "Info"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
					return;
				}

				var allSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc");
				allSlots.RemoveAll(t => t.StationID == _stationID);
				allSlots.AddRange(_timeSlots);

				for (int i = 0; i < allSlots.Count; i++)
				{
					allSlots[i].ID = i + 1;
				}

				bool success = DbcManager.Save("ADV_TimeSlots.dbc", allSlots);

				if (success)
				{
					// ✅ Rigenera export AirDirector
					try
					{
						bool exported = AirDirectorExportService.ExportFullSchedule(
							_stationID,
							DateTime.Now.Date,
							DateTime.Now.Date.AddDays(30)
						);

						if (exported)
						{
							Console.WriteLine("[TimeSlotsForm] ✅ Export AirDirector rigenerato");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"[TimeSlotsForm] ⚠️ Errore export:  {ex.Message}");
					}

					_isDirty = false;
					MessageBox.Show(
						LanguageManager.Get("TimeSlots.SaveSuccess", "✅ Punti orari salvati! "),
						LanguageManager.Get("Common.Success", "Successo"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					MessageBox.Show(
						LanguageManager.Get("Messages.SaveError", "❌ Errore salvataggio!"),
						LanguageManager.Get("Common.Error", "Errore"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("Messages.SaveError", "Errore")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			if (_isDirty)
			{
				var result = MessageBox.Show(
					LanguageManager.Get("Messages.UnsavedChangesReload", "Modifiche non salvate.Ricaricare? "),
					LanguageManager.Get("Common.Confirm", "Conferma"),
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);

				if (result == DialogResult.No)
					return;
			}

			LoadTimeSlots();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (_isDirty)
			{
				var result = MessageBox.Show(
					LanguageManager.Get("Messages.UnsavedChangesSave", "Salvare le modifiche? "),
					LanguageManager.Get("Common.Confirm", "Conferma"),
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question
				);

				if (result == DialogResult.Yes)
				{
					btnSave_Click(null, null);
				}
				else if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}

			// ✅ Cleanup
			LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
		}
	}
}
