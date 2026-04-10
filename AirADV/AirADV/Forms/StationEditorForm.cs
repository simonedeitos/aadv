using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
	public partial class StationManagerForm : Form
	{
		private DbcManager.Station _selectedStation = null;

		public int SelectedStationID { get; private set; } = 0;

		public StationManagerForm()
		{
			InitializeComponent();
			this.Load += StationManagerForm_Load;
		}

		private void StationManagerForm_Load(object sender, EventArgs e)
		{
			try
			{
				LoadStations();
				ClearEditor();
				ApplyLanguage();

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
			Console.WriteLine("[StationManager] 🔄 Cambio lingua rilevato");
			ApplyLanguage();
			LoadStations(); // ✅ Ricarica per aggiornare label count
		}

		private void LoadStations()
		{
			try
			{
				var stations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc")
					.Where(s => s.IsActive)
					.OrderBy(s => s.StationName)
					.ToList();

				lstStations.Items.Clear();

				foreach (var station in stations)
				{
					var item = new ListViewItem(station.StationID.ToString());
					item.SubItems.Add(station.StationName);
					item.SubItems.Add(station.Frequency);
					item.SubItems.Add(station.DatabasePath);
					item.Tag = station;

					// Evidenzia emittente corrente
					if (station.StationID == ConfigManager.CurrentStationID)
					{
						item.BackColor = Color.FromArgb(200, 230, 255);
						item.Font = new Font(lstStations.Font, FontStyle.Bold);
					}

					lstStations.Items.Add(item);
				}

				// ✅ Label tradotta con formato
				lblStationsCount.Text = string.Format(
					LanguageManager.Get("StationManager.StationsCount", "Emittenti configurate: {0}"),
					stations.Count
				);

				Console.WriteLine($"[StationManager] Caricate {stations.Count} emittenti");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[StationManager] Errore LoadStations:  {ex.Message}");
			}
		}

		private void lstStations_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstStations.SelectedItems.Count > 0)
			{
				_selectedStation = lstStations.SelectedItems[0].Tag as DbcManager.Station;
				LoadStationToEditor(_selectedStation);
				btnSelect.Enabled = true;
				btnEditSave.Text = LanguageManager.Get("StationManager.BtnSaveChanges", "💾 Salva Modifiche");
			}
			else
			{
				_selectedStation = null;
				btnSelect.Enabled = false;
				btnEditSave.Text = LanguageManager.Get("StationManager.BtnAddStation", "➕ Aggiungi Emittente");
			}
		}

		private void LoadStationToEditor(DbcManager.Station station)
		{
			try
			{
				txtStationName.Text = station.StationName;
				txtFrequency.Text = station.Frequency;
				txtDatabasePath.Text = station.DatabasePath;
				txtMediaPath.Text = station.MediaPath;
				txtReportsPath.Text = station.ReportsPath;
				chkActive.Checked = station.IsActive;

				// Carica logo
				if (!string.IsNullOrEmpty(station.LogoPath) && File.Exists(station.LogoPath))
				{
					try
					{
						picLogo.Image?.Dispose();
						picLogo.Image = Image.FromFile(station.LogoPath);
						picLogo.SizeMode = PictureBoxSizeMode.Zoom;
					}
					catch
					{
						picLogo.Image = null;
					}
				}
				else
				{
					picLogo.Image = null;
				}

				grpEditor.Enabled = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[StationManager] Errore LoadStationToEditor:  {ex.Message}");
			}
		}

		private void ClearEditor()
		{
			txtStationName.Clear();
			txtFrequency.Clear();
			txtDatabasePath.Clear();
			txtMediaPath.Clear();
			txtReportsPath.Clear();
			chkActive.Checked = true;
			picLogo.Image?.Dispose();
			picLogo.Image = null;
			grpEditor.Enabled = true;
			_selectedStation = null;
			btnEditSave.Text = LanguageManager.Get("StationManager.BtnAddStation", "➕ Aggiungi Emittente");
		}

		// ═══════════════════════════════════════════════════════════
		// SELEZIONE EMITTENTE
		// ═══════════════════════════════════════════════════════════

		private void btnSelect_Click(object sender, EventArgs e)
		{
			if (_selectedStation != null)
			{
				SelectedStationID = _selectedStation.StationID;
				ConfigManager.CurrentStationID = SelectedStationID;
				ConfigManager.Save();

				MessageBox.Show(
					string.Format(
						LanguageManager.Get("StationManager.StationSelected", "✅ Emittente selezionata:\n\n{0}"),
						_selectedStation.StationName
					),
					LanguageManager.Get("StationManager.StationChangedTitle", "Emittente Cambiata"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnSelectClose_Click(object sender, EventArgs e)
		{
			// Usa emittente corrente senza cambiarla
			SelectedStationID = ConfigManager.CurrentStationID;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// ═══════════════════════════════════════════════════════════
		// EDITOR EMITTENTE
		// ═══════════════════════════════════════════════════════════

		private void btnNew_Click(object sender, EventArgs e)
		{
			lstStations.SelectedItems.Clear();
			ClearEditor();
			txtStationName.Focus();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (_selectedStation == null)
			{
				MessageBox.Show(
					LanguageManager.Get("StationManager.SelectStationToDelete", "Seleziona un'emittente da eliminare! "),
					LanguageManager.Get("Common.Warning", "Attenzione"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			if (_selectedStation.StationID == ConfigManager.CurrentStationID)
			{
				MessageBox.Show(
					LanguageManager.Get("StationManager.CannotDeleteCurrentStation", "Non puoi eliminare l'emittente attualmente in uso!"),
					LanguageManager.Get("Common.Warning", "Attenzione"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			var result = MessageBox.Show(
				string.Format(
					LanguageManager.Get("StationManager.ConfirmDelete", "Sei sicuro di voler eliminare l'emittente:\n\n{0}?\n\n⚠️ ATTENZIONE: I file NON verranno eliminati, solo la configurazione."),
					_selectedStation.StationName
				),
				LanguageManager.Get("Messages.ConfirmDeleteTitle", "Conferma Eliminazione"),
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning
			);

			if (result == DialogResult.Yes)
			{
				try
				{
					var stations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc");
					stations.RemoveAll(s => s.StationID == _selectedStation.StationID);
					DbcManager.Save("ADV_Config.dbc", stations);

					MessageBox.Show(
						LanguageManager.Get("StationManager.StationDeleted", "✅ Emittente eliminata! "),
						LanguageManager.Get("Common.Success", "Successo"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					LoadStations();
					ClearEditor();
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						$"{LanguageManager.Get("StationManager.DeleteError", "Errore eliminazione")}:\n{ex.Message}",
						LanguageManager.Get("Common.Error", "Errore"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				}
			}
		}

		private void btnAutoSetup_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtStationName.Text))
				{
					MessageBox.Show(
						LanguageManager.Get("StationManager.EnterStationNameFirst", "Inserisci prima il nome dell'emittente! "),
						LanguageManager.Get("Common.Warning", "Attenzione"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtStationName.Focus();
					return;
				}

				string safeName = string.Join("_", txtStationName.Text.Trim().Split(Path.GetInvalidFileNameChars()));
				string stationFolder = Path.Combine(ConfigManager.STATIONS_PATH, safeName);

				txtDatabasePath.Text = Path.Combine(stationFolder, "Database");
				txtMediaPath.Text = Path.Combine(stationFolder, "Media");
				txtReportsPath.Text = Path.Combine(stationFolder, "Reports");

				MessageBox.Show(
					string.Format(
						LanguageManager.Get("StationManager.AutoSetupSuccess", "✅ Percorsi configurati automaticamente:\n\nDatabase: {0}\nMedia:  {1}\nReport: {2}\n\nLe cartelle verranno create al salvataggio."),
						txtDatabasePath.Text,
						txtMediaPath.Text,
						txtReportsPath.Text
					),
					LanguageManager.Get("StationManager.AutoSetupTitle", "Setup Automatico"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("StationManager.AutoSetupError", "Errore setup automatico")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void btnBrowseDatabasePath_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.Description = LanguageManager.Get("StationManager.SelectDatabaseFolder", "Seleziona cartella Database emittente");
				dialog.SelectedPath = txtDatabasePath.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					txtDatabasePath.Text = dialog.SelectedPath;
				}
			}
		}

		private void btnBrowseMediaPath_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.Description = LanguageManager.Get("StationManager.SelectMediaFolder", "Seleziona cartella Media emittente");
				dialog.SelectedPath = txtMediaPath.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					txtMediaPath.Text = dialog.SelectedPath;
				}
			}
		}

		private void btnBrowseReportsPath_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.Description = LanguageManager.Get("StationManager.SelectReportsFolder", "Seleziona cartella Report emittente");
				dialog.SelectedPath = txtReportsPath.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					txtReportsPath.Text = dialog.SelectedPath;
				}
			}
		}

		private void btnBrowseLogo_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				dialog.Title = LanguageManager.Get("StationManager.SelectLogo", "Seleziona logo emittente");
				dialog.Filter = LanguageManager.Get("StationManager.ImageFilter", "Immagini|*.png;*.jpg;*.jpeg;*.bmp|Tutti i file|*.*");

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						picLogo.Image?.Dispose();
						picLogo.Image = Image.FromFile(dialog.FileName);
						picLogo.SizeMode = PictureBoxSizeMode.Zoom;

						if (_selectedStation != null)
						{
							_selectedStation.LogoPath = dialog.FileName;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(
							$"{LanguageManager.Get("StationManager.LogoLoadError", "Errore caricamento logo")}:\n{ex.Message}",
							LanguageManager.Get("Common.Error", "Errore"),
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
					}
				}
			}
		}

		private void btnRemoveLogo_Click(object sender, EventArgs e)
		{
			picLogo.Image?.Dispose();
			picLogo.Image = null;

			if (_selectedStation != null)
			{
				_selectedStation.LogoPath = "";
			}
		}

		private void btnEditSave_Click(object sender, EventArgs e)
		{
			try
			{
				// Validazione
				if (string.IsNullOrWhiteSpace(txtStationName.Text))
				{
					MessageBox.Show(
						LanguageManager.Get("StationManager.EnterStationName", "Inserisci il nome dell'emittente!"),
						LanguageManager.Get("Common.Warning", "Attenzione"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					txtStationName.Focus();
					return;
				}

				if (string.IsNullOrWhiteSpace(txtDatabasePath.Text))
				{
					MessageBox.Show(
						LanguageManager.Get("StationManager.EnterDatabasePath", "Inserisci il percorso Database! "),
						LanguageManager.Get("Common.Warning", "Attenzione"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return;
				}

				// Crea cartelle
				Directory.CreateDirectory(txtDatabasePath.Text);
				Directory.CreateDirectory(txtMediaPath.Text);
				Directory.CreateDirectory(txtReportsPath.Text);

				var stations = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc");

				if (_selectedStation != null)
				{
					// ✅ MODIFICA ESISTENTE
					_selectedStation.StationName = txtStationName.Text.Trim();
					_selectedStation.Frequency = txtFrequency.Text.Trim();
					_selectedStation.DatabasePath = txtDatabasePath.Text.Trim();
					_selectedStation.MediaPath = txtMediaPath.Text.Trim();
					_selectedStation.ReportsPath = txtReportsPath.Text.Trim();
					_selectedStation.IsActive = chkActive.Checked;

					for (int i = 0; i < stations.Count; i++)
					{
						if (stations[i].StationID == _selectedStation.StationID)
						{
							stations[i] = _selectedStation;
							break;
						}
					}

					DbcManager.Save("ADV_Config.dbc", stations);

					MessageBox.Show(
						string.Format(
							LanguageManager.Get("StationManager.StationModified", "✅ Emittente '{0}' modificata! "),
							_selectedStation.StationName
						),
						LanguageManager.Get("Common.Success", "Successo"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					// ✅ NUOVA EMITTENTE
					int maxStationId = 0;
					foreach (var s in stations)
					{
						if (s.StationID > maxStationId)
							maxStationId = s.StationID;
					}

					var newStation = new DbcManager.Station
					{
						StationID = maxStationId + 1,
						StationName = txtStationName.Text.Trim(),
						Frequency = txtFrequency.Text.Trim(),
						DatabasePath = txtDatabasePath.Text.Trim(),
						MediaPath = txtMediaPath.Text.Trim(),
						ReportsPath = txtReportsPath.Text.Trim(),
						LogoPath = picLogo.Image != null ? _selectedStation?.LogoPath ?? "" : "",
						IsActive = chkActive.Checked,
						CreatedDate = DateTime.Now
					};

					stations.Add(newStation);
					DbcManager.Save("ADV_Config.dbc", stations);

					MessageBox.Show(
						string.Format(
							LanguageManager.Get("StationManager.StationAdded", "✅ Emittente '{0}' aggiunta!"),
							newStation.StationName
						),
						LanguageManager.Get("Common.Success", "Successo"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}

				LoadStations();
				ClearEditor();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{LanguageManager.Get("Messages.SaveError", "Errore salvataggio")}:\n{ex.Message}",
					LanguageManager.Get("Common.Error", "Errore"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		/// <summary>
		/// ✅ COMPLETO:  Tutte le traduzioni
		/// </summary>
		private void ApplyLanguage()
		{
			try
			{
				Console.WriteLine($"[StationManager] 🌐 Applicazione traduzioni (lingua:  {LanguageManager.CurrentCulture})");

				// ✅ Titolo finestra
				this.Text = LanguageManager.Get("StationManager.WindowTitle", "🔄 Gestione Emittenti");

				// ✅ GroupBox
				grpStationList.Text = LanguageManager.Get("StationManager.GrpStationList", "📻 Emittenti Configurate");
				grpEditor.Text = LanguageManager.Get("StationManager.GrpEditor", "✏️ Editor Emittente");

				// ✅ Colonne ListView
				colID.Text = LanguageManager.Get("StationManager.ColID", "ID");
				colName.Text = LanguageManager.Get("StationManager.ColName", "Nome Emittente");
				colFrequency.Text = LanguageManager.Get("StationManager.ColFrequency", "Frequenza");
				colPath.Text = LanguageManager.Get("StationManager.ColPath", "Percorso Database");

				// ✅ Labels Editor
				lblStationName.Text = LanguageManager.Get("StationManager.LblStationName", "Nome Emittente:");
				lblFrequency.Text = LanguageManager.Get("StationManager.LblFrequency", "Frequenza:");
				lblDatabasePath.Text = LanguageManager.Get("StationManager.LblDatabasePath", "Database:");
				lblMediaPath.Text = LanguageManager.Get("StationManager.LblMediaPath", "Media:");
				lblReportsPath.Text = LanguageManager.Get("StationManager.LblReportsPath", "Report:");
				chkActive.Text = LanguageManager.Get("StationManager.ChkActive", "Attiva");

				// ✅ Bottoni
				btnSelect.Text = LanguageManager.Get("StationManager.BtnSelect", "✅ Seleziona e Cambia");
				btnSelectClose.Text = LanguageManager.Get("StationManager.BtnSelectClose", "✖ Chiudi");
				btnNew.Text = LanguageManager.Get("StationManager.BtnNew", "➕ Nuova");
				btnDelete.Text = LanguageManager.Get("StationManager.BtnDelete", "🗑️ Elimina");
				btnAutoSetup.Text = LanguageManager.Get("StationManager.BtnAutoSetup", "⚡ Setup Automatico Cartelle");
				btnBrowseLogo.Text = LanguageManager.Get("StationManager.BtnBrowseLogo", "📁 Logo...");
				btnRemoveLogo.Text = LanguageManager.Get("StationManager.BtnRemoveLogo", "🗑️ Rimuovi");

				// ✅ Bottone Salva (dinamico in base a selezione)
				if (_selectedStation != null)
				{
					btnEditSave.Text = LanguageManager.Get("StationManager.BtnSaveChanges", "💾 Salva Modifiche");
				}
				else
				{
					btnEditSave.Text = LanguageManager.Get("StationManager.BtnAddStation", "➕ Aggiungi Emittente");
				}

				// ✅ Bottoni Browse (uguali)
				string browseText = LanguageManager.Get("Common.Browse", "📁 Sfoglia");
				btnBrowseDatabasePath.Text = browseText;
				btnBrowseMediaPath.Text = browseText;
				btnBrowseReportsPath.Text = browseText;

				// ✅ Refresh visivo
				this.Refresh();

				Console.WriteLine("[StationManager] ✅ Traduzioni applicate");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[StationManager] ❌ Errore ApplyLanguage: {ex.Message}");
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			// ✅ Cleanup
			picLogo.Image?.Dispose();

			// ✅ Rimuovi listener
			LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
		}
	}
}
