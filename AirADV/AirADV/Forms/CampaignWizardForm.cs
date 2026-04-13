using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;

namespace AirADV.Forms
{
    public partial class CampaignWizardForm : Form
    {
        private int _stationID = 0;
        private DbcManager.Campaign _campaign;
        private List<int> _selectedSpotIDs = new List<int>();
        private List<ScheduleEngine.DailySchedule> _dailySchedules;

        private bool _avoidRepetition = true;
        private bool _distributePasses = true;

        private CheckBox chkAvoidRepetition;
        private CheckBox chkDistributePasses;

        private bool _scheduleGenerated = false;

        private bool _isLoadingData = false;

        private bool _isDuplicate = false;

        private DataGridView dgvSchedule;
        private List<string> _availableTimeSlots;

        public CampaignWizardForm(int stationID)
        {
            InitializeComponent();
            InitializeAdvancedOptions();
            InitializeScheduleGrid();
            _stationID = stationID;
            _campaign = new DbcManager.Campaign
            {
                StationID = stationID,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                DailyPasses = 8,
                TimeFrom = "00:00:00",
                TimeTo = "23:59:59",
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true,
                DistributionMode = "BALANCED",
                IsActive = true,
                CategoryID = 0 // ✅ Default vuoto
            };

            this.Load += CampaignWizardForm_Load;
        }

        public CampaignWizardForm(int stationID, int clientID) : this(stationID)
        {
            this.Load += (s, e) =>
            {
                if (cmbClient.Items.Count > 0)
                {
                    for (int i = 0; i < cmbClient.Items.Count; i++)
                    {
                        var client = cmbClient.Items[i] as DbcManager.Client;
                        if (client != null && client.ID == clientID)
                        {
                            cmbClient.SelectedIndex = i;
                            break;
                        }
                    }
                }
            };
        }

        public CampaignWizardForm(int stationID, DbcManager.Campaign existingCampaign)
        {
            InitializeComponent();
            InitializeAdvancedOptions();
            InitializeScheduleGrid();
            _stationID = stationID;
            _campaign = existingCampaign;

            if (existingCampaign.SpotID > 0)
            {
                _selectedSpotIDs.Add(existingCampaign.SpotID);
            }

            this.Load += CampaignWizardForm_Load;
            this.Text = string.Format(
                LanguageManager.Get("CampaignWizard.EditTitle", "✏️ Modifica Campagna - {0}"),
                existingCampaign.CampaignName
            );
        }

        public CampaignWizardForm(int stationID, DbcManager.Campaign duplicateCampaign, bool isDuplicate)
            : this(stationID, duplicateCampaign)
        {
            _isDuplicate = isDuplicate;
        }

        private void InitializeAdvancedOptions()
        {
            chkAvoidRepetition = new CheckBox
            {
                Name = "chkAvoidRepetition",
                Text = LanguageManager.Get("CampaignWizard.ChkAvoidRepetition", "⚙️ Evita ripetizione orari nei giorni successivi"),
                AutoSize = true,
                Checked = true,
                Location = new Point(350, 28),
                Font = new Font("Segoe UI", 9F),
                TabIndex = 12
            };

            chkDistributePasses = new CheckBox
            {
                Name = "chkDistributePasses",
                Text = LanguageManager.Get("CampaignWizard.ChkDistributePasses", "📊 Distribuisci passaggi uniformemente"),
                AutoSize = true,
                Checked = true,
                Location = new Point(680, 28),
                Font = new Font("Segoe UI", 9F),
                TabIndex = 13
            };

            try
            {
                if (grpAutoConfig != null)
                {
                    grpAutoConfig.Controls.Add(chkAvoidRepetition);
                    grpAutoConfig.Controls.Add(chkDistributePasses);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore InitializeAdvancedOptions: {ex.Message}");
            }
        }

        private void InitializeScheduleGrid()
        {
            dgvSchedule = new DataGridView
            {
                Name = "dgvSchedule",
                Location = new Point(25, 60),
                Size = new Size(1014, 360),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                Font = new Font("Segoe UI", 9F),
                EnableHeadersVisualStyles = false,
                EditMode = DataGridViewEditMode.EditOnEnter,
                ScrollBars = ScrollBars.Both,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                RowTemplate = { Height = 25 }
            };

            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                HeaderText = LanguageManager.Get("CampaignWizard.ColDate", "Data"),
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DayOfWeek",
                HeaderText = LanguageManager.Get("CampaignWizard.ColDayOfWeek", "Giorno"),
                Width = 90,
                ReadOnly = true
            });

            dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PassCount",
                HeaderText = LanguageManager.Get("CampaignWizard.ColPassCount", "Pass."),
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            for (int i = 1; i <= 15; i++)
            {
                var comboColumn = new DataGridViewComboBoxColumn
                {
                    Name = $"Slot{i}",
                    HeaderText = string.Format(
                        LanguageManager.Get("CampaignWizard.ColSlot", "Orario {0}"),
                        i
                    ),
                    Width = 80,
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat
                };
                dgvSchedule.Columns.Add(comboColumn);
            }

            dgvSchedule.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            dgvSchedule.ColumnHeadersHeight = 30;
            dgvSchedule.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dgvSchedule.DataError += DgvSchedule_DataError;
            dgvSchedule.CellValueChanged += DgvSchedule_CellValueChanged;
            dgvSchedule.CurrentCellDirtyStateChanged += DgvSchedule_CurrentCellDirtyStateChanged;
            dgvSchedule.CellMouseEnter += DgvSchedule_CellMouseEnter;

            try
            {
                if (pnlStep3 != null)
                {
                    pnlStep3.Controls.Add(dgvSchedule);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore InitializeScheduleGrid: {ex.Message}");
            }
        }

        private void CampaignWizardForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadClients();
                LoadCategories();
                LoadTimeSlots();
                LoadAvailableTimeSlots();
                ApplyLanguage();

                UpdateDistributionMode();
                UpdateManualSlotCount();
                DisableStep3();
                AddGenerateButton();

                if (_campaign.ID > 0 || _isDuplicate)
                {
                    LoadExistingCampaignData();
                }

                if (!chkTimeFilter.Checked)
                {
                    dtpTimeFrom.Value = DateTime.Today.AddHours(0);
                    dtpTimeTo.Value = DateTime.Today.AddHours(23).AddMinutes(59);
                }

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
            Console.WriteLine("[CampaignWizard] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
        }

