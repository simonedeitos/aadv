using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AirADV.Services;

namespace AirADV.Services
{
    /// <summary>
    /// Gestione export PDF e CSV
    /// </summary>
    public class ExportManager
    {
        // ===== EXPORT PDF =====

        public bool ExportClientReportPdf(int stationID, int campaignID, DateTime fromDate, DateTime toDate, string outputPath)
        {
            try
            {
                var station = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc")
                    .FirstOrDefault(s => s.StationID == stationID);

                var campaign = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc")
                    .FirstOrDefault(c => c.ID == campaignID);

                var client = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc")
                    .FirstOrDefault(c => c.ID == campaign?.ClientID);

                var spot = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc")
                    .FirstOrDefault(s => s.ID == campaign?.SpotID);

                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == stationID &&
                                s.CampaignID == campaignID &&
                                s.FileType == "SPOT" &&
                                s.ScheduleDate >= fromDate &&
                                s.ScheduleDate <= toDate)
                    .OrderBy(s => s.ScheduleDate)
                    .ThenBy(s => s.SlotTime)
                    .ToList();

                Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));

                doc.Open();

                // Logo
                if (!string.IsNullOrEmpty(station?.LogoPath) && File.Exists(station.LogoPath))
                {
                    try
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(station.LogoPath);
                        logo.ScaleToFit(200f, 60f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(logo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ExportManager] Errore caricamento logo: {ex.Message}");
                    }
                }

                // Titolo
                iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Paragraph title = new Paragraph(station?.StationName ?? "EMITTENTE", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10f;
                doc.Add(title);

                iTextSharp.text.Font subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);
                Paragraph subtitle = new Paragraph("PALINSESTO PUBBLICITARIO", subtitleFont);
                subtitle.Alignment = Element.ALIGN_CENTER;
                subtitle.SpacingAfter = 20f;
                doc.Add(subtitle);

                // Info campagna
                iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                doc.Add(new Paragraph($"Cliente: {client?.ClientName ?? ""}", normalFont));
                doc.Add(new Paragraph($"Azienda: {client?.CompanyName ?? ""}", normalFont));
                doc.Add(new Paragraph($"Campagna: {campaign?.CampaignName ?? ""}", normalFont));
                doc.Add(new Paragraph($"Spot:  {spot?.SpotTitle ?? ""} ({spot?.Duration ?? 0}s)", normalFont));
                doc.Add(new Paragraph($"Periodo: {fromDate: dd/MM/yyyy} - {toDate:dd/MM/yyyy}", normalFont));
                doc.Add(new Paragraph(" ", normalFont));

                // Palinsesto
                var byDate = schedules.GroupBy(s => s.ScheduleDate.Date);

                foreach (var dateGroup in byDate)
                {
                    iTextSharp.text.Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    string dayName = dateGroup.Key.ToString("dddd", System.Globalization.CultureInfo.GetCultureInfo("it-IT"));
                    Paragraph dateHeader = new Paragraph($"{dayName} {dateGroup.Key:dd/MM/yyyy}", dateFont);
                    dateHeader.SpacingBefore = 10f;
                    dateHeader.SpacingAfter = 5f;
                    doc.Add(dateHeader);

                    foreach (var schedule in dateGroup)
                    {
                        doc.Add(new Paragraph($"  {schedule.SlotTime}", normalFont));
                    }
                }

                // Totali
                doc.Add(new Paragraph(" ", normalFont));
                iTextSharp.text.Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                doc.Add(new Paragraph($"Totale Passaggi: {schedules.Count}", totalFont));
                int totalSeconds = schedules.Sum(s => s.Duration);
                doc.Add(new Paragraph($"Totale Secondi: {totalSeconds}s ({TimeSpan.FromSeconds(totalSeconds):mm\\:ss})", totalFont));

                // Footer
                doc.Add(new Paragraph(" ", normalFont));
                iTextSharp.text.Font footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                Paragraph footer = new Paragraph($"Generato il: {DateTime.Now:dd/MM/yyyy HH:mm}\nAirADV - Gestionale Pubblicitario", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                doc.Add(footer);

                doc.Close();

                Console.WriteLine($"[ExportManager] PDF esportato: {outputPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExportManager] Errore export PDF: {ex.Message}");
                return false;
            }
        }

        // ===== EXPORT CSV =====

        public bool ExportScheduleCsv(int stationID, DateTime fromDate, DateTime toDate, string outputPath)
        {
            try
            {
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == stationID &&
                                s.FileType == "SPOT" &&
                                s.ScheduleDate >= fromDate &&
                                s.ScheduleDate <= toDate)
                    .OrderBy(s => s.ScheduleDate)
                    .ThenBy(s => s.SlotTime)
                    .ToList();

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
                var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");

                var sb = new StringBuilder();
                sb.AppendLine("Data,Orario,Cliente,Campagna,Spot,Durata");

                foreach (var schedule in schedules)
                {
                    var campaign = campaigns.FirstOrDefault(c => c.ID == schedule.CampaignID);
                    var client = clients.FirstOrDefault(c => c.ID == schedule.ClientID);
                    var spot = spots.FirstOrDefault(s => s.ID == schedule.SpotID);

                    sb.AppendLine($"{schedule.ScheduleDate:dd/MM/yyyy},{schedule.SlotTime},{client?.ClientName ?? ""},{campaign?.CampaignName ?? ""},{spot?.SpotTitle ?? ""},{schedule.Duration}");
                }

                File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);

                Console.WriteLine($"[ExportManager] CSV esportato:  {outputPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExportManager] Errore export CSV: {ex.Message}");
                return false;
            }
        }
    }
}