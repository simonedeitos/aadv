using AirADV.Services;
using AirADV.Services.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace AirADV.Forms
{
    public partial class ReportForm : Form
    {
        private int _stationID = 0;
        private DateTime _dateFrom = DateTime.Now.Date.AddMonths(-1);
        private DateTime _dateTo = DateTime.Now.Date;

        public ReportForm()
        {
            InitializeComponent();
            this.Load += ReportForm_Load;
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                _stationID = ConfigManager.CurrentStationID;

                if (_stationID == 0)
                {
                    MessageBox.Show("Seleziona prima un'emittente!", "Attenzione",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                SetupDataGridViews();
                LoadFilters();
                ApplyLanguage();

                // Imposta date default
                dtpDateFrom.Value = _dateFrom;
                dtpDateTo.Value = _dateTo;

                LanguageManager.LanguageChanged += (s, ev) => ApplyLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore inizializzazione:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyLanguage()
        {
            try
            {
                lblTitle.Text = LanguageManager.Get("ReportForm.Title", "📊 Report e Statistiche");

                // Filtri
                grpFilters.Text = LanguageManager.Get("ReportForm.GrpFilters", "🔍 Filtri");
                lblDateFrom.Text = LanguageManager.Get("ReportForm.LblDateFrom", "Dal:");
                lblDateTo.Text = LanguageManager.Get("ReportForm.LblDateTo", "Al:");
                lblClient.Text = LanguageManager.Get("ReportForm.LblClient", "Cliente:");
                lblCampaign.Text = LanguageManager.Get("ReportForm.LblCampaign", "Campagna:");
                btnGenerate.Text = LanguageManager.Get("ReportForm.BtnGenerate", "📊 Genera Report");

                // Tab
                tabStatistics.Text = LanguageManager.Get("ReportForm.TabStatistics", "📈 Statistiche");
                tabByCampaign.Text = LanguageManager.Get("ReportForm.TabByCampaign", "📋 Per Campagna");
                tabByClient.Text = LanguageManager.Get("ReportForm.TabByClient", "👥 Per Cliente");
                tabByCategory.Text = LanguageManager.Get("ReportForm.TabByCategory", "🏷️ Per Categoria");

                // Bottoni
                btnExportPdf.Text = LanguageManager.Get("ReportForm.BtnExportPdf", "📄 Esporta PDF");
                btnExportCsv.Text = LanguageManager.Get("ReportForm.BtnExportCsv", "📊 Esporta CSV");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore ApplyLanguage:  {ex.Message}");
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Carica clienti
                var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc")
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClientName)
                    .ToList();

                clients.Insert(0, new DbcManager.Client { ID = 0, ClientName = "-- Tutti i clienti --" });

                cmbClient.DataSource = clients;
                cmbClient.DisplayMember = "ClientName";
                cmbClient.ValueMember = "ID";
                cmbClient.SelectedIndex = 0;

                // Carica campagne
                LoadCampaignsFilter();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore LoadFilters: {ex.Message}");
            }
        }

        private void LoadCampaignsFilter()
        {
            try
            {
                int clientID = (int)(cmbClient.SelectedValue ?? 0);

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc")
                    .Where(c => c.StationID == _stationID)
                    .Where(c => clientID == 0 || c.ClientID == clientID)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToList();

                campaigns.Insert(0, new DbcManager.Campaign
                {
                    ID = 0,
                    CampaignName = "-- Tutte le campagne --"
                });

                cmbCampaign.DataSource = campaigns;
                cmbCampaign.DisplayMember = "CampaignName";
                cmbCampaign.ValueMember = "ID";
                cmbCampaign.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore LoadCampaignsFilter: {ex.Message}");
            }
        }

        private void SetupDataGridViews()
        {
            // Setup dgvCampaigns
            dgvCampaigns.AutoGenerateColumns = false;
            dgvCampaigns.AllowUserToAddRows = false;
            dgvCampaigns.AllowUserToDeleteRows = false;
            dgvCampaigns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCampaigns.MultiSelect = false;
            dgvCampaigns.RowHeadersVisible = false;
            dgvCampaigns.BackgroundColor = Color.White;
            dgvCampaigns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCampaignCode",
                HeaderText = "Codice",
                DataPropertyName = "CampaignCode",
                FillWeight = 15
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCampaignName",
                HeaderText = "Campagna",
                DataPropertyName = "CampaignName",
                FillWeight = 25
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClientName",
                HeaderText = "Cliente",
                DataPropertyName = "ClientName",
                FillWeight = 20
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPeriod",
                HeaderText = "Periodo",
                DataPropertyName = "Period",
                FillWeight = 20
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalPasses",
                HeaderText = "Passaggi",
                DataPropertyName = "TotalPasses",
                FillWeight = 10
            });

            dgvCampaigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalDuration",
                HeaderText = "Durata Tot.",
                DataPropertyName = "TotalDuration",
                FillWeight = 10
            });

            // Setup dgvClients
            dgvClients.AutoGenerateColumns = false;
            dgvClients.AllowUserToAddRows = false;
            dgvClients.AllowUserToDeleteRows = false;
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.MultiSelect = false;
            dgvClients.RowHeadersVisible = false;
            dgvClients.BackgroundColor = Color.White;
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClientCode",
                HeaderText = "Codice",
                DataPropertyName = "ClientCode",
                FillWeight = 15
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClientName2",
                HeaderText = "Cliente",
                DataPropertyName = "ClientName",
                FillWeight = 30
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colActiveCampaigns",
                HeaderText = "Campagne Attive",
                DataPropertyName = "ActiveCampaigns",
                FillWeight = 15
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalPasses2",
                HeaderText = "Tot.Passaggi",
                DataPropertyName = "TotalPasses",
                FillWeight = 15
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalSpots",
                HeaderText = "Tot.Spot",
                DataPropertyName = "TotalSpots",
                FillWeight = 15
            });

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalDuration2",
                HeaderText = "Durata Tot.",
                DataPropertyName = "TotalDuration",
                FillWeight = 10
            });

            // Setup dgvCategories
            dgvCategories.AutoGenerateColumns = false;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.MultiSelect = false;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryName",
                HeaderText = "Categoria",
                DataPropertyName = "CategoryName",
                FillWeight = 30
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryCampaigns",
                HeaderText = "Campagne",
                DataPropertyName = "Campaigns",
                FillWeight = 15
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryPasses",
                HeaderText = "Passaggi",
                DataPropertyName = "TotalPasses",
                FillWeight = 15
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryDuration",
                HeaderText = "Durata Tot.",
                DataPropertyName = "TotalDuration",
                FillWeight = 15
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryPercentage",
                HeaderText = "% Totale",
                DataPropertyName = "Percentage",
                FillWeight = 15
            });
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCampaignsFilter();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                _dateFrom = dtpDateFrom.Value.Date;
                _dateTo = dtpDateTo.Value.Date;

                if (_dateTo < _dateFrom)
                {
                    MessageBox.Show("La data finale deve essere successiva alla data iniziale!", "Attenzione",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblLoading.Visible = true;
                Application.DoEvents();

                // Genera tutti i report
                GenerateStatistics();
                GenerateCampaignReport();
                GenerateClientReport();
                GenerateCategoryReport();

                lblLoading.Visible = false;

                MessageBox.Show($"✅ Report generato con successo!\n\nPerido: {_dateFrom: dd/MM/yyyy} - {_dateTo:dd/MM/yyyy}",
                    "Report Generato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblLoading.Visible = false;
                MessageBox.Show($"Errore generazione report:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateStatistics()
        {
            try
            {
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == _stationID &&
                               s.ScheduleDate >= _dateFrom &&
                               s.ScheduleDate <= _dateTo &&
                               s.FileType == "SPOT")
                    .ToList();

                int clientID = (int)(cmbClient.SelectedValue ?? 0);
                int campaignID = (int)(cmbCampaign.SelectedValue ?? 0);

                if (clientID > 0)
                    schedules = schedules.Where(s => s.ClientID == clientID).ToList();

                if (campaignID > 0)
                    schedules = schedules.Where(s => s.CampaignID == campaignID).ToList();

                // Calcola statistiche
                int totalDays = (_dateTo - _dateFrom).Days + 1;
                int totalPasses = schedules.Count;
                int totalDuration = schedules.Sum(s => s.Duration);
                int uniqueCampaigns = schedules.Select(s => s.CampaignID).Distinct().Count();
                int uniqueClients = schedules.Select(s => s.ClientID).Distinct().Count();
                int uniqueSpots = schedules.Select(s => s.SpotID).Distinct().Count();

                double avgPassesPerDay = totalDays > 0 ? (double)totalPasses / totalDays : 0;
                double avgDurationPerDay = totalDays > 0 ? (double)totalDuration / totalDays : 0;

                // Popola labels
                lblTotalDays.Text = $"Giorni analizzati: {totalDays}";
                lblTotalPasses.Text = $"Passaggi totali: {totalPasses: N0}";
                lblTotalDuration.Text = $"Durata totale: {TimeSpan.FromSeconds(totalDuration):hh\\:mm\\:ss}";
                lblUniqueCampaigns.Text = $"Campagne attive: {uniqueCampaigns}";
                lblUniqueClients.Text = $"Clienti attivi: {uniqueClients}";
                lblUniqueSpots.Text = $"Spot diversi: {uniqueSpots}";
                lblAvgPassesPerDay.Text = $"Media passaggi/giorno: {avgPassesPerDay:N1}";
                lblAvgDurationPerDay.Text = $"Media durata/giorno: {TimeSpan.FromSeconds(avgDurationPerDay):hh\\:mm\\:ss}";

                Console.WriteLine($"[ReportForm] Statistiche generate - Passaggi: {totalPasses}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore GenerateStatistics: {ex.Message}");
            }
        }

        private void GenerateCampaignReport()
        {
            try
            {
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == _stationID &&
                               s.ScheduleDate >= _dateFrom &&
                               s.ScheduleDate <= _dateTo &&
                               s.FileType == "SPOT")
                    .ToList();

                int clientID = (int)(cmbClient.SelectedValue ?? 0);
                int campaignID = (int)(cmbCampaign.SelectedValue ?? 0);

                if (clientID > 0)
                    schedules = schedules.Where(s => s.ClientID == clientID).ToList();

                if (campaignID > 0)
                    schedules = schedules.Where(s => s.CampaignID == campaignID).ToList();

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");

                var campaignStats = schedules
                    .GroupBy(s => s.CampaignID)
                    .Select(g =>
                    {
                        var campaign = campaigns.FirstOrDefault(c => c.ID == g.Key);
                        var client = clients.FirstOrDefault(c => c.ID == g.First().ClientID);

                        return new
                        {
                            CampaignCode = campaign?.CampaignCode ?? "N/A",
                            CampaignName = campaign?.CampaignName ?? "Sconosciuta",
                            ClientName = client?.ClientName ?? "N/A",
                            Period = campaign != null ? $"{campaign.StartDate: dd/MM/yy} - {campaign.EndDate:dd/MM/yy}" : "",
                            TotalPasses = g.Count(),
                            TotalDuration = $"{TimeSpan.FromSeconds(g.Sum(s => s.Duration)):hh\\:mm\\:ss}"
                        };
                    })
                    .OrderByDescending(c => c.TotalPasses)
                    .ToList();

                dgvCampaigns.DataSource = campaignStats;

                Console.WriteLine($"[ReportForm] Report campagne:  {campaignStats.Count} campagne");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore GenerateCampaignReport: {ex.Message}");
            }
        }

        private void GenerateClientReport()
        {
            try
            {
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == _stationID &&
                               s.ScheduleDate >= _dateFrom &&
                               s.ScheduleDate <= _dateTo &&
                               s.FileType == "SPOT")
                    .ToList();

                int clientID = (int)(cmbClient.SelectedValue ?? 0);

                if (clientID > 0)
                    schedules = schedules.Where(s => s.ClientID == clientID).ToList();

                var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");

                var clientStats = schedules
                    .GroupBy(s => s.ClientID)
                    .Select(g =>
                    {
                        var client = clients.FirstOrDefault(c => c.ID == g.Key);
                        var clientCampaigns = campaigns.Where(c => c.ClientID == g.Key).ToList();

                        return new
                        {
                            ClientCode = client?.ClientCode ?? "N/A",
                            ClientName = client?.ClientName ?? "Sconosciuto",
                            ActiveCampaigns = g.Select(s => s.CampaignID).Distinct().Count(),
                            TotalPasses = g.Count(),
                            TotalSpots = g.Select(s => s.SpotID).Distinct().Count(),
                            TotalDuration = $"{TimeSpan.FromSeconds(g.Sum(s => s.Duration)):hh\\:mm\\:ss}"
                        };
                    })
                    .OrderByDescending(c => c.TotalPasses)
                    .ToList();

                dgvClients.DataSource = clientStats;

                Console.WriteLine($"[ReportForm] Report clienti:  {clientStats.Count} clienti");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore GenerateClientReport: {ex.Message}");
            }
        }

        private void GenerateCategoryReport()
        {
            try
            {
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == _stationID &&
                               s.ScheduleDate >= _dateFrom &&
                               s.ScheduleDate <= _dateTo &&
                               s.FileType == "SPOT")
                    .ToList();

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var categories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc");

                int totalPasses = schedules.Count;

                var categoryStats = schedules
                    .Select(s =>
                    {
                        var campaign = campaigns.FirstOrDefault(c => c.ID == s.CampaignID);
                        return new
                        {
                            Schedule = s,
                            CategoryID = campaign?.CategoryID ?? 0
                        };
                    })
                    .GroupBy(x => x.CategoryID)
                    .Select(g =>
                    {
                        var category = categories.FirstOrDefault(c => c.ID == g.Key);
                        int passes = g.Count();
                        double percentage = totalPasses > 0 ? (passes * 100.0 / totalPasses) : 0;

                        return new
                        {
                            CategoryName = category?.CategoryName ?? "Non Classificata",
                            Campaigns = g.Select(x => x.Schedule.CampaignID).Distinct().Count(),
                            TotalPasses = passes,
                            TotalDuration = $"{TimeSpan.FromSeconds(g.Sum(x => x.Schedule.Duration)):hh\\:mm\\:ss}",
                            Percentage = $"{percentage:F1}%"
                        };
                    })
                    .OrderByDescending(c => c.TotalPasses)
                    .ToList();

                dgvCategories.DataSource = categoryStats;

                Console.WriteLine($"[ReportForm] Report categorie: {categoryStats.Count} categorie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] Errore GenerateCategoryReport: {ex.Message}");
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                int campaignID = (int)(cmbCampaign.SelectedValue ?? 0);

                if (campaignID == 0)
                {
                    MessageBox.Show("Seleziona una campagna specifica per esportare il PDF!", "Attenzione",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PDF|*.pdf";
                    dialog.FileName = $"Report_Campagna_{campaignID}_{DateTime.Now:yyyyMMdd}.pdf";
                    dialog.InitialDirectory = ConfigManager.PdfOutputPath;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var exportManager = new ExportManager();
                        bool success = exportManager.ExportClientReportPdf(
                            _stationID,
                            campaignID,
                            _dateFrom,
                            _dateTo,
                            dialog.FileName
                        );

                        if (success)
                        {
                            MessageBox.Show("✅ PDF esportato con successo!", "Successo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var result = MessageBox.Show("Vuoi aprire il file? ", "Apri PDF",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = dialog.FileName,
                                    UseShellExecute = true
                                });
                            }
                        }
                        else
                        {
                            MessageBox.Show("❌ Errore durante l'esportazione!", "Errore",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore export PDF:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "CSV|*.csv";
                    dialog.FileName = $"Report_Palinsesto_{DateTime.Now:yyyyMMdd}.csv";
                    dialog.InitialDirectory = ConfigManager.PdfOutputPath;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var exportManager = new ExportManager();
                        bool success = exportManager.ExportScheduleCsv(
                            _stationID,
                            _dateFrom,
                            _dateTo,
                            dialog.FileName
                        );

                        if (success)
                        {
                            MessageBox.Show("✅ CSV esportato con successo!", "Successo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var result = MessageBox.Show("Vuoi aprire il file?", "Apri CSV",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = dialog.FileName,
                                    UseShellExecute = true
                                });
                            }
                        }
                        else
                        {
                            MessageBox.Show("❌ Errore durante l'esportazione!", "Errore",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore export CSV:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}