        // ✅ NUOVO:  Handler cambio categoria
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            if (cmbCategory.SelectedItem is DbcManager.Category selectedCategory)
            {
                _campaign.CategoryID = selectedCategory.ID;
                Console.WriteLine($"[CampaignWizard] Categoria selezionata: {selectedCategory.CategoryName} (ID: {selectedCategory.ID})");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string title = LanguageManager.Get("CampaignWizard.AddCategoryTitle", "Nuova Categoria");
            string prompt = LanguageManager.Get("CampaignWizard.AddCategoryPrompt", "Inserisci il nome della nuova categoria merceologica:");

            using (var inputForm = new Form())
            {
                inputForm.Text = title;
                inputForm.Size = new System.Drawing.Size(420, 160);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;
                inputForm.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

                var lbl = new Label
                {
                    Text = prompt,
                    Location = new System.Drawing.Point(12, 15),
                    Size = new System.Drawing.Size(380, 20),
                    ForeColor = System.Drawing.Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 9.5f)
                };

                var txt = new TextBox
                {
                    Location = new System.Drawing.Point(12, 42),
                    Size = new System.Drawing.Size(380, 25),
                    Font = new System.Drawing.Font("Segoe UI", 10f),
                    BackColor = System.Drawing.Color.FromArgb(62, 62, 66),
                    ForeColor = System.Drawing.Color.White
                };

                var btnOk = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(220, 80),
                    Size = new System.Drawing.Size(80, 28),
                    BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderSize = 0;

                var btnCancel = new Button
                {
                    Text = LanguageManager.Get("Common.Cancel", "Annulla"),
                    DialogResult = DialogResult.Cancel,
                    Location = new System.Drawing.Point(312, 80),
                    Size = new System.Drawing.Size(80, 28),
                    BackColor = System.Drawing.Color.FromArgb(108, 117, 125),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                inputForm.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
                inputForm.AcceptButton = btnOk;
                inputForm.CancelButton = btnCancel;

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    string categoryName = txt.Text.Trim();
                    if (string.IsNullOrEmpty(categoryName))
                    {
                        MessageBox.Show(
                            LanguageManager.Get("CampaignWizard.AddCategoryEmptyName", "Il nome della categoria non può essere vuoto!"),
                            LanguageManager.Get("Common.Warning", "Attenzione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    try
                    {
                        var allCategories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc");
                        int maxID = allCategories.Any() ? allCategories.Max(c => c.ID) : 0;
                        int maxNum = 0;
                        foreach (var c in allCategories)
                        {
                            if (c.CategoryCode.StartsWith("CAT-") && int.TryParse(c.CategoryCode.Substring(4), out int n))
                                if (n > maxNum) maxNum = n;
                        }

                        var newCategory = new DbcManager.Category
                        {
                            ID = maxID + 1,
                            CategoryCode = $"CAT-{(maxNum + 1):D3}",
                            CategoryName = categoryName,
                            Color = "#808080",
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };

                        allCategories.Add(newCategory);
                        DbcManager.Save("ADV_Categories.dbc", allCategories);

                        LoadCategories();

                        // Seleziona la nuova categoria
                        for (int i = 0; i < cmbCategory.Items.Count; i++)
                        {
                            if (cmbCategory.Items[i] is DbcManager.Category cat && cat.ID == newCategory.ID)
                            {
                                cmbCategory.SelectedIndex = i;
                                break;
                            }
                        }

                        MessageBox.Show(
                            string.Format(LanguageManager.Get("CampaignWizard.AddCategorySuccess", "✅ Categoria '{0}' creata!"), categoryName),
                            LanguageManager.Get("Common.Success", "Successo"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CampaignWizard] Errore creazione categoria: {ex.Message}");
                        MessageBox.Show(ex.Message, LanguageManager.Get("Common.Error", "Errore"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadClients()
        {
            var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc")
                .Where(c => c.IsActive)
                .OrderBy(c => c.ClientName)
                .ToList();

            cmbClient.DataSource = clients;
            cmbClient.DisplayMember = "ClientName";
            cmbClient.ValueMember = "ID";

            // ✅ Reset selezione
            if (clients.Count > 0)
                cmbClient.SelectedIndex = -1;
        }

        private void LoadCategories()
        {
            _isLoadingData = true;
            try
            {
                var categories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc")
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "ID";

                // ✅ Reset selezione (nessuna categoria di default)
                if (categories.Count > 0)
                    cmbCategory.SelectedIndex = -1;

                Console.WriteLine($"[CampaignWizard] Caricate {categories.Count} categorie");
            }
            finally
            {
                _isLoadingData = false;
            }
        }

        // ✅ VALIDAZIONE STEP1 AGGIORNATA
        private bool ValidateStep1()
        {
            if (string.IsNullOrWhiteSpace(txtCampaignName.Text))
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorCampaignName", "Inserisci il nome della campagna! "),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtCampaignName.Focus();
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep1.Top - 20);
                return false;
            }

            if (cmbClient.SelectedValue == null || cmbClient.SelectedIndex < 0)
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorClient", "Seleziona un cliente!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                cmbClient.Focus();
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep1.Top - 20);
                return false;
            }

            // ✅ NUOVA VALIDAZIONE: Categoria obbligatoria
            if (cmbCategory.SelectedValue == null || cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorCategory", "Seleziona una categoria merceologica!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                cmbCategory.Focus();
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep1.Top - 20);
                return false;
            }

            if (_selectedSpotIDs.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorSpots", "Aggiungi almeno uno spot alla campagna!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep1.Top - 20);
                return false;
            }

            if (dtpEndDate.Value <= dtpStartDate.Value)
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorDates", "La data fine deve essere successiva alla data inizio!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep1.Top - 20);
                return false;
            }

            return true;
        }

        private void SaveStep1Data()
        {
            try
            {
                _campaign.CampaignName = txtCampaignName.Text.Trim();

                if (cmbClient.SelectedItem is DbcManager.Client selectedClient)
                {
                    _campaign.ClientID = selectedClient.ID;
                }

                // ✅ SALVA CATEGORIA (ora obbligatoria)
                if (cmbCategory.SelectedItem is DbcManager.Category selectedCategory)
                {
                    _campaign.CategoryID = selectedCategory.ID;
                    Console.WriteLine($"[CampaignWizard] ✅ Categoria salvata:  {selectedCategory.CategoryName} (ID: {selectedCategory.ID})");
                }
                else
                {
                    // ✅ Fallback di sicurezza (non dovrebbe mai succedere dopo validazione)
                    Console.WriteLine("[CampaignWizard] ⚠️ ATTENZIONE: Nessuna categoria selezionata!");
                    _campaign.CategoryID = 0;
                }

                _campaign.SpotID = _selectedSpotIDs.Count > 0 ? _selectedSpotIDs[0] : 0;
                _campaign.StartDate = dtpStartDate.Value.Date;
                _campaign.EndDate = dtpEndDate.Value.Date;

                if (_campaign.ID == 0)
                {
                    // ✅ Contatore persistente: non riparte mai da capo
                    var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                    int maxFromCampaigns = 0;

                    foreach (var c in campaigns)
                    {
                        if (c.CampaignCode.StartsWith("CAMP-"))
                        {
                            string numberPart = c.CampaignCode.Substring(5);
                            if (int.TryParse(numberPart, out int number))
                            {
                                if (number > maxFromCampaigns)
                                    maxFromCampaigns = number;
                            }
                        }
                    }

                    // Usa il massimo tra il contatore persistente e quello ricavato dalle campagne esistenti
                    int nextNumber = Math.Max(ConfigManager.LastCampaignNumber, maxFromCampaigns) + 1;
                    ConfigManager.LastCampaignNumber = nextNumber;
                    ConfigManager.Save();

                    _campaign.CampaignCode = $"CAMP-{nextNumber:D3}";
                }

                Console.WriteLine($"[CampaignWizard] Step1 salvato - Cliente: {_campaign.ClientID}, Spot: {_campaign.SpotID}, Categoria: {_campaign.CategoryID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore SaveStep1Data: {ex.Message}");
                throw;
            }
        }
        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[CampaignWizard] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                // ✅ Titolo finestra
                if (_campaign.ID > 0)
                {
                    this.Text = string.Format(
                        LanguageManager.Get("CampaignWizard.EditTitle", "✏️ Modifica Campagna - {0}"),
                        _campaign.CampaignName
                    );
                }
                else
                {
                    this.Text = LanguageManager.Get("CampaignWizard.WindowTitle", "📋 Nuova Campagna Pubblicitaria");
                }

                // ✅ STEP 1
                lblStep1Title.Text = LanguageManager.Get("CampaignWizard.Step1Title", "📋 STEP 1: Dati Campagna");
                lblCampaignName.Text = LanguageManager.Get("CampaignWizard.LblCampaignName", "Nome Campagna:");
                lblClient.Text = LanguageManager.Get("CampaignWizard.LblClient", "Cliente:");
                lblCategory.Text = LanguageManager.Get("CampaignWizard.LblCategory", "Categoria:");
                lblSpots.Text = LanguageManager.Get("CampaignWizard.LblSpots", "Spot (sequenza rotazione):");

                grpPeriod.Text = LanguageManager.Get("CampaignWizard.GrpPeriod", "📅 Periodo Campagna");
                lblStartDate.Text = LanguageManager.Get("CampaignWizard.LblStartDate", "Data Inizio:");
                lblEndDate.Text = LanguageManager.Get("CampaignWizard.LblEndDate", "Data Fine:");

                btnAddSpot.Text = LanguageManager.Get("CampaignWizard.BtnAddSpot", "➕ Aggiungi");
                btnRemoveSpot.Text = LanguageManager.Get("CampaignWizard.BtnRemoveSpot", "➖ Rimuovi");
                btnMoveUp.Text = LanguageManager.Get("CampaignWizard.BtnMoveUp", "⬆️");
                btnMoveDown.Text = LanguageManager.Get("CampaignWizard.BtnMoveDown", "⬇️");

                // ✅ STEP 2
                lblStep2Title.Text = LanguageManager.Get("CampaignWizard.Step2Title", "⚙️ STEP 2: Configurazione Schedulazione");
                grpDistribution.Text = LanguageManager.Get("CampaignWizard.GrpDistribution", "Modalità Distribuzione");
                rdAutoBalanced.Text = LanguageManager.Get("CampaignWizard.OptAutoBalanced", "Automatica - Bilanciata");
                rdAutoAudience.Text = LanguageManager.Get("CampaignWizard.OptAutoAudience", "Automatica - Affollamento Audience");
                rdManual.Text = LanguageManager.Get("CampaignWizard.OptManual", "Manuale - Selezione Punti Orari");

                grpAutoConfig.Text = LanguageManager.Get("CampaignWizard.GrpAutoConfig", "Configurazione Automatica");
                lblDailyPasses.Text = LanguageManager.Get("CampaignWizard.LblDailyPasses", "Passaggi al giorno:");

                if (chkAvoidRepetition != null)
                    chkAvoidRepetition.Text = LanguageManager.Get("CampaignWizard.ChkAvoidRepetition", "⚙️ Evita ripetizione orari nei giorni successivi");

                if (chkDistributePasses != null)
                    chkDistributePasses.Text = LanguageManager.Get("CampaignWizard.ChkDistributePasses", "📊 Distribuisci passaggi uniformemente");

                chkTimeFilter.Text = LanguageManager.Get("CampaignWizard.ChkTimeFilter", "☑ Abilita Filtro Orario");
                lblTimeFrom.Text = LanguageManager.Get("CampaignWizard.LblTimeFrom", "Da:");
                lblTimeTo.Text = LanguageManager.Get("CampaignWizard.LblTimeTo", "A:");

                grpDays.Text = LanguageManager.Get("CampaignWizard.GrpDays", "Giorni Settimana");
                grpManualSlots.Text = LanguageManager.Get("CampaignWizard.GrpManualSlots", "Selezione Manuale Punti Orari");
                btnSelectAllSlots.Text = LanguageManager.Get("CampaignWizard.BtnSelectAll", "✓ Seleziona Tutti");
                btnDeselectAllSlots.Text = LanguageManager.Get("CampaignWizard.BtnDeselectAll", "✖ Deseleziona Tutti");
                if (lblManualDailyPasses != null)
                    lblManualDailyPasses.Text = LanguageManager.Get("CampaignWizard.LblManualDailyPasses", "Passaggi al giorno:");
                UpdateManualSlotCount();

                // ✅ STEP 3
                if (_campaign.ID > 0 && !_scheduleGenerated)
                {
                    lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Loading", "⏳ STEP 3: Caricamento Schedulazione Esistente...");
                }
                else if (_scheduleGenerated)
                {
                    lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Active", "✅ STEP 3: Revisione e Modifica Schedulazione");
                }
                else
                {
                    lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Disabled", "⏳ STEP 3: Revisione Schedulazione (Genera prima la schedulazione)");
                }

                btnConfirmAll.Text = LanguageManager.Get("CampaignWizard.BtnResetAll", "🔄 Reset Modifiche");

                // ✅ Bottoni principali
                btnSave.Text = LanguageManager.Get("Common.Save", "💾 Salva");
                btnCancel.Text = LanguageManager.Get("Common.Cancel", "✖ Annulla");

                // ✅ Giorni settimana
                chkMonday.Text = LanguageManager.Get("Common.DayMonday", "Lun");
                chkTuesday.Text = LanguageManager.Get("Common.DayTuesday", "Mar");
                chkWednesday.Text = LanguageManager.Get("Common.DayWednesday", "Mer");
                chkThursday.Text = LanguageManager.Get("Common.DayThursday", "Gio");
                chkFriday.Text = LanguageManager.Get("Common.DayFriday", "Ven");
                chkSaturday.Text = LanguageManager.Get("Common.DaySaturday", "Sab");
                chkSunday.Text = LanguageManager.Get("Common.DaySunday", "Dom");

                // ✅ Bottone genera schedulazione
                var btnGenerate = panelButtons.Controls.Find("btnGenerateSchedule", false).FirstOrDefault();
                if (btnGenerate != null)
                {
                    btnGenerate.Text = LanguageManager.Get("CampaignWizard.BtnGenerate", "📊 Genera Schedulazione");
                }

                // ✅ Aggiorna header colonne grid
                if (dgvSchedule != null && dgvSchedule.Columns.Count > 0)
                {
                    dgvSchedule.Columns["Date"].HeaderText = LanguageManager.Get("CampaignWizard.ColDate", "Data");
                    dgvSchedule.Columns["DayOfWeek"].HeaderText = LanguageManager.Get("CampaignWizard.ColDayOfWeek", "Giorno");
                    dgvSchedule.Columns["PassCount"].HeaderText = LanguageManager.Get("CampaignWizard.ColPassCount", "Pass.");

                    for (int i = 1; i <= 15; i++)
                    {
                        if (dgvSchedule.Columns.Contains($"Slot{i}"))
                        {
                            dgvSchedule.Columns[$"Slot{i}"].HeaderText = string.Format(
                                LanguageManager.Get("CampaignWizard.ColSlot", "Orario {0}"),
                                i
                            );
                        }
                    }
                }

                Console.WriteLine("[CampaignWizard] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore ApplyLanguage: {ex.Message}");
            }
        }

