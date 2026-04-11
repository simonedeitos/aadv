using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirADV.Services
{
    public static class AirDirectorExportService
    {
        private const string EXPORT_FILENAME = "ADV_AirDirector.dbc";

        /// <summary>
        /// Esporta palinsesto completo per AirDirector
        /// </summary>
        public static bool ExportFullSchedule(int stationID, DateTime startDate, DateTime endDate)
        {
            try
            {
                Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");
                Console.WriteLine($"[AirDirectorExport] Inizio export palinsesto per stazione {stationID}");
                Console.WriteLine($"[AirDirectorExport] Periodo: {startDate: dd/MM/yyyy} - {endDate:dd/MM/yyyy}");

                // Carica dati necessari
                var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc")
                    .Where(s => s.StationID == stationID &&
                                s.ScheduleDate >= startDate &&
                                s.ScheduleDate <= endDate)
                    .OrderBy(s => s.ScheduleDate)
                    .ThenBy(s => s.SlotTime)
                    .ThenBy(s => s.SequenceOrder)
                    .ToList();

                var clients = DbcManager.Load<DbcManager.Client>("ADV_Clients.dbc");
                var spots = DbcManager.Load<DbcManager.Spot>("ADV_Spots.dbc");
                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var categories = DbcManager.Load<DbcManager.Category>("ADV_Categories.dbc");

                Console.WriteLine($"[AirDirectorExport] Trovati {schedules.Count} record da processare");

                // Raggruppa per Data + Slot
                var groupedBySlot = schedules
                    .GroupBy(s => new { s.ScheduleDate, s.SlotTime })
                    .OrderBy(g => g.Key.ScheduleDate)
                    .ThenBy(g => g.Key.SlotTime)
                    .ToList();

                Console.WriteLine($"[AirDirectorExport] Raggrupati in {groupedBySlot.Count} punti orari unici");
                Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");

                // Costruisci playlist
                var playlist = new List<DbcManager.AirDirectorPlaylistItem>();
                int idCounter = 1;
                int emptySlots = 0;
                int totalInfraSpots = 0;

                foreach (var slotGroup in groupedBySlot)
                {
                    var slotItems = slotGroup.OrderBy(s => s.SequenceOrder).ToList();

                    // Salta punti orari senza SPOT
                    var spotItems = slotItems.Where(s => s.FileType == "SPOT").ToList();
                    if (spotItems.Count == 0)
                    {
                        emptySlots++;
                        Console.WriteLine($"[AirDirectorExport] ⏭️ Saltato slot vuoto: {slotGroup.Key.ScheduleDate:dd/MM/yyyy} {slotGroup.Key.SlotTime}");
                        continue;
                    }

                    Console.WriteLine($"[AirDirectorExport] 📅 Slot: {slotGroup.Key.ScheduleDate:dd/MM/yyyy} {slotGroup.Key.SlotTime}");

                    int sequenceOrder = 1;
                    bool openingAdded = false;
                    bool closingAdded = false;
                    int spotCount = 0;
                    int infraSpotAddedCount = 0;

                    // ✅ ELABORA OGNI ELEMENTO DELLA SEQUENZA
                    for (int i = 0; i < slotItems.Count; i++)
                    {
                        var schedule = slotItems[i];
                        DbcManager.AirDirectorPlaylistItem item = null;

                        switch (schedule.FileType)
                        {
                            case "OPENING":
                                if (!openingAdded && !string.IsNullOrEmpty(schedule.FilePath))
                                {
                                    item = CreatePlaylistItem(schedule, idCounter++, sequenceOrder++, clients, spots, campaigns, categories);
                                    playlist.Add(item);
                                    openingAdded = true;
                                    Console.WriteLine($"[AirDirectorExport]   📢 {sequenceOrder - 1}.OPENING: {Path.GetFileName(schedule.FilePath)}");
                                }
                                break;

                            case "SPOT":
                                if (!string.IsNullOrEmpty(schedule.FilePath))
                                {
                                    spotCount++;
                                    item = CreatePlaylistItem(schedule, idCounter++, sequenceOrder++, clients, spots, campaigns, categories);
                                    playlist.Add(item);
                                    Console.WriteLine($"[AirDirectorExport]   🎵 {sequenceOrder - 1}.SPOT {spotCount}/{spotItems.Count}:  {Path.GetFileName(schedule.FilePath)}");
                                }
                                break;

                            case "INFRASPOT":
                                if (!string.IsNullOrEmpty(schedule.FilePath))
                                {
                                    item = CreatePlaylistItem(schedule, idCounter++, sequenceOrder++, clients, spots, campaigns, categories);
                                    playlist.Add(item);
                                    infraSpotAddedCount++;
                                    totalInfraSpots++;
                                    Console.WriteLine($"[AirDirectorExport]   🔹 {sequenceOrder - 1}. INFRASPOT:  {Path.GetFileName(schedule.FilePath)}");
                                }
                                break;

                            case "CLOSING":
                                if (!closingAdded && !string.IsNullOrEmpty(schedule.FilePath))
                                {
                                    item = CreatePlaylistItem(schedule, idCounter++, sequenceOrder++, clients, spots, campaigns, categories);
                                    playlist.Add(item);
                                    closingAdded = true;
                                    Console.WriteLine($"[AirDirectorExport]   📢 {sequenceOrder - 1}. CLOSING: {Path.GetFileName(schedule.FilePath)}");
                                }
                                break;
                        }
                    }

                    // Log riepilogo slot
                    Console.WriteLine($"[AirDirectorExport] ✅ Totale elementi: {sequenceOrder - 1} " +
                                    $"({(openingAdded ? "OPENING + " : "")}" +
                                    $"{spotCount} SPOT" +
                                    $"{(infraSpotAddedCount > 0 ? $" + {infraSpotAddedCount} INFRASPOT" : "")}" +
                                    $"{(closingAdded ? " + CLOSING" : "")})");
                    Console.WriteLine($"[AirDirectorExport] ───────────────────────────────────────");
                }

                // Salva file
                string exportPath = Path.Combine(ConfigManager.CurrentStationDatabasePath, EXPORT_FILENAME);
                bool success = DbcManager.Save(EXPORT_FILENAME, playlist);

                if (success)
                {
                    GenerateMetadataFile(stationID, startDate, endDate, playlist.Count, exportPath);

                    Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");
                    Console.WriteLine($"[AirDirectorExport] ✅ EXPORT COMPLETATO");
                    Console.WriteLine($"[AirDirectorExport]   - Totale elementi esportati: {playlist.Count}");
                    Console.WriteLine($"[AirDirectorExport]   - Slot processati: {groupedBySlot.Count}");
                    Console.WriteLine($"[AirDirectorExport]   - Slot vuoti saltati: {emptySlots}");
                    Console.WriteLine($"[AirDirectorExport]   - Slot con contenuto: {groupedBySlot.Count - emptySlots}");
                    Console.WriteLine($"[AirDirectorExport]   - Infraspot inseriti: {totalInfraSpots}");
                    Console.WriteLine($"[AirDirectorExport]   - File:  {exportPath}");
                    Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");
                Console.WriteLine($"[AirDirectorExport] ❌ ERRORE CRITICO");
                Console.WriteLine($"[AirDirectorExport]   Messaggio: {ex.Message}");
                Console.WriteLine($"[AirDirectorExport]   StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[AirDirectorExport] ═══════════════════════════════════════");
                return false;
            }
        }

        /// <summary>
        /// Crea un item della playlist con tutte le info
        /// </summary>
        private static DbcManager.AirDirectorPlaylistItem CreatePlaylistItem(
            DbcManager.Schedule schedule,
            int id,
            int sequenceOrder,
            List<DbcManager.Client> clients,
            List<DbcManager.Spot> spots,
            List<DbcManager.Campaign> campaigns,
            List<DbcManager.Category> categories)
        {
            var item = new DbcManager.AirDirectorPlaylistItem
            {
                ID = id,
                Date = schedule.ScheduleDate,
                SlotTime = schedule.SlotTime,
                SequenceOrder = sequenceOrder,
                FileType = schedule.FileType,
                FilePath = schedule.FilePath,
                Duration = schedule.Duration,
                IsActive = true
            };

            // Se è uno SPOT, aggiungi info dettagliate
            if (schedule.FileType == "SPOT" && schedule.ClientID > 0)
            {
                var client = clients.FirstOrDefault(c => c.ID == schedule.ClientID);
                var spot = spots.FirstOrDefault(s => s.ID == schedule.SpotID);
                var campaign = campaigns.FirstOrDefault(c => c.ID == schedule.CampaignID);
                var category = campaign != null ? categories.FirstOrDefault(cat => cat.ID == campaign.CategoryID) : null;

                item.ClientName = client?.ClientName ?? "";
                item.SpotTitle = spot?.SpotTitle ?? "";
                item.CampaignName = campaign?.CampaignName ?? "";
                item.CategoryName = category?.CategoryName ?? "";
            }

            return item;
        }

        /// <summary>
        /// Esporta solo un periodo specifico (es.settimana corrente)
        /// </summary>
        public static bool ExportWeek(int stationID, DateTime weekStart)
        {
            DateTime weekEnd = weekStart.AddDays(7);
            return ExportFullSchedule(stationID, weekStart, weekEnd);
        }

        /// <summary>
        /// Esporta solo mese corrente
        /// </summary>
        public static bool ExportMonth(int stationID, int year, int month)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            return ExportFullSchedule(stationID, start, end);
        }

        /// <summary>
        /// Genera file di metadati con informazioni aggiuntive
        /// </summary>
        private static void GenerateMetadataFile(int stationID, DateTime startDate, DateTime endDate, int totalItems, string playlistPath)
        {
            try
            {
                var station = DbcManager.Load<DbcManager.Station>("ADV_Config.dbc")
                    .FirstOrDefault(s => s.StationID == stationID);

                var lines = new List<string>
                {
                    "# AirDirector Playlist Metadata",
                    $"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm: ss}",
                    "",
                    $"StationID={stationID}",
                    $"StationName={station?.StationName ??  "N/A"}",
                    $"StartDate={startDate:yyyy-MM-dd}",
                    $"EndDate={endDate:yyyy-MM-dd}",
                    $"TotalItems={totalItems}",
                    $"PlaylistFile={Path.GetFileName(playlistPath)}",
                    "",
                    "# File Format:",
                    "# Date,SlotTime,SequenceOrder,FileType,FilePath,Duration,ClientName,SpotTitle,CampaignName,CategoryName,IsActive"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AirDirectorExport] ⚠️ Errore metadata: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica integrità file esportato
        /// </summary>
        public static bool ValidateExport(int stationID)
        {
            try
            {
                string exportPath = Path.Combine(ConfigManager.CurrentStationDatabasePath, EXPORT_FILENAME);

                if (!File.Exists(exportPath))
                {
                    Console.WriteLine($"[AirDirectorExport] ⚠️ File non trovato: {exportPath}");
                    return false;
                }

                var playlist = DbcManager.Load<DbcManager.AirDirectorPlaylistItem>(EXPORT_FILENAME);

                int missingFiles = 0;
                foreach (var item in playlist)
                {
                    if (!string.IsNullOrEmpty(item.FilePath) && !File.Exists(item.FilePath))
                    {
                        Console.WriteLine($"[AirDirectorExport] ⚠️ File mancante: {item.FilePath}");
                        missingFiles++;
                    }
                }

                if (missingFiles > 0)
                {
                    Console.WriteLine($"[AirDirectorExport] ⚠️ {missingFiles} file mancanti su {playlist.Count}");
                    return false;
                }

                Console.WriteLine($"[AirDirectorExport] ✅ Validazione OK:  {playlist.Count} item, 0 file mancanti");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AirDirectorExport] ❌ Errore validazione:  {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Ottieni statistiche export corrente
        /// </summary>
        public static ExportStats GetExportStats(int stationID)
        {
            try
            {
                var playlist = DbcManager.Load<DbcManager.AirDirectorPlaylistItem>(EXPORT_FILENAME);

                var stats = new ExportStats
                {
                    TotalItems = playlist.Count,
                    TotalSpots = playlist.Count(p => p.FileType == "SPOT"),
                    TotalInfraSpots = playlist.Count(p => p.FileType == "INFRASPOT"),
                    TotalJingles = playlist.Count(p => p.FileType == "OPENING" || p.FileType == "CLOSING"),
                    TotalDuration = playlist.Sum(p => p.Duration),
                    DaysCount = playlist.Select(p => p.Date.Date).Distinct().Count(),
                    SlotsCount = playlist.Select(p => new { p.Date, p.SlotTime }).Distinct().Count(),
                    StartDate = playlist.Any() ? playlist.Min(p => p.Date) : DateTime.MinValue,
                    EndDate = playlist.Any() ? playlist.Max(p => p.Date) : DateTime.MinValue
                };

                return stats;
            }
            catch
            {
                return new ExportStats();
            }
        }

        public class ExportStats
        {
            public int TotalItems { get; set; }
            public int TotalSpots { get; set; }
            public int TotalInfraSpots { get; set; }
            public int TotalJingles { get; set; }
            public int TotalDuration { get; set; }
            public int DaysCount { get; set; }
            public int SlotsCount { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}