using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
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

                using var stream = new FileStream(outputPath, FileMode.Create);
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document doc = new Document(pdf);

                // Logo
                if (!string.IsNullOrEmpty(station?.LogoPath) && File.Exists(station.LogoPath))
                {
                    try
                    {
                        ImageData imageData = ImageDataFactory.Create(station.LogoPath);
                        iText.Layout.Element.Image logo = new iText.Layout.Element.Image(imageData);
                        logo.ScaleToFit(200f, 60f);
                        logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        doc.Add(logo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ExportManager] Errore caricamento logo: {ex.Message}");
                    }
                }

                // Titolo
                PdfFont titleFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                Paragraph title = new Paragraph(station?.StationName ?? "EMITTENTE")
                    .SetFont(titleFontPdf)
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(10f);
                doc.Add(title);

                PdfFont subtitleFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                Paragraph subtitle = new Paragraph("PALINSESTO PUBBLICITARIO")
                    .SetFont(subtitleFontPdf)
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20f);
                doc.Add(subtitle);

                // Info campagna
                PdfFont normalFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                doc.Add(new Paragraph($"Cliente: {client?.ClientName ?? ""}").SetFont(normalFontPdf).SetFontSize(10));
                doc.Add(new Paragraph($"Azienda: {client?.CompanyName ?? ""}").SetFont(normalFontPdf).SetFontSize(10));
                doc.Add(new Paragraph($"Campagna: {campaign?.CampaignName ?? ""}").SetFont(normalFontPdf).SetFontSize(10));
                doc.Add(new Paragraph($"Spot:  {spot?.SpotTitle ?? ""} ({spot?.Duration ?? 0}s)").SetFont(normalFontPdf).SetFontSize(10));
                doc.Add(new Paragraph($"Periodo: {fromDate:dd/MM/yyyy} - {toDate:dd/MM/yyyy}").SetFont(normalFontPdf).SetFontSize(10));
                doc.Add(new Paragraph(" ").SetFont(normalFontPdf).SetFontSize(10));

                // Palinsesto
                var byDate = schedules.GroupBy(s => s.ScheduleDate.Date);
                PdfFont dateFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                foreach (var dateGroup in byDate)
                {
                    string dayName = dateGroup.Key.ToString("dddd", System.Globalization.CultureInfo.GetCultureInfo("it-IT"));
                    Paragraph dateHeader = new Paragraph($"{dayName} {dateGroup.Key:dd/MM/yyyy}")
                        .SetFont(dateFontPdf)
                        .SetFontSize(12)
                        .SetMarginTop(10f)
                        .SetMarginBottom(5f);
                    doc.Add(dateHeader);

                    foreach (var schedule in dateGroup)
                    {
                        doc.Add(new Paragraph($"  {schedule.SlotTime}").SetFont(normalFontPdf).SetFontSize(10));
                    }
                }

                // Totali
                doc.Add(new Paragraph(" ").SetFont(normalFontPdf).SetFontSize(10));
                PdfFont totalFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                doc.Add(new Paragraph($"Totale Passaggi: {schedules.Count}").SetFont(totalFontPdf).SetFontSize(10));
                int totalSeconds = schedules.Sum(s => s.Duration);
                doc.Add(new Paragraph($"Totale Secondi: {totalSeconds}s ({TimeSpan.FromSeconds(totalSeconds):mm\\:ss})").SetFont(totalFontPdf).SetFontSize(10));

                // Footer
                doc.Add(new Paragraph(" ").SetFont(normalFontPdf).SetFontSize(10));
                PdfFont footerFontPdf = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                Paragraph footer = new Paragraph($"Generato il: {DateTime.Now:dd/MM/yyyy HH:mm}\nAirADV - Gestionale Pubblicitario")
                    .SetFont(footerFontPdf)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.CENTER);
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