        private void LoadAvailableTimeSlots()
        {
            try
            {
                var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                    .Where(t => t.StationID == _stationID && t.IsActive)
                    .OrderBy(t => t.SlotTime)
                    .Select(t => t.SlotTime)
                    .ToList();

                _availableTimeSlots = timeSlots.Select(t => FormatTimeSlot(t)).ToList();

                Console.WriteLine($"[CampaignWizard] ✅ Caricati {_availableTimeSlots.Count} orari disponibili");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore LoadAvailableTimeSlots: {ex.Message}");
                _availableTimeSlots = new List<string>();
            }
        }

        private string FormatTimeSlot(string timeSlot)
        {
            if (string.IsNullOrEmpty(timeSlot))
                return timeSlot;

            if (timeSlot.Length <= 5)
                return timeSlot;

            if (timeSlot.Length >= 8 && timeSlot.Contains(":"))
            {
                var parts = timeSlot.Split(':');
                if (parts.Length >= 2)
                {
                    return $"{parts[0]}:{parts[1]}";
                }
            }

            return timeSlot;
        }

        private string ConvertToFullTime(string shortTime)
        {
            if (string.IsNullOrEmpty(shortTime))
                return shortTime;

            if (shortTime.Length >= 8)
                return shortTime;

            if (shortTime.Length == 5 && shortTime.Contains(":"))
            {
                return $"{shortTime}:00";
            }

            return shortTime;
        }

