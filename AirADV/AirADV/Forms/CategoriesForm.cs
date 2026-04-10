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
    public partial class CategoriesForm : Form
    {
        private List<DbcManager.Category> _categories = new List<DbcManager.Category>();
        private List<DbcManager.Category> _allCategories = new List<DbcManager.Category>(); // ✅ NUOVO: Lista completa
        private bool _isDirty = false;
        private static readonly string CATEGORIES_LANG_PATH = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Resources",
            "Languages",
            "Categories"
        );

        public CategoriesForm()
        {
            InitializeComponent();
            this.Load += CategoriesForm_Load;
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetupDataGridView();
                SetupSearchBox(); // ✅ NUOVO
                ApplyLanguage();
                LoadCategories();

                CleanOrphanSchedules();

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
            Console.WriteLine("[CategoriesForm] 🔄 Cambio lingua rilevato");
            ApplyLanguage();
        }

        /// <summary>
        /// ✅ NUOVO: Setup campo ricerca
        /// </summary>
        private void SetupSearchBox()
        {
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Imposta placeholder (se non hai il controllo nativo)
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = LanguageManager.Get("Categories.SearchPlaceholder", "🔍 Cerca categoria...");

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.ForeColor == Color.Gray)
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.ForeColor = Color.Gray;
                    txtSearch.Text = LanguageManager.Get("Categories.SearchPlaceholder", "🔍 Cerca categoria...");
                }
            };
        }

        /// <summary>
        /// ✅ NUOVO:  Gestisce ricerca in tempo reale
        /// </summary>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Ignora se è il placeholder
                if (txtSearch.ForeColor == Color.Gray)
                    return;

                string searchText = txtSearch.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchText))
                {
                    // Mostra tutte le categorie
                    _categories = _allCategories.ToList();
                }
                else
                {
                    // Filtra per codice O nome
                    _categories = _allCategories
                        .Where(c =>
                            c.CategoryCode.ToLower().Contains(searchText) ||
                            c.CategoryName.ToLower().Contains(searchText)
                        )
                        .ToList();
                }

                // Aggiorna grid
                dgvCategories.DataSource = null;
                dgvCategories.DataSource = _categories;

                UpdateStatusLabel();

                Console.WriteLine($"[CategoriesForm] 🔍 Ricerca '{searchText}':  {_categories.Count} risultati");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore ricerca: {ex.Message}");
            }
        }

        private void SetupDataGridView()
        {
            dgvCategories.AutoGenerateColumns = false;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.MultiSelect = false;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.Fixed3D;
            dgvCategories.RowTemplate.Height = 30;

            var colCode = new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "Codice",
                DataPropertyName = "CategoryCode",
                Width = 150,
                ReadOnly = false
            };
            dgvCategories.Columns.Add(colCode);

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Nome Categoria",
                DataPropertyName = "CategoryName",
                Width = 300,
                ReadOnly = false
            };
            dgvCategories.Columns.Add(colName);

            var colColorDisplay = new DataGridViewTextBoxColumn
            {
                Name = "colColorDisplay",
                HeaderText = "Colore",
                Width = 200,
                ReadOnly = true
            };
            dgvCategories.Columns.Add(colColorDisplay);

            var colColorBtn = new DataGridViewButtonColumn
            {
                Name = "colColorBtn",
                HeaderText = "",
                Text = "🎨",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dgvCategories.Columns.Add(colColorBtn);

            var colActive = new DataGridViewCheckBoxColumn
            {
                Name = "colActive",
                HeaderText = "Attivo",
                DataPropertyName = "IsActive",
                Width = 80
            };
            dgvCategories.Columns.Add(colActive);

            dgvCategories.CellValueChanged += (s, e) => _isDirty = true;
            dgvCategories.CellContentClick += DgvCategories_CellContentClick;
            dgvCategories.CellPainting += DgvCategories_CellPainting;
        }

        private void DgvCategories_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dgvCategories.Columns["colColorDisplay"].Index && e.RowIndex >= 0)
            {
                if (e.RowIndex < _categories.Count)
                {
                    var category = _categories[e.RowIndex];
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    try
                    {
                        Color categoryColor = ColorTranslator.FromHtml(category.Color);
                        using (SolidBrush brush = new SolidBrush(categoryColor))
                        {
                            Rectangle colorRect = new Rectangle(
                                e.CellBounds.X + 5,
                                e.CellBounds.Y + 5,
                                e.CellBounds.Width - 10,
                                e.CellBounds.Height - 10
                            );
                            e.Graphics.FillRectangle(brush, colorRect);
                            e.Graphics.DrawRectangle(Pens.Black, colorRect);
                        }

                        using (SolidBrush textBrush = new SolidBrush(e.CellStyle.ForeColor))
                        {
                            StringFormat sf = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            e.Graphics.DrawString(category.Color, e.CellStyle.Font, textBrush, e.CellBounds, sf);
                        }
                    }
                    catch { }

                    e.Handled = true;
                }
            }
        }

        private void DgvCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _categories.Count)
                return;

            if (dgvCategories.Columns[e.ColumnIndex].Name == "colColorBtn")
            {
                var category = _categories[e.RowIndex];
                ChangeColor(category);
            }
        }

        private void ChangeColor(DbcManager.Category category)
        {
            using (var colorDialog = new ColorDialog())
            {
                try
                {
                    colorDialog.Color = ColorTranslator.FromHtml(category.Color);
                }
                catch
                {
                    colorDialog.Color = Color.Gray;
                }

                colorDialog.FullOpen = true;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    category.Color = ColorTranslator.ToHtml(colorDialog.Color);
                    _isDirty = true;
                    dgvCategories.Invalidate();
                }
            }
        }

        private void LoadCategories()
        {
            try
            {
                _allCategories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc")
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                if (_allCategories.Count == 0)
                {
                    InitializeDefaultCategories();
                }

                // ✅ Copia in _categories (lista filtrata)
                _categories = _allCategories.ToList();

                dgvCategories.DataSource = null;
                dgvCategories.DataSource = _categories;

                _isDirty = false;
                UpdateStatusLabel();

                Console.WriteLine($"[CategoriesForm] Caricate {_categories.Count} categorie");
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

        private void InitializeDefaultCategories()
        {
            try
            {
                if (!Directory.Exists(CATEGORIES_LANG_PATH))
                {
                    Directory.CreateDirectory(CATEGORIES_LANG_PATH);
                    Console.WriteLine($"[CategoriesForm] Cartella creata: {CATEGORIES_LANG_PATH}");
                }

                var languageFiles = Directory.GetFiles(CATEGORIES_LANG_PATH, "*.ini")
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();

                if (languageFiles.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("Categories.NoLanguageFiles",
                            "Nessun file di lingua trovato in:\n\n" +
                            $"{CATEGORIES_LANG_PATH}\n\n" +
                            "Crea almeno un file (es:  Italiano.ini) con il formato:\n" +
                            "Codice,Nome Categoria,Colore,Attivo"),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                string selectedLanguage = ShowLanguageSelectionDialog(languageFiles);

                if (string.IsNullOrEmpty(selectedLanguage))
                {
                    Console.WriteLine("[CategoriesForm] Importazione annullata dall'utente");
                    return;
                }

                string filePath = Path.Combine(CATEGORIES_LANG_PATH, $"{selectedLanguage}.ini");
                _allCategories = ImportCategoriesFromFile(filePath);

                if (_allCategories.Count > 0)
                {
                    bool saved = DbcManager.Save("ADV_Categories.dbc", _allCategories);

                    if (saved)
                    {
                        Console.WriteLine($"[CategoriesForm] ✅ Importate e salvate {_allCategories.Count} categorie da {selectedLanguage}.ini");

                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.ImportSuccess",
                                    "✅ Importazione completata!\n\n" +
                                    "{0} categorie merceologiche importate da:\n{1}"),
                                _allCategories.Count,
                                selectedLanguage
                            ),
                            LanguageManager.Get("Categories.Initialization", "Inizializzazione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        _isDirty = false;
                    }
                    else
                    {
                        throw new Exception("Errore durante il salvataggio delle categorie");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore InitializeDefaultCategories: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("Categories.ImportError", "Errore importazione categorie")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                _isDirty = true;
            }
        }

        private string ShowLanguageSelectionDialog(List<string> languageFiles)
        {
            using (var dialog = new Form())
            {
                dialog.Text = LanguageManager.Get("Categories.SelectLanguageTitle", "Seleziona Lingua Categorie");
                dialog.Size = new Size(400, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                var lblPrompt = new Label
                {
                    Text = LanguageManager.Get("Categories.SelectLanguagePrompt",
                        "Seleziona la lingua per importare le categorie merceologiche: "),
                    Location = new Point(20, 20),
                    Size = new Size(350, 40),
                    Font = new Font("Segoe UI", 10F)
                };

                var listBox = new ListBox
                {
                    Location = new Point(20, 70),
                    Size = new Size(350, 100),
                    Font = new Font("Segoe UI", 10F)
                };

                foreach (var lang in languageFiles.OrderBy(l => l))
                {
                    listBox.Items.Add(lang);
                }

                if (listBox.Items.Count > 0)
                    listBox.SelectedIndex = 0;

                var btnOK = new Button
                {
                    Text = LanguageManager.Get("Common.OK", "OK"),
                    DialogResult = DialogResult.OK,
                    Location = new Point(295, 180),
                    Size = new Size(75, 30),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var btnCancel = new Button
                {
                    Text = LanguageManager.Get("Common.Cancel", "Annulla"),
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(210, 180),
                    Size = new Size(75, 30)
                };

                dialog.Controls.Add(lblPrompt);
                dialog.Controls.Add(listBox);
                dialog.Controls.Add(btnOK);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                {
                    return listBox.SelectedItem.ToString();
                }

                return null;
            }
        }

        private List<DbcManager.Category> ImportCategoriesFromFile(string filePath)
        {
            var categories = new List<DbcManager.Category>();

            try
            {
                Console.WriteLine($"[CategoriesForm] 📂 Importazione da:  {filePath}");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File non trovato: {filePath}");
                }

                var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
                int lineNumber = 0;
                int id = 1;

                foreach (var line in lines)
                {
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#") || line.TrimStart().StartsWith(";"))
                        continue;

                    try
                    {
                        var parts = line.Split(',');

                        if (parts.Length < 2)
                        {
                            Console.WriteLine($"[CategoriesForm] ⚠️ Riga {lineNumber} ignorata (formato non valido): {line}");
                            continue;
                        }

                        string code = parts[0].Trim();
                        string name = parts[1].Trim();
                        string color = parts.Length > 2 ? parts[2].Trim() : "#808080";
                        bool isActive = parts.Length > 3 ? parts[3].Trim().Equals("true", StringComparison.OrdinalIgnoreCase) : true;

                        if (code.Length > 4)
                        {
                            Console.WriteLine($"[CategoriesForm] ⚠️ Riga {lineNumber}:  Codice troncato a 4 caratteri: {code}");
                            code = code.Substring(0, 4);
                        }

                        if (!color.StartsWith("#"))
                            color = "#808080";

                        var category = new DbcManager.Category
                        {
                            ID = id++,
                            CategoryCode = code,
                            CategoryName = name,
                            Color = color,
                            IsActive = isActive
                        };

                        categories.Add(category);

                        Console.WriteLine($"[CategoriesForm]   ✅ {code} - {name} - {color}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CategoriesForm] ❌ Errore riga {lineNumber}: {ex.Message}");
                    }
                }

                Console.WriteLine($"[CategoriesForm] ✅ Importate {categories.Count} categorie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore ImportCategoriesFromFile: {ex.Message}");
                throw;
            }

            return categories;
        }

        private void UpdateStatusLabel()
        {
            // ✅ Mostra risultati filtrati vs totale
            if (_categories.Count < _allCategories.Count)
            {
                lblStatus.Text = string.Format(
                    LanguageManager.Get("Categories.StatusLabelFiltered", "Visualizzate:  {0} / {1} categorie"),
                    _categories.Count,
                    _allCategories.Count
                );
            }
            else
            {
                lblStatus.Text = string.Format(
                    LanguageManager.Get("Categories.StatusLabel", "Categorie configurate: {0}"),
                    _categories.Count
                );
            }
        }

        private void ApplyLanguage()
        {
            try
            {
                Console.WriteLine($"[CategoriesForm] 🌐 Applicazione traduzioni (lingua: {LanguageManager.CurrentCulture})");

                this.Text = LanguageManager.Get("Categories.WindowTitle", "🏷️ Categorie Merceologiche");
                lblTitle.Text = LanguageManager.Get("Categories.Title", "🏷️ CATEGORIE MERCEOLOGICHE");

                btnAdd.Text = LanguageManager.Get("Categories.BtnAdd", "➕ Aggiungi Categoria");
                btnDelete.Text = LanguageManager.Get("Categories.BtnDelete", "🗑️ Elimina");
                btnSave.Text = LanguageManager.Get("Common.Save", "💾 Salva");
                btnRefresh.Text = LanguageManager.Get("Common.Refresh", "🔄 Aggiorna");

                // ✅ Aggiorna placeholder ricerca
                if (txtSearch.ForeColor == Color.Gray)
                {
                    txtSearch.Text = LanguageManager.Get("Categories.SearchPlaceholder", "🔍 Cerca categoria...");
                }

                if (dgvCategories.Columns.Count > 0)
                {
                    dgvCategories.Columns["colCode"].HeaderText = LanguageManager.Get("Categories.ColCode", "Codice");
                    dgvCategories.Columns["colName"].HeaderText = LanguageManager.Get("Categories.ColName", "Nome Categoria");
                    dgvCategories.Columns["colColorDisplay"].HeaderText = LanguageManager.Get("Categories.ColColor", "Colore");
                    dgvCategories.Columns["colActive"].HeaderText = LanguageManager.Get("Categories.ColActive", "Attivo");
                }

                UpdateStatusLabel();

                Console.WriteLine("[CategoriesForm] ✅ Traduzioni applicate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore ApplyLanguage:  {ex.Message}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newCategory = new DbcManager.Category
                {
                    CategoryCode = "",
                    CategoryName = "",
                    Color = "#808080",
                    IsActive = true
                };

                _allCategories.Add(newCategory); // ✅ Aggiungi a lista completa
                _categories.Add(newCategory);

                dgvCategories.DataSource = null;
                dgvCategories.DataSource = _categories;

                _isDirty = true;
                UpdateStatusLabel();

                dgvCategories.ClearSelection();
                dgvCategories.Rows[dgvCategories.Rows.Count - 1].Selected = true;
                dgvCategories.CurrentCell = dgvCategories.Rows[dgvCategories.Rows.Count - 1].Cells[0];
                dgvCategories.BeginEdit(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Categories.AddError", "Errore aggiunta categoria")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCategories.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.Get("Categories.SelectToDelete", "Seleziona una categoria da eliminare! "),
                        LanguageManager.Get("Common.Warning", "Attenzione"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var category = _categories[dgvCategories.SelectedRows[0].Index];

                Console.WriteLine($"[CategoriesForm] 🔍 Verifica uso categoria ID: {category.ID}");

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc")
                    .Where(c => c.CategoryID == category.ID)
                    .ToList();

                if (campaigns.Count > 0)
                {
                    var campaignList = string.Join("\n", campaigns.Take(5).Select(c => $"• {c.CampaignName}"));
                    if (campaigns.Count > 5)
                        campaignList += $"\n...{LanguageManager.Get("Categories.AndOthers", "e altre")} {campaigns.Count - 5}";

                    var result = MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("Categories.ConfirmDeleteWithCampaigns",
                                "⚠️ ATTENZIONE!\n\nLa categoria '{0}' è utilizzata in {1} campagna/e:\n\n{2}\n\n" +
                                "Vuoi eliminare la categoria E TUTTE LE CAMPAGNE associate?\n\n" +
                                "⚠️ Verranno eliminati anche TUTTI i passaggi programmati!\n\n" +
                                "Questa operazione è IRREVERSIBILE! "),
                            category.CategoryName,
                            campaigns.Count,
                            campaignList
                        ),
                        LanguageManager.Get("Messages.ConfirmDeleteTitle", "Conferma Eliminazione"),
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Yes)
                    {
                        DeleteCategoryWithCampaigns(category, campaigns);
                    }
                }
                else
                {
                    var result = MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("Categories.ConfirmDelete", "Sei sicuro di voler eliminare la categoria:\n\n{0}? "),
                            category.CategoryName
                        ),
                        LanguageManager.Get("Messages.ConfirmDeleteTitle", "Conferma Eliminazione"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        _allCategories.Remove(category); // ✅ Rimuovi da entrambe
                        _categories.Remove(category);

                        dgvCategories.DataSource = null;
                        dgvCategories.DataSource = _categories;

                        _isDirty = true;
                        UpdateStatusLabel();

                        Console.WriteLine($"[CategoriesForm] ✅ Categoria eliminata (nessuna campagna associata)");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Get("Categories.DeleteError", "Errore eliminazione")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DeleteCategoryWithCampaigns(DbcManager.Category category, List<DbcManager.Campaign> campaigns)
        {
            try
            {
                Console.WriteLine($"[CategoriesForm] 🗑️ Inizio eliminazione categoria '{category.CategoryName}' con {campaigns.Count} campagne");

                int totalSchedulesRemoved = 0;

                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");

                foreach (var campaign in campaigns)
                {
                    int removed = allSchedules.RemoveAll(s => s.CampaignID == campaign.ID);
                    totalSchedulesRemoved += removed;
                    Console.WriteLine($"[CategoriesForm]   - Campagna '{campaign.CampaignName}':  rimossi {removed} schedule");
                }

                bool scheduleSaved = DbcManager.Save("ADV_Schedule.dbc", allSchedules);
                Console.WriteLine($"[CategoriesForm] ✅ Rimossi {totalSchedulesRemoved} schedule totali da {campaigns.Count} campagne");

                var allCampaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");

                foreach (var campaign in campaigns)
                {
                    allCampaigns.RemoveAll(c => c.ID == campaign.ID);
                }

                bool campaignsSaved = DbcManager.Save("ADV_Campaigns.dbc", allCampaigns);
                Console.WriteLine($"[CategoriesForm] ✅ Rimosse {campaigns.Count} campagne");

                _allCategories.Remove(category); // ✅ Rimuovi da entrambe
                _categories.Remove(category);

                dgvCategories.DataSource = null;
                dgvCategories.DataSource = _categories;
                _isDirty = true;

                Console.WriteLine($"[CategoriesForm] ✅ Rimossa categoria '{category.CategoryName}'");

                if (scheduleSaved && campaignsSaved)
                {
                    MessageBox.Show(
                        string.Format(
                            LanguageManager.Get("Categories.DeleteSuccess",
                                "✅ Eliminazione completata!\n\nCategoria:  {0}\nCampagne eliminate: {1}\nPassaggi eliminati: {2}\n\nIl palinsesto è stato aggiornato."),
                            category.CategoryName,
                            campaigns.Count,
                            totalSchedulesRemoved
                        ),
                        LanguageManager.Get("Categories.DeleteCompleted", "Eliminazione Completata"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    try
                    {
                        int stationID = campaigns.FirstOrDefault()?.StationID ?? 1;
                        AirDirectorExportService.ExportFullSchedule(
                            stationID,
                            DateTime.Today,
                            DateTime.Today.AddMonths(3)
                        );
                        Console.WriteLine("[CategoriesForm] ✅ AirDirector export aggiornato");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CategoriesForm] ⚠️ Errore export: {ex.Message}");
                    }

                    UpdateStatusLabel();
                }
                else
                {
                    MessageBox.Show(
                        LanguageManager.Get("Categories.DeleteError", "Errore durante l'eliminazione! "),
                        LanguageManager.Get("Common.Error", "Errore"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore DeleteCategoryWithCampaigns:  {ex.Message}");
                throw;
            }
        }

        private void CleanOrphanSchedules()
        {
            try
            {
                Console.WriteLine("[CategoriesForm] 🧹 Inizio pulizia schedule orfani...");

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var validCampaignIDs = campaigns.Select(c => c.ID).ToHashSet();

                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");
                int countBefore = allSchedules.Count;

                int removed = allSchedules.RemoveAll(s =>
                    s.CampaignID > 0 &&
                    !validCampaignIDs.Contains(s.CampaignID)
                );

                if (removed > 0)
                {
                    bool saved = DbcManager.Save("ADV_Schedule.dbc", allSchedules);

                    if (saved)
                    {
                        Console.WriteLine($"[CategoriesForm] ✅ Puliti {removed} schedule orfani");
                        Console.WriteLine($"[CategoriesForm]   Record prima: {countBefore}, dopo: {allSchedules.Count}");

                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.CleanOrphansSuccess", "🧹 Pulizia automatica completata!\n\nRimossi {0} passaggi orfani da campagne eliminate."),
                                removed
                            ),
                            LanguageManager.Get("Categories.CleanDatabase", "Pulizia Database"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
                else
                {
                    Console.WriteLine("[CategoriesForm] ✅ Nessuno schedule orfano trovato");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoriesForm] ❌ Errore CleanOrphanSchedules: {ex.Message}");
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

                // ✅ Valida lista completa
                for (int i = 0; i < _allCategories.Count; i++)
                {
                    var category = _allCategories[i];

                    if (string.IsNullOrWhiteSpace(category.CategoryCode))
                    {
                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.CodeMissing", "Codice categoria mancante alla riga {0}"),
                                i + 1
                            ),
                            LanguageManager.Get("Categories.ValidationError", "Errore Validazione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(category.CategoryName))
                    {
                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.NameMissing", "Nome categoria mancante alla riga {0}"),
                                i + 1
                            ),
                            LanguageManager.Get("Categories.ValidationError", "Errore Validazione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    try
                    {
                        ColorTranslator.FromHtml(category.Color);
                    }
                    catch
                    {
                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.InvalidColor", "Colore non valido per categoria '{0}':\n{1}"),
                                category.CategoryName,
                                category.Color
                            ),
                            LanguageManager.Get("Categories.ValidationError", "Errore Validazione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    var duplicates = _allCategories.Where(c => c.CategoryCode == category.CategoryCode).ToList();
                    if (duplicates.Count > 1)
                    {
                        MessageBox.Show(
                            string.Format(
                                LanguageManager.Get("Categories.DuplicateCode", "Codice categoria duplicato:  {0}"),
                                category.CategoryCode
                            ),
                            LanguageManager.Get("Categories.ValidationError", "Errore Validazione"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }

                for (int i = 0; i < _allCategories.Count; i++)
                {
                    if (_allCategories[i].ID == 0)
                    {
                        int maxId = _allCategories.Count > 0 ? _allCategories.Max(c => c.ID) : 0;
                        _allCategories[i].ID = maxId + 1;
                    }
                }

                // ✅ Salva lista completa
                bool success = DbcManager.Save("ADV_Categories.dbc", _allCategories);

                if (success)
                {
                    _isDirty = false;
                    MessageBox.Show(
                        LanguageManager.Get("Categories.SaveSuccess", "✅ Categorie salvate con successo!"),
                        LanguageManager.Get("Common.Success", "Successo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        LanguageManager.Get("Messages.SaveError", "❌ Errore durante il salvataggio!"),
                        LanguageManager.Get("Common.Error", "Errore"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.UnsavedChangesReload", "Ci sono modifiche non salvate.Vuoi ricaricare comunque?"),
                    LanguageManager.Get("Common.Confirm", "Conferma"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                    return;
            }

            LoadCategories();
            CleanOrphanSchedules();

            // ✅ Reset ricerca
            txtSearch.Text = "";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = LanguageManager.Get("Categories.SearchPlaceholder", "🔍 Cerca categoria...");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_isDirty)
            {
                var result = MessageBox.Show(
                    LanguageManager.Get("Messages.UnsavedChangesSave", "Ci sono modifiche non salvate.Vuoi salvarle? "),
                    LanguageManager.Get("Messages.UnsavedChangesTitle", "Modifiche Non Salvate"),
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

            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        }
    }
}
