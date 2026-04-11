using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AirADV.Services
{
    public static class AirDirectorExportService
    {
        private const string EXPORT_FILENAME = "ADV_AirDirector.dbc";
        private const string DATE_FORMAT = "dd/MM/yyyy HH:mm:ss";

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
                var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                    .Where(t => t.StationID == stationID && t.IsActive)
                    .ToList();

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

                    // Cerca il TimeSlot aggiornato corrispondente a questo punto orario
                    var timeSlot = timeSlots.FirstOrDefault(ts => ts.SlotTime == slotGroup.Key.SlotTime);

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
                                if (!openingAdded)
                                {
                                    string openingPath = !string.IsNullOrEmpty(timeSlot?.OpeningFile) ? timeSlot.OpeningFile : schedule.FilePath;
                                    if (!string.IsNullOrEmpty(openingPath))
                                    {
                                        var openingSchedule = new DbcManager.Schedule { ID = schedule.ID, StationID = schedule.StationID, ScheduleDate = schedule.ScheduleDate, SlotTime = schedule.SlotTime, SequenceOrder = schedule.SequenceOrder, FileType = schedule.FileType, FilePath = openingPath, ClientID = schedule.ClientID, SpotID = schedule.SpotID, CampaignID = schedule.CampaignID, Duration = schedule.Duration, IsManual = schedule.IsManual };
                                        item = CreatePlaylistItem(openingSchedule, idCounter++, sequenceOrder++, clients, spots);
                                        playlist.Add(item);
                                        openingAdded = true;
                                        if (timeSlot != null && !string.IsNullOrEmpty(timeSlot.OpeningFile))
                                            Console.WriteLine($"[AirDirectorExport] 🔄 OPENING aggiornato da TimeSlot: {Path.GetFileName(openingPath)}");
                                        else
                                            Console.WriteLine($"[AirDirectorExport]   📢 {sequenceOrder - 1}.OPENING: {Path.GetFileName(openingPath)}");
                                    }
                                }
                                break;

                            case "SPOT":
                                if (!string.IsNullOrEmpty(schedule.FilePath))
                                {
                                    spotCount++;
                                    item = CreatePlaylistItem(schedule, idCounter++, sequenceOrder++, clients, spots);
                                    playlist.Add(item);
                                    Console.WriteLine($"[AirDirectorExport]   🎵 {sequenceOrder - 1}.SPOT {spotCount}/{spotItems.Count}:  {Path.GetFileName(schedule.FilePath)}");
                                }
                                break;

                            case "INFRASPOT":
                                {
                                    string infraSpotPath = !string.IsNullOrEmpty(timeSlot?.InfraSpotFile) ? timeSlot.InfraSpotFile : schedule.FilePath;
                                    if (!string.IsNullOrEmpty(infraSpotPath))
                                    {
                                        var infraSchedule = new DbcManager.Schedule { ID = schedule.ID, StationID = schedule.StationID, ScheduleDate = schedule.ScheduleDate, SlotTime = schedule.SlotTime, SequenceOrder = schedule.SequenceOrder, FileType = schedule.FileType, FilePath = infraSpotPath, ClientID = schedule.ClientID, SpotID = schedule.SpotID, CampaignID = schedule.CampaignID, Duration = schedule.Duration, IsManual = schedule.IsManual };
                                        item = CreatePlaylistItem(infraSchedule, idCounter++, sequenceOrder++, clients, spots);
                                        playlist.Add(item);
                                        infraSpotAddedCount++;
                                        totalInfraSpots++;
                                        if (timeSlot != null && !string.IsNullOrEmpty(timeSlot.InfraSpotFile))
                                            Console.WriteLine($"[AirDirectorExport] 🔄 INFRASPOT aggiornato da TimeSlot: {Path.GetFileName(infraSpotPath)}");
                                        else
                                            Console.WriteLine($"[AirDirectorExport]   🔹 {sequenceOrder - 1}. INFRASPOT:  {Path.GetFileName(infraSpotPath)}");
                                    }
                                }
                                break;

                            case "CLOSING":
                                if (!closingAdded)
                                {
                                    string closingPath = !string.IsNullOrEmpty(timeSlot?.ClosingFile) ? timeSlot.ClosingFile : schedule.FilePath;
                                    if (!string.IsNullOrEmpty(closingPath))
                                    {
                                        var closingSchedule = new DbcManager.Schedule { ID = schedule.ID, StationID = schedule.StationID, ScheduleDate = schedule.ScheduleDate, SlotTime = schedule.SlotTime, SequenceOrder = schedule.SequenceOrder, FileType = schedule.FileType, FilePath = closingPath, ClientID = schedule.ClientID, SpotID = schedule.SpotID, CampaignID = schedule.CampaignID, Duration = schedule.Duration, IsManual = schedule.IsManual };
                                        item = CreatePlaylistItem(closingSchedule, idCounter++, sequenceOrder++, clients, spots);
                                        playlist.Add(item);
                                        closingAdded = true;
                                        if (timeSlot != null && !string.IsNullOrEmpty(timeSlot.ClosingFile))
                                            Console.WriteLine($"[AirDirectorExport] 🔄 CLOSING aggiornato da TimeSlot: {Path.GetFileName(closingPath)}");
                                        else
                                            Console.WriteLine($"[AirDirectorExport]   📢 {sequenceOrder - 1}. CLOSING: {Path.GetFileName(closingPath)}");
                                    }
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
                bool success = SaveAirDirectorDbc(playlist);

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
            List<DbcManager.Spot> spots)
        {
            var item = new DbcManager.AirDirectorPlaylistItem
            {
                ID = id,
                Date = schedule.ScheduleDate,
                SlotTime = schedule.SlotTime,
                SequenceOrder = sequenceOrder,
                FileType = schedule.FileType,
                FilePath = schedule.FilePath,
                Duration = schedule.Duration
            };

            // Se è uno SPOT, aggiungi info dettagliate
            if (schedule.FileType == "SPOT" && schedule.ClientID > 0)
            {
                var client = clients.FirstOrDefault(c => c.ID == schedule.ClientID);
                var spot = spots.FirstOrDefault(s => s.ID == schedule.SpotID);

                item.ClientName = client?.ClientName ?? "";
                item.SpotTitle = spot?.SpotTitle ?? "";
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
        /// Salva il file ADV_AirDirector.dbc con separatore ; e campi tra apici doppi
        /// </summary>
        private static bool SaveAirDirectorDbc(List<DbcManager.AirDirectorPlaylistItem> playlist)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("\"ID\";\"Date\";\"SlotTime\";\"SequenceOrder\";\"FileType\";\"FilePath\";\"Duration\";\"ClientName\";\"SpotTitle\"");

                foreach (var item in playlist)
                {
                    sb.AppendLine(string.Join(";", new[]
                    {
                        $"\"{item.ID}\"",
                        $"\"{item.Date.ToString(DATE_FORMAT)}\"",
                        $"\"{Escape(item.SlotTime)}\"",
                        $"\"{item.SequenceOrder}\"",
                        $"\"{Escape(item.FileType)}\"",
                        $"\"{Escape(item.FilePath)}\"",
                        $"\"{item.Duration}\"",
                        $"\"{Escape(item.ClientName)}\"",
                        $"\"{Escape(item.SpotTitle)}\""
                    }));
                }

                string csvContent = sb.ToString();
                string fileNameOnly = EXPORT_FILENAME;
                bool centralSuccess = false;
                bool stationSuccess = false;

                try
                {
                    string centralPath = Path.Combine(ConfigManager.DATABASE_PATH, fileNameOnly);
                    Directory.CreateDirectory(Path.GetDirectoryName(centralPath));
                    File.WriteAllText(centralPath, csvContent, Encoding.UTF8);
                    centralSuccess = true;
                    Console.WriteLine($"[AirDirectorExport] ✅ Salvato in Database CENTRALE: {fileNameOnly}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AirDirectorExport] ⚠️ Errore salvataggio centrale {fileNameOnly}: {ex.Message}");
                }

                try
                {
                    string stationPath = Path.Combine(ConfigManager.CurrentStationDatabasePath, fileNameOnly);
                    Directory.CreateDirectory(Path.GetDirectoryName(stationPath));
                    File.WriteAllText(stationPath, csvContent, Encoding.UTF8);
                    stationSuccess = true;
                    Console.WriteLine($"[AirDirectorExport] ✅ Salvato in Database EMITTENTE: {fileNameOnly} ({playlist.Count} record)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AirDirectorExport] ❌ Errore salvataggio emittente {fileNameOnly}: {ex.Message}");
                }

                return centralSuccess || stationSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AirDirectorExport] ❌ Errore salvataggio {EXPORT_FILENAME}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Carica il file ADV_AirDirector.dbc nel formato con separatore ; e campi tra apici doppi
        /// </summary>
        private static List<DbcManager.AirDirectorPlaylistItem> LoadAirDirectorDbc()
        {
            var result = new List<DbcManager.AirDirectorPlaylistItem>();
            try
            {
                string filePath = Path.Combine(ConfigManager.CurrentStationDatabasePath, EXPORT_FILENAME);
                if (!File.Exists(filePath))
                    filePath = Path.Combine(ConfigManager.DATABASE_PATH, EXPORT_FILENAME);

                if (!File.Exists(filePath))
                    return result;

                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var fields = line.Split(';')
                        .Select(f => f.Trim().Trim('"').Replace("\"\"", "\""))
                        .ToArray();

                    if (fields.Length < 9)
                        continue;

                    var item = new DbcManager.AirDirectorPlaylistItem();
                    if (int.TryParse(fields[0], out int id)) item.ID = id;
                    if (DateTime.TryParseExact(fields[1], DATE_FORMAT,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime date)) item.Date = date;
                    item.SlotTime = fields[2];
                    if (int.TryParse(fields[3], out int seq)) item.SequenceOrder = seq;
                    item.FileType = fields[4];
                    item.FilePath = fields[5];
                    if (int.TryParse(fields[6], out int dur)) item.Duration = dur;
                    item.ClientName = fields[7];
                    item.SpotTitle = fields[8];

                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AirDirectorExport] ⚠️ Errore caricamento {EXPORT_FILENAME}: {ex.Message}");
            }
            return result;
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
                    "# \"ID\";\"Date\";\"SlotTime\";\"SequenceOrder\";\"FileType\";\"FilePath\";\"Duration\";\"ClientName\";\"SpotTitle\""
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

                var playlist = LoadAirDirectorDbc();

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
                var playlist = LoadAirDirectorDbc();

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

        private static string Escape(string value)
        {
            return (value ?? "").Replace("\"", "\"\"");
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