        private void AddGenerateButton()
        {
            if (_campaign.ID > 0)
            {
                Console.WriteLine("[CampaignWizard] ⚠️ Campagna esistente - bottone 'Genera' non necessario");
                return;
            }

            var btnGenerateSchedule = new Button
            {
                Name = "btnGenerateSchedule",
                Text = LanguageManager.Get("CampaignWizard.BtnGenerate", "📊 Genera Schedulazione"),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point((panelButtons.Width - 220) / 2, 15),
                TabIndex = 2
            };

            btnGenerateSchedule.FlatAppearance.BorderSize = 0;

            btnGenerateSchedule.Click += (s, e) =>
            {
                if (!ValidateStep1() || !ValidateStep2())
                    return;

                SaveStep1Data();
                SaveStep2Data();
                GenerateSchedule();
                EnableStep3();
                LoadScheduleGrid();

                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.GenerateSuccess", "✅ Schedulazione generata!\n\nPuoi ora modificare gli orari di ogni giorno cliccando sulle celle nello Step 3."),
                    LanguageManager.Get("Common.Success", "Successo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            };

            panelButtons.Controls.Add(btnGenerateSchedule);
            btnGenerateSchedule.BringToFront();

            Console.WriteLine("[CampaignWizard] ✅ Bottone 'Genera Schedulazione' aggiunto");
        }

        private void HideGenerateButton()
        {
            var btnGenerate = panelButtons.Controls.Find("btnGenerateSchedule", false).FirstOrDefault();
            if (btnGenerate != null)
            {
                btnGenerate.Visible = false;
                Console.WriteLine("[CampaignWizard] ✅ Bottone 'Genera Schedulazione' nascosto");
            }
        }

        private void DisableStep3()
        {
            if (_scheduleGenerated)
            {
                Console.WriteLine("[CampaignWizard] ⚠️ DisableStep3 ignorato - schedulazione già generata");
                return;
            }

            pnlStep3.Enabled = false;
            lblStep3Title.BackColor = Color.Gray;

            if (_campaign.ID > 0)
            {
                lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Loading", "⏳ STEP 3: Caricamento Schedulazione Esistente...");
            }
            else
            {
                lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Disabled", "⏳ STEP 3: Revisione Schedulazione (Genera prima la schedulazione)");
            }

            Console.WriteLine($"[CampaignWizard] Step3 DISABILITATO");
        }

        private void EnableStep3()
        {
            pnlStep3.Enabled = true;
            lblStep3Title.BackColor = Color.FromArgb(40, 167, 69);
            lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Active", "✅ STEP 3: Revisione e Modifica Schedulazione");
            _scheduleGenerated = true;

            Console.WriteLine($"[CampaignWizard] ✅ Step3 ABILITATO");
        }

        private void LoadExistingCampaignData()
        {
            _isLoadingData = true;
            try
            {
                Console.WriteLine($"[CampaignWizard] 🔄 Caricamento campagna esistente ID: {_campaign.ID}");

                txtCampaignName.Text = _campaign.CampaignName;

                for (int i = 0; i < cmbClient.Items.Count; i++)
                {
                    if (cmbClient.Items[i] is DbcManager.Client client && client.ID == _campaign.ClientID)
                    {
                        cmbClient.SelectedIndex = i;
                        break;
                    }
                }

                // ✅ Carica categoria esistente
                for (int i = 0; i < cmbCategory.Items.Count; i++)
                {
                    if (cmbCategory.Items[i] is DbcManager.Category category && category.ID == _campaign.CategoryID)
                    {
                        cmbCategory.SelectedIndex = i;
                        Console.WriteLine($"[CampaignWizard] ✅ Categoria esistente caricata: {category.CategoryName}");
                        break;
                    }
                }

                // Allow past start dates by not enforcing MinDate
                try { dtpStartDate.Value = _campaign.StartDate; }
                catch (ArgumentOutOfRangeException ex2) { Console.WriteLine($"[CampaignWizard] ⚠️ dtpStartDate out of range: {ex2.Message}"); dtpStartDate.Value = DateTime.Today; }
                dtpEndDate.Value = _campaign.EndDate;

                LoadExistingSpots();
                UpdateSpotList();

                if (_campaign.DailyPasses >= (int)numDailyPasses.Minimum)
                    numDailyPasses.Value = _campaign.DailyPasses;
                else
                    numDailyPasses.Value = numDailyPasses.Minimum;

                if (_campaign.DistributionMode == "BALANCED")
                    rdAutoBalanced.Checked = true;
                else if (_campaign.DistributionMode == "AUDIENCE")
                    rdAutoAudience.Checked = true;
                else if (_campaign.DistributionMode == "MANUAL")
                    rdManual.Checked = true;

                // Load numManualDailyPasses for manual campaigns
                if (_campaign.DistributionMode == "MANUAL")
                {
                    int manualMax = _campaign.DailyPasses > 0 ? _campaign.DailyPasses : 1;
                    if (manualMax >= (int)numManualDailyPasses.Minimum && manualMax <= (int)numManualDailyPasses.Maximum)
                        numManualDailyPasses.Value = manualMax;
                    else
                        numManualDailyPasses.Value = numManualDailyPasses.Minimum;
                }

                chkMonday.Checked = _campaign.Monday;
                chkTuesday.Checked = _campaign.Tuesday;
                chkWednesday.Checked = _campaign.Wednesday;
                chkThursday.Checked = _campaign.Thursday;
                chkFriday.Checked = _campaign.Friday;
                chkSaturday.Checked = _campaign.Saturday;
                chkSunday.Checked = _campaign.Sunday;

                if (_campaign.TimeFrom != "00:00:00" || _campaign.TimeTo != "23:59:59")
                {
                    chkTimeFilter.Checked = true;
                    try { dtpTimeFrom.Value = DateTime.Today.Add(TimeSpan.Parse(_campaign.TimeFrom)); }
                    catch (Exception ex2) { Console.WriteLine($"[CampaignWizard] ⚠️ dtpTimeFrom parse error: {ex2.Message}"); }
                    try { dtpTimeTo.Value = DateTime.Today.Add(TimeSpan.Parse(_campaign.TimeTo)); }
                    catch (Exception ex2) { Console.WriteLine($"[CampaignWizard] ⚠️ dtpTimeTo parse error: {ex2.Message}"); }
                }

                // Restore manual slot checkboxes when editing a manual campaign
                if (_campaign.DistributionMode == "MANUAL" && !string.IsNullOrEmpty(_campaign.ManualSlots))
                {
                    var savedSlots = _campaign.ManualSlots.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (Control ctrl in flowManualSlots.Controls)
                    {
                        if (ctrl is CheckBox chk && chk.Tag != null)
                        {
                            chk.Checked = savedSlots.Contains(chk.Tag.ToString());
                        }
                    }
                    Console.WriteLine($"[CampaignWizard] ✅ Ripristinati {savedSlots.Length} slot manuali");
                }

                // ✅ Per campagne duplicate (ID=0) non caricare la schedulazione
                if (!_isDuplicate)
                {
                    LoadExistingSchedule();
                }
                else
                {
                    // Per una campagna duplicata, la schedulazione deve essere rigenerata dall'utente
                    _scheduleGenerated = false;
                    DisableStep3();
                    Console.WriteLine($"[CampaignWizard] ℹ️ Campagna duplicata: schedulazione azzerata, deve essere rigenerata.");
                }

                Console.WriteLine($"[CampaignWizard] ✅ Caricata campagna esistente: {_campaign.CampaignName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore LoadExistingCampaignData: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.LoadError", "Errore caricamento campagna")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                _isLoadingData = false;
                UpdateDistributionMode();
                UpdateManualSlotCount();
            }
        }

        private void LoadExistingSpots()
        {
            try
            {
                _selectedSpotIDs.Clear();

                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.CampaignID == _campaign.ID && s.FileType == "SPOT")
                    .OrderBy(s => s.ScheduleDate)
                    .ThenBy(s => s.SlotTime)
                    .ToList();

                if (schedules.Count > 0)
                {
                    var uniqueSpotIDs = schedules
                        .Select(s => s.SpotID)
                        .Distinct()
                        .ToList();

                    _selectedSpotIDs.AddRange(uniqueSpotIDs);

                    Console.WriteLine($"[CampaignWizard] ✅ Caricati {_selectedSpotIDs.Count} spot dalla schedulazione");
                }
                else
                {
                    if (_campaign.SpotID > 0)
                    {
                        _selectedSpotIDs.Add(_campaign.SpotID);
                        Console.WriteLine($"[CampaignWizard] ⚠️ Usato SpotID principale: {_campaign.SpotID}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore LoadExistingSpots: {ex.Message}");

                if (_campaign.SpotID > 0 && !_selectedSpotIDs.Contains(_campaign.SpotID))
                {
                    _selectedSpotIDs.Add(_campaign.SpotID);
                }
            }
        }

        private void LoadExistingSchedule()
        {
            try
            {
                Console.WriteLine($"[CampaignWizard] 🔄 Caricamento schedulazione esistente per campagna ID: {_campaign.ID}");

                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.CampaignID == _campaign.ID && s.FileType == "SPOT")
                    .OrderBy(s => s.ScheduleDate)
                    .ThenBy(s => s.SlotTime)
                    .ToList();

                if (schedules.Count == 0)
                {
                    Console.WriteLine($"[CampaignWizard] ⚠️ Nessuna schedulazione trovata");
                    return;
                }

                var groupedByDate = schedules
                    .GroupBy(s => s.ScheduleDate.Date)
                    .OrderBy(g => g.Key)
                    .ToList();

                _dailySchedules = new List<ScheduleEngine.DailySchedule>();

                foreach (var group in groupedByDate)
                {
                    var dailySchedule = new ScheduleEngine.DailySchedule
                    {
                        Date = group.Key,
                        TimeSlots = group.Select(s => s.SlotTime).ToList(),
                        IsConfirmed = true,
                        IsModified = false
                    };

                    _dailySchedules.Add(dailySchedule);
                }

                Console.WriteLine($"[CampaignWizard] ✅ Caricati {_dailySchedules.Count} giorni");

                EnableStep3();
                Application.DoEvents();
                LoadScheduleGrid();
                dgvSchedule.Refresh();
                HideGenerateButton();

                BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        pnlStep3.Enabled = true;
                        dgvSchedule.Enabled = true;
                        lblStep3Title.BackColor = Color.FromArgb(40, 167, 69);
                        lblStep3Title.Text = LanguageManager.Get("CampaignWizard.Step3Active", "✅ STEP 3: Revisione e Modifica Schedulazione");
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"[CampaignWizard] ⚠️ Errore deferred Step3 update: {ex2.Message}");
                    }
                });

                Console.WriteLine($"[CampaignWizard] ✅ Schedulazione esistente caricata");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore LoadExistingSchedule: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.LoadScheduleError", "Errore caricamento schedulazione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            try
            {
                if (cmbClient.SelectedItem != null && cmbClient.SelectedItem is DbcManager.Client selectedClient)
                {
                    int clientID = selectedClient.ID;

                    var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc")
                        .Where(s => s.ClientID == clientID && s.IsActive)
                        .OrderBy(s => s.SpotTitle)
                        .ToList();

                    cmbSpotSelector.DataSource = null;
                    cmbSpotSelector.DataSource = spots;
                    cmbSpotSelector.DisplayMember = "SpotTitle";
                    cmbSpotSelector.ValueMember = "ID";
                    cmbSpotSelector.SelectedIndex = spots.Count > 0 ? 0 : -1;

                    Console.WriteLine($"[CampaignWizard] Cliente: {selectedClient.ClientName}, Spot: {spots.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore cmbClient_SelectedIndexChanged: {ex.Message}");
            }
        }

        private void btnAddSpot_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSpotSelector.SelectedItem != null && cmbSpotSelector.SelectedItem is DbcManager.Spot selectedSpot)
                {
                    int spotID = selectedSpot.ID;

                    if (_selectedSpotIDs.Contains(spotID))
                    {
                        MessageBox.Show(
                            LanguageManager.Get("CampaignWizard.SpotAlreadyAdded", "Questo spot è già nella lista! "),
                            LanguageManager.Get("Common.Warning", "Attenzione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    _selectedSpotIDs.Add(spotID);
                    UpdateSpotList();

                    Console.WriteLine($"[CampaignWizard] Aggiunto spot:  {selectedSpot.SpotTitle}");
                }
                else
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.SelectSpot", "Seleziona uno spot da aggiungere! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.AddSpotError", "Errore aggiunta spot")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRemoveSpot_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstSelectedSpots.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.SelectSpotToRemove", "Seleziona uno spot da rimuovere! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int index = lstSelectedSpots.SelectedIndex;
                _selectedSpotIDs.RemoveAt(index);
                UpdateSpotList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.RemoveSpotError", "Errore rimozione spot")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                int index = lstSelectedSpots.SelectedIndex;
                if (index <= 0) return;

                int temp = _selectedSpotIDs[index];
                _selectedSpotIDs[index] = _selectedSpotIDs[index - 1];
                _selectedSpotIDs[index - 1] = temp;

                UpdateSpotList();
                lstSelectedSpots.SelectedIndex = index - 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore MoveUp: {ex.Message}");
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                int index = lstSelectedSpots.SelectedIndex;
                if (index < 0 || index >= _selectedSpotIDs.Count - 1) return;

                int temp = _selectedSpotIDs[index];
                _selectedSpotIDs[index] = _selectedSpotIDs[index + 1];
                _selectedSpotIDs[index + 1] = temp;

                UpdateSpotList();
                lstSelectedSpots.SelectedIndex = index + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore MoveDown: {ex.Message}");
            }
        }

        private void UpdateSpotList()
        {
            lstSelectedSpots.Items.Clear();

            var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");

            for (int i = 0; i < _selectedSpotIDs.Count; i++)
            {
                var spot = allSpots.FirstOrDefault(s => s.ID == _selectedSpotIDs[i]);
                if (spot != null)
                {
                    lstSelectedSpots.Items.Add($"{i + 1}.{spot.SpotTitle} ({spot.Duration}s)");
                }
            }

            lblSpotCount.Text = string.Format(
                LanguageManager.Get("CampaignWizard.SpotCount", "Spot selezionati: {0}"),
                _selectedSpotIDs.Count
            );
        }

        private void LoadTimeSlots()
        {
            var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                .Where(t => t.StationID == _stationID && t.IsActive)
                .OrderBy(t => t.SlotTime)
                .ToList();

            flowManualSlots.Controls.Clear();

            foreach (var slot in timeSlots)
            {
                var chk = new CheckBox
                {
                    Text = FormatTimeSlot(slot.SlotTime),
                    Tag = slot.SlotTime,
                    AutoSize = true,
                    Margin = new Padding(5),
                    Font = new Font("Segoe UI", 9F)
                };
                chk.CheckedChanged += ManualSlotCheckBox_CheckedChanged;
                flowManualSlots.Controls.Add(chk);
            }
        }

        private void ManualSlotCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;

            if (sender is CheckBox chk && chk.Checked)
            {
                int max = (int)numManualDailyPasses.Value;
                int count = flowManualSlots.Controls.OfType<CheckBox>().Count(c => c.Checked);
                if (count > max)
                {
                    chk.Checked = false;
                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("CampaignWizard.MaxSlotsReached", "Puoi selezionare al massimo {0} punti orari (passaggi al giorno)!"),
                            max
                        ),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            UpdateManualSlotCount();
        }

        private void UpdateManualSlotCount()
        {
            int selectedCount = flowManualSlots.Controls.OfType<CheckBox>().Count(c => c.Checked);
            int max = (int)numManualDailyPasses.Value;

            if (lblManualSlotCount != null)
            {
                lblManualSlotCount.Text = string.Format(
                    LanguageManager.Get("CampaignWizard.ManualSlotCountFmt", "Slot selezionati: {0} / {1}"),
                    selectedCount, max
                );
            }
        }

        private void rdAutoBalanced_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            UpdateDistributionMode();
        }

        private void rdAutoAudience_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            UpdateDistributionMode();
        }

        private void rdManual_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            UpdateDistributionMode();
        }

        private void numManualDailyPasses_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            UpdateManualSlotCount();
        }

        private void UpdateDistributionMode()
        {
            bool isManual = rdManual.Checked;

            grpAutoConfig.Visible = rdAutoBalanced.Checked || rdAutoAudience.Checked;
            grpManualSlots.Visible = isManual;

            // Dynamically reposition grpDays and resize pnlStep2/pnlStep3 based on active config panel
            if (isManual)
            {
                // grpManualSlots bottom: 139 + 220 = 359, add 5px gap → 364
                grpDays.Location = new Point(49, 364);
                pnlStep2.Size = new Size(1070, 420);
                pnlStep3.Location = new Point(20, pnlStep2.Bottom + 15);
            }
            else
            {
                // grpAutoConfig bottom: 139 + 110 = 249, add 5px gap → 254
                grpDays.Location = new Point(49, 254);
                pnlStep2.Size = new Size(1070, 310);
                pnlStep3.Location = new Point(20, pnlStep2.Bottom + 15);
            }

            if (rdAutoBalanced.Checked)
            {
                _campaign.DistributionMode = "BALANCED";
            }
            else if (rdAutoAudience.Checked)
            {
                _campaign.DistributionMode = "AUDIENCE";
            }
            else if (rdManual.Checked)
            {
                _campaign.DistributionMode = "MANUAL";
            }
        }

        private void chkTimeFilter_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTimeFilterState();
        }

        private void UpdateTimeFilterState()
        {
            bool enabled = chkTimeFilter.Checked;
            lblTimeFrom.Enabled = enabled;
            dtpTimeFrom.Enabled = enabled;
            lblTimeTo.Enabled = enabled;
            dtpTimeTo.Enabled = enabled;
        }

        private void btnSelectAllSlots_Click(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            int max = (int)numManualDailyPasses.Value;
            int count = 0;
            foreach (Control ctrl in flowManualSlots.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.Checked = count < max;
                    if (chk.Checked) count++;
                }
            }
        }

        private void btnDeselectAllSlots_Click(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            foreach (Control ctrl in flowManualSlots.Controls)
            {
                if (ctrl is CheckBox chk)
                    chk.Checked = false;
            }
        }

        private bool ValidateStep2()
        {
            // Day-of-week validation applies to all modes
            if (!chkMonday.Checked && !chkTuesday.Checked && !chkWednesday.Checked &&
                !chkThursday.Checked && !chkFriday.Checked && !chkSaturday.Checked && !chkSunday.Checked)
            {
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ErrorDays", "Seleziona almeno un giorno della settimana!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep2.Top - 20);
                return false;
            }

            if (rdAutoBalanced.Checked || rdAutoAudience.Checked)
            {
                if (numDailyPasses.Value <= 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.ErrorDailyPasses", "Inserisci il numero di passaggi giornalieri! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep2.Top - 20);
                    return false;
                }

                if (chkTimeFilter.Checked)
                {
                    if (dtpTimeTo.Value <= dtpTimeFrom.Value)
                    {
                        MessageBox.Show(
                            LanguageManager.Get("CampaignWizard.ErrorTimeRange", "L'ora finale deve essere successiva all'ora iniziale!"),
                            LanguageManager.Get("Common.Warning", "Attenzione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep2.Top - 20);
                        return false;
                    }
                }
            }
            else if (rdManual.Checked)
            {
                bool hasSelected = false;
                foreach (Control ctrl in flowManualSlots.Controls)
                {
                    if (ctrl is CheckBox chk && chk.Checked)
                    {
                        hasSelected = true;
                        break;
                    }
                }

                if (!hasSelected)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.ErrorManualSlots", "Seleziona almeno un punto orario!"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    pnlMainContainer.AutoScrollPosition = new Point(0, pnlStep2.Top - 20);
                    return false;
                }
            }

            return true;
        }

        private void SaveStep2Data()
        {
            // Always save day-of-week flags regardless of distribution mode
            _campaign.Monday = chkMonday.Checked;
            _campaign.Tuesday = chkTuesday.Checked;
            _campaign.Wednesday = chkWednesday.Checked;
            _campaign.Thursday = chkThursday.Checked;
            _campaign.Friday = chkFriday.Checked;
            _campaign.Saturday = chkSaturday.Checked;
            _campaign.Sunday = chkSunday.Checked;

            if (rdManual.Checked)
            {
                var selectedSlots = new List<string>();
                foreach (Control ctrl in flowManualSlots.Controls)
                {
                    if (ctrl is CheckBox chk && chk.Checked)
                    {
                        selectedSlots.Add(chk.Tag.ToString());
                    }
                }
                _campaign.ManualSlots = string.Join(";", selectedSlots);
                _campaign.DailyPasses = (int)numManualDailyPasses.Value;
                _campaign.TimeFrom = "00:00:00";
                _campaign.TimeTo = "23:59:59";
            }
            else
            {
                _campaign.DailyPasses = (int)numDailyPasses.Value;

                if (chkTimeFilter.Checked)
                {
                    _campaign.TimeFrom = dtpTimeFrom.Value.ToString("HH:mm:ss");
                    _campaign.TimeTo = dtpTimeTo.Value.ToString("HH:mm:ss");
                }
                else
                {
                    _campaign.TimeFrom = "00:00:00";
                    _campaign.TimeTo = "23:59:59";
                }

                _campaign.ManualSlots = "";
            }

            _avoidRepetition = chkAvoidRepetition?.Checked ?? true;
            _distributePasses = chkDistributePasses?.Checked ?? true;
        }

        private void GenerateSchedule()
        {
            try
            {
                Console.WriteLine($"[CampaignWizard] 🔄 Inizio generazione schedulazione...");

                var scheduleEngine = new ScheduleEngine();
                var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                    .Where(t => t.StationID == _stationID && t.IsActive)
                    .ToList();

                Console.WriteLine($"[CampaignWizard] TimeSlots caricati: {timeSlots.Count}");

                if (_campaign.DistributionMode == "MANUAL")
                {
                    var selectedSlots = _campaign.ManualSlots.Split(';').ToList();
                    _dailySchedules = scheduleEngine.GenerateManual(_campaign, selectedSlots);
                }
                else
                {
                    _dailySchedules = scheduleEngine.GenerateAutomatic(
                        _campaign,
                        timeSlots,
                        _avoidRepetition,
                        _distributePasses
                    );
                }

                Console.WriteLine($"[CampaignWizard] ✅ Generati {_dailySchedules.Count} giorni di schedulazione");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore GenerateSchedule: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.GenerateError", "Errore generazione schedulazione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Metodi DataGridView (DgvSchedule_DataError, DgvSchedule_CellValueChanged, etc.)
        // restano identici a quelli forniti nel codice originale...
        // (omessi per brevità - mantieni quelli esistenti)

        private void LoadScheduleGrid()
        {
            if (_availableTimeSlots == null || _availableTimeSlots.Count == 0)
            {
                Console.WriteLine("[CampaignWizard] ⚠️ _availableTimeSlots vuoto!  Ricarico...");
                LoadAvailableTimeSlots();
            }

            if (_dailySchedules == null || dgvSchedule == null || _availableTimeSlots == null || _availableTimeSlots.Count == 0)
            {
                Console.WriteLine($"[CampaignWizard] ❌ LoadScheduleGrid fallito");
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.NoTimeSlotsError", "Errore:  nessun orario disponibile!\n\nVerifica la configurazione dei TimeSlots."),
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            try
            {
                dgvSchedule.SuspendLayout();
                dgvSchedule.Rows.Clear();

                DateTime today = DateTime.Today;

                Console.WriteLine($"[CampaignWizard] 📊 Caricamento griglia - {_dailySchedules.Count} giorni");

                for (int i = 3; i < dgvSchedule.Columns.Count; i++)
                {
                    if (dgvSchedule.Columns[i] is DataGridViewComboBoxColumn comboCol)
                    {
                        comboCol.ReadOnly = false;
                    }
                }

                foreach (var daily in _dailySchedules)
                {
                    string dateStr = daily.Date.ToString("dd/MM/yyyy");
                    string dayOfWeek = daily.Date.ToString("dddd");
                    int passCount = daily.TimeSlots.Count;

                    bool isPastDay = daily.Date.Date < today;

                    int rowIndex = dgvSchedule.Rows.Add();
                    var row = dgvSchedule.Rows[rowIndex];

                    row.Cells["Date"].Value = dateStr;
                    row.Cells["DayOfWeek"].Value = dayOfWeek;
                    row.Cells["PassCount"].Value = passCount;
                    row.Height = 25;

                    if (isPastDay)
                    {
                        row.Cells["Date"].Style.BackColor = Color.FromArgb(200, 200, 200);
                        row.Cells["Date"].Style.ForeColor = Color.DarkGray;
                        row.Cells["DayOfWeek"].Style.BackColor = Color.FromArgb(200, 200, 200);
                        row.Cells["DayOfWeek"].Style.ForeColor = Color.DarkGray;
                        row.Cells["PassCount"].Style.BackColor = Color.FromArgb(200, 200, 200);
                        row.Cells["PassCount"].Style.ForeColor = Color.DarkGray;
                    }

                    for (int i = 0; i < 15; i++)
                    {
                        var comboCell = row.Cells[$"Slot{i + 1}"] as DataGridViewComboBoxCell;

                        if (comboCell != null)
                        {
                            if (i < daily.TimeSlots.Count)
                            {
                                var slotDataSource = new List<string>(_availableTimeSlots);

                                string formattedValue = FormatTimeSlot(daily.TimeSlots[i]);

                                // Ensure the stored slot value is present in the DataSource
                                if (!slotDataSource.Contains(formattedValue) && !string.IsNullOrEmpty(formattedValue))
                                {
                                    slotDataSource.Add(formattedValue);
                                    slotDataSource.Sort();
                                }

                                comboCell.DataSource = slotDataSource;
                                comboCell.Value = formattedValue;

                                if (isPastDay)
                                {
                                    comboCell.ReadOnly = true;
                                    comboCell.Style.BackColor = Color.FromArgb(200, 200, 200);
                                    comboCell.Style.ForeColor = Color.DarkGray;
                                }
                                else
                                {
                                    comboCell.ReadOnly = false;
                                    comboCell.Style.BackColor = Color.White;
                                    comboCell.Style.ForeColor = Color.Black;
                                }
                            }
                            else
                            {
                                comboCell.DataSource = new List<string> { "-" };
                                comboCell.Value = "-";
                                comboCell.ReadOnly = true;
                                comboCell.Style.BackColor = Color.FromArgb(230, 230, 230);
                            }
                        }
                    }
                }

                dgvSchedule.ResumeLayout();
                dgvSchedule.ClearSelection();
                UpdateProgressBar();

                Console.WriteLine($"[CampaignWizard] ✅ Griglia caricata - {dgvSchedule.Rows.Count} righe");
            }
            catch (Exception ex)
            {
                dgvSchedule.ResumeLayout();
                Console.WriteLine($"[CampaignWizard] ❌ Errore LoadScheduleGrid: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("CampaignWizard.LoadGridError", "Errore caricamento griglia")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void UpdateProgressBar()
        {
            if (_dailySchedules == null || progressDays == null)
                return;

            int total = _dailySchedules.Count;
            int modified = _dailySchedules.Count(d => d.IsModified);
            int pastDays = _dailySchedules.Count(d => d.Date.Date < DateTime.Today);
            int futureDays = total - pastDays;

            progressDays.Maximum = total;
            progressDays.Value = total;

            lblProgress.Text = string.Format(
                LanguageManager.Get("CampaignWizard.ProgressLabel", "Giorni totali: {0} | 🔒 Passati: {1} | ✏️ Modificabili: {2} | ✨ Modificati: {3}"),
                total, pastDays, futureDays, modified
            );
        }

        private void btnConfirmAll_Click(object sender, EventArgs e)
        {
            if (_dailySchedules == null)
                return;

            var result = MessageBox.Show(
                LanguageManager.Get("CampaignWizard.ConfirmReset", "Vuoi resettare tutte le modifiche manuali?"),
                LanguageManager.Get("CampaignWizard.ResetTitle", "Reset Modifiche"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                foreach (var day in _dailySchedules)
                {
                    day.IsModified = false;
                }

                GenerateSchedule();
                LoadScheduleGrid();

                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.ResetSuccess", "✅ Schedulazione resettata ai valori originali!"),
                    LanguageManager.Get("Common.Success", "Completato"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateStep1())
                    return;

                if (!ValidateStep2())
                    return;

                if (!_scheduleGenerated || _dailySchedules == null || _dailySchedules.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.GenerateFirst", "Prima di salvare devi generare la schedulazione!\n\nClicca sul bottone '📊 Genera Schedulazione' in basso."),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int modified = _dailySchedules.Count(d => d.IsModified);
                if (modified > 0)
                {
                    var result = MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("CampaignWizard.ConfirmModified", "Hai modificato manualmente {0} giorni.\n\nVuoi salvare con le modifiche?"),
                            modified
                        ),
                        LanguageManager.Get("CampaignWizard.ConfirmModifiedTitle", "Conferma Modifiche"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.No)
                        return;
                }

                _campaign.CreatedDate = DateTime.Now;

                bool campaignSaved;
                if (_campaign.ID > 0)
                {
                    campaignSaved = DbcManager.Update("ADV_Campaigns.dbc", _campaign);
                }
                else
                {
                    campaignSaved = DbcManager.Insert("ADV_Campaigns.dbc", _campaign);
                }

                if (!campaignSaved)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.SaveCampaignError", "Errore salvataggio campagna! "),
                        LanguageManager.Get("Common.Error", "Errore"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var savedCampaign = campaigns.OrderByDescending(c => c.ID).FirstOrDefault();

                if (savedCampaign == null)
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.RecoverIDError", "Errore recupero ID campagna!"),
                        LanguageManager.Get("Common.Error", "Errore"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                var allSpots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
                var selectedSpots = allSpots.Where(s => _selectedSpotIDs.Contains(s.ID)).ToList();

                var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                    .Where(t => t.StationID == _stationID)
                    .ToList();

                bool scheduleSaved = SaveScheduleWithRotation(
                    _stationID,
                    _dailySchedules,
                    savedCampaign,
                    selectedSpots,
                    timeSlots
                );

                if (scheduleSaved)
                {
                    try
                    {
                        bool exported = AirDirectorExportService.ExportFullSchedule(
                            _stationID,
                            _campaign.StartDate,
                            _campaign.EndDate.AddDays(7)
                        );

                        if (exported)
                        {
                            Console.WriteLine("[CampaignWizard] ✅ Export AirDirector completato");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CampaignWizard] ⚠️ Errore export: {ex.Message}");
                    }

                    int totalPasses = _dailySchedules.Sum(d => d.TimeSlots.Count);
                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("CampaignWizard.SaveSuccess",
                                "✅ Campagna '{0}' salvata con successo!\n\n" +
                                "Giorni schedulati: {1}\n" +
                                "Spot in rotazione: {2}\n" +
                                "Passaggi/giorno: {3}\n" +
                                "Fascia oraria: {4} - {5}\n" +
                                "Totale passaggi: {6}\n" +
                                "Giorni modificati: {7}"),
                            _campaign.CampaignName,
                            _dailySchedules.Count,
                            _selectedSpotIDs.Count,
                            _campaign.DailyPasses,
                            _campaign.TimeFrom,
                            _campaign.TimeTo,
                            totalPasses,
                            modified
                        ),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    if (MessageBox.Show(
                            LanguageManager.Get("CampaignWizard.AskPDFReport", "Vuoi generare il report orari in formato PDF?"),
                            LanguageManager.Get("CampaignWizard.AskPDFReportTitle", "Report Orari"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        ) == DialogResult.Yes)
                    {
                        ShowPDFPreview(savedCampaign, selectedSpots);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        LanguageManager.Get("CampaignWizard.SaveScheduleError", "Errore salvataggio schedulazione!"),
                        LanguageManager.Get("Common.Error", "Errore"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Common.Error", "Errore")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowPDFPreview(DbcManager.Campaign campaign, List<DbcManager.Spot> spots)
        {
            try
            {
                var client = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc")
                    .FirstOrDefault(c => c.ID == campaign.ClientID);

                var pdfForm = new CampaignPDFPreviewForm(campaign, _dailySchedules, spots, client);
                pdfForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] Errore apertura PDF: {ex.Message}");
                MessageBox.Show(
                    LanguageManager.Get("CampaignWizard.PDFError", "Campagna salvata, ma impossibile generare PDF."),
                    LanguageManager.Get("Common.Warning", "Avviso"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private bool SaveScheduleWithRotation(
            int stationID,
            List<ScheduleEngine.DailySchedule> dailySchedules,
            DbcManager.Campaign campaign,
            List<DbcManager.Spot> spots,
            List<DbcManager.TimeSlot> timeSlots)
        {
            try
            {
                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");

                if (campaign.ID > 0)
                {
                    Console.WriteLine($"[CampaignWizard] 🔄 MODIFICA CAMPAGNA ESISTENTE ID: {campaign.ID}");

                    DateTime today = DateTime.Today;
                    int removedCount = allSchedules.RemoveAll(s =>
                        s.CampaignID == campaign.ID &&
                        s.ScheduleDate.Date >= today
                    );

                    Console.WriteLine($"[CampaignWizard] ✅ Rimossi {removedCount} record futuri");
                }

                int spotRotationIndex = 0;
                int addedCount = 0;

                Console.WriteLine($"[CampaignWizard] 📊 Salvataggio schedulazione con {spots.Count} spot");

                foreach (var daily in dailySchedules)
                {
                    if (daily.Date.Date < DateTime.Today && campaign.ID > 0)
                    {
                        Console.WriteLine($"[CampaignWizard] ⏭️ Saltato giorno passato:  {daily.Date:dd/MM/yyyy}");
                        continue;
                    }

                    foreach (var slotTime in daily.TimeSlots)
                    {
                        string fullSlotTime = ConvertToFullTime(slotTime);

                        var timeSlot = timeSlots.FirstOrDefault(ts =>
                            ts.SlotTime == fullSlotTime ||
                            FormatTimeSlot(ts.SlotTime) == slotTime);

                        if (timeSlot == null)
                        {
                            Console.WriteLine($"[CampaignWizard] ⚠️ TimeSlot non trovato: {slotTime}");
                            continue;
                        }

                        int sequenceOrder = 1;

                        if (!string.IsNullOrEmpty(timeSlot.OpeningFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = timeSlot.SlotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "OPENING",
                                FilePath = timeSlot.OpeningFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = campaign.ID,
                                Duration = 5,
                                IsManual = daily.IsModified
                            });
                        }

                        var currentSpot = spots[spotRotationIndex % spots.Count];
                        allSchedules.Add(new DbcManager.Schedule
                        {
                            StationID = stationID,
                            ScheduleDate = daily.Date,
                            SlotTime = timeSlot.SlotTime,
                            SequenceOrder = sequenceOrder++,
                            FileType = "SPOT",
                            FilePath = currentSpot.FilePath,
                            ClientID = campaign.ClientID,
                            SpotID = currentSpot.ID,
                            CampaignID = campaign.ID,
                            Duration = currentSpot.Duration,
                            IsManual = daily.IsModified
                        });

                        spotRotationIndex++;
                        addedCount++;

                        if (!string.IsNullOrEmpty(timeSlot.InfraSpotFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = timeSlot.SlotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "INFRASPOT",
                                FilePath = timeSlot.InfraSpotFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = campaign.ID,
                                Duration = 3,
                                IsManual = daily.IsModified
                            });
                        }

                        if (!string.IsNullOrEmpty(timeSlot.ClosingFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = timeSlot.SlotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "CLOSING",
                                FilePath = timeSlot.ClosingFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = campaign.ID,
                                Duration = 5,
                                IsManual = daily.IsModified
                            });
                        }
                    }
                }

                bool success = DbcManager.Save("ADV_Schedule.dbc", allSchedules);

                Console.WriteLine($"[CampaignWizard] ✅ Schedulazione salvata:  {success}");
                Console.WriteLine($"[CampaignWizard]   - Passaggi aggiunti: {addedCount}");

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CampaignWizard] ❌ Errore SaveScheduleWithRotation: {ex.Message}");
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                LanguageManager.Get("CampaignWizard.ConfirmCancel", "Sei sicuro di voler annullare?"),
                LanguageManager.Get("CampaignWizard.CancelTitle", "Conferma Annullamento"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        // ✅ Aggiungi i metodi DataGridView mancanti (DataError, CellValueChanged, etc.)
        // dal codice originale che hai fornito...

        private void DgvSchedule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine($"[CampaignWizard] DataError - Row: {e.RowIndex}, Col: {e.ColumnIndex}, Error: {e.Exception?.Message}");
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void DgvSchedule_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            if (dgvSchedule.IsCurrentCellDirty && dgvSchedule.CurrentCell is DataGridViewComboBoxCell)
            {
                dgvSchedule.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvSchedule_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoadingData) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 3)
                return;

            if (_dailySchedules != null && e.RowIndex < _dailySchedules.Count)
            {
                var day = _dailySchedules[e.RowIndex];

                if (day.Date.Date < DateTime.Today)
                {
                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("CampaignWizard.PastDayError", "Non puoi modificare un giorno passato!\n\nData: {0}"),
                            day.Date.ToString("dd/MM/yyyy")
                        ),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    BeginInvoke((MethodInvoker)delegate { LoadScheduleGrid(); });
                    return;
                }

                var cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                int slotIndex = e.ColumnIndex - 3;

                if (slotIndex < day.TimeSlots.Count && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    string newTime = cell.Value.ToString();
                    string oldTime = FormatTimeSlot(day.TimeSlots[slotIndex]);

                    if (newTime != oldTime)
                    {
                        day.TimeSlots[slotIndex] = ConvertToFullTime(newTime);
                        day.IsModified = true;
                        UpdateProgressBar();
                    }
                }
            }
        }

        private void DgvSchedule_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (_dailySchedules != null && e.RowIndex < _dailySchedules.Count)
            {
                var day = _dailySchedules[e.RowIndex];
                var cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (day.Date.Date < DateTime.Today && e.ColumnIndex >= 3)
                {
                    cell.ToolTipText = string.Format(
                        LanguageManager.Get("CampaignWizard.PastDayTooltip", "⛔ Giorno passato ({0})\nNon modificabile"),
                        day.Date.ToString("dd/MM/yyyy")
                    );
                }
                else if (e.ColumnIndex >= 3 && e.ColumnIndex < 3 + day.TimeSlots.Count)
                {
                    cell.ToolTipText = string.Format(
                        LanguageManager.Get("CampaignWizard.EditTooltip", "✏️ Clicca per modificare\nData: {0}"),
                        day.Date.ToString("dd/MM/yyyy")
                    );
                }
                else
                {
                    cell.ToolTipText = "";
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }

        private void pnlStep3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grpPeriod_Enter(object sender, EventArgs e)
        {

        }
    }
}