using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AirADV.Services
{
    /// <summary>
    /// Gestione completa file .dbc (CSV) con sistema multi-emittente
    /// Usa ConfigManager per percorsi dinamici Database centrale + Database emittente
    /// </summary>
    public static class DbcManager
    {
        // ═══════════════════════════════════════════════════════════
        // FILE GLOBALI (sempre in Database centrale)
        // ═══════════════════════════════════════════════════════════

        private static readonly string[] GLOBAL_FILES = new[]
        {
            "ADV_Config.dbc",         // Configurazione emittenti
            "ADV_Categories.dbc",     // Categorie condivise
            "ADV_AppConfig.dbc"       // Configurazione applicazione
        };

        // ✅ Determina se il file è globale o specifico emittente
        private static bool IsGlobalFile(string fileName)
        {
            return GLOBAL_FILES.Contains(Path.GetFileName(fileName));
        }

        // ✅ Ottieni percorso completo file
        private static string GetFilePath(string fileName)
        {
            string fileNameOnly = Path.GetFileName(fileName);

            // File globali sempre in Database centrale
            if (IsGlobalFile(fileNameOnly))
            {
                return Path.Combine(ConfigManager.DATABASE_PATH, fileNameOnly);
            }

            // File specifici in Database emittente
            return Path.Combine(ConfigManager.CurrentStationDatabasePath, fileNameOnly);
        }

        // ═══════════════════════════════════════════════════════════
        // LOAD
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Carica dati da file .dbc (usa automaticamente percorsi corretti)
        /// </summary>
        public static List<T> Load<T>(string fileName) where T : class, new()
        {
            string filePath = GetFilePath(fileName);
            var result = new List<T>();

            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[DbcManager] ⚠️ File non trovato: {fileName}, ritorno lista vuota");
                    return result;
                }

                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                if (lines.Length < 2)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ File vuoto o senza header: {fileName}");
                    return result;
                }

                string[] headers = lines[0].Split(',');
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                        continue;

                    string[] values = SplitCsvLine(lines[i]);
                    var obj = new T();

                    for (int j = 0; j < headers.Length && j < values.Length; j++)
                    {
                        var prop = properties.FirstOrDefault(p => p.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase));
                        if (prop != null && prop.CanWrite)
                        {
                            try
                            {
                                object value = ConvertValue(values[j], prop.PropertyType);
                                prop.SetValue(obj, value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[DbcManager] ⚠️ Errore conversione {headers[j]}: {ex.Message}");
                            }
                        }
                    }

                    result.Add(obj);
                }

                Console.WriteLine($"[DbcManager] ✅ Caricati {result.Count} record da {fileName} ({filePath})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore caricamento {fileName}: {ex.Message}");
            }

            return result;
        }

        // ═══════════════════════════════════════════════════════════
        // SAVE - CON DOPPIO SALVATAGGIO E SOVRASCRITTURA COMPLETA
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// ✅ Salva dati su file .dbc con SOVRASCRITTURA COMPLETA
        /// File viene SEMPRE sostituito completamente, mai append
        /// </summary>
        public static bool Save<T>(string fileName, List<T> data) where T : class
        {
            try
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var sb = new StringBuilder();

                // Header
                sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

                // Data
                foreach (var item in data)
                {
                    var values = properties.Select(p =>
                    {
                        var value = p.GetValue(item);
                        return EscapeCsvValue(value?.ToString() ?? "");
                    });
                    sb.AppendLine(string.Join(",", values));
                }

                string csvContent = sb.ToString();
                string fileNameOnly = Path.GetFileName(fileName);

                // File globali:  salva solo in centrale
                if (IsGlobalFile(fileNameOnly))
                {
                    string centralPath = Path.Combine(ConfigManager.DATABASE_PATH, fileNameOnly);
                    Directory.CreateDirectory(Path.GetDirectoryName(centralPath));

                    // ✅ SOVRASCRITTURA COMPLETA
                    File.WriteAllText(centralPath, csvContent, Encoding.UTF8);

                    Console.WriteLine($"[DbcManager] ✅ Salvato GLOBALE: {fileNameOnly} ({data.Count} record)");
                    return true;
                }

                // File specifici: doppio salvataggio
                bool centralSuccess = false;
                bool stationSuccess = false;

                // 1.Salvataggio CENTRALE (backup)
                try
                {
                    string centralPath = Path.Combine(ConfigManager.DATABASE_PATH, fileNameOnly);
                    Directory.CreateDirectory(Path.GetDirectoryName(centralPath));

                    // ✅ SOVRASCRITTURA COMPLETA
                    File.WriteAllText(centralPath, csvContent, Encoding.UTF8);
                    centralSuccess = true;
                    Console.WriteLine($"[DbcManager] ✅ Salvato in Database CENTRALE: {fileNameOnly}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Errore salvataggio centrale {fileNameOnly}: {ex.Message}");
                }

                // 2.Salvataggio EMITTENTE (operativo)
                try
                {
                    string stationPath = Path.Combine(ConfigManager.CurrentStationDatabasePath, fileNameOnly);
                    Directory.CreateDirectory(Path.GetDirectoryName(stationPath));

                    // ✅ SOVRASCRITTURA COMPLETA
                    File.WriteAllText(stationPath, csvContent, Encoding.UTF8);
                    stationSuccess = true;
                    Console.WriteLine($"[DbcManager] ✅ Salvato in Database EMITTENTE: {fileNameOnly} ({data.Count} record)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DbcManager] ❌ Errore salvataggio emittente {fileNameOnly}:  {ex.Message}");
                }

                return centralSuccess || stationSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore salvataggio {fileName}: {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // INSERT
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Inserisce un nuovo record con auto-increment ID
        /// </summary>
        public static bool Insert<T>(string fileName, T entry) where T : class, new()
        {
            try
            {
                var data = Load<T>(fileName);

                // Auto-increment ID
                var idProp = typeof(T).GetProperty("ID");
                if (idProp != null && idProp.PropertyType == typeof(int))
                {
                    int maxId = data.Count > 0 ? data.Max(x => (int)idProp.GetValue(x)) : 0;
                    idProp.SetValue(entry, maxId + 1);
                }

                data.Add(entry);
                bool success = Save(fileName, data);

                if (success)
                {
                    int id = idProp != null ? (int)idProp.GetValue(entry) : 0;
                    Console.WriteLine($"[DbcManager] ✅ Insert: {fileName} - ID {id}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore insert {fileName}: {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // UPDATE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Aggiorna un record esistente per ID
        /// </summary>
        public static bool Update<T>(string fileName, T entry) where T : class, new()
        {
            try
            {
                var data = Load<T>(fileName);
                var idProp = typeof(T).GetProperty("ID");

                if (idProp == null)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Tipo {typeof(T).Name} non ha proprietà ID");
                    return false;
                }

                int id = (int)idProp.GetValue(entry);
                var existing = data.FirstOrDefault(x => (int)idProp.GetValue(x) == id);

                if (existing == null)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Record con ID {id} non trovato");
                    return false;
                }

                data.Remove(existing);
                data.Add(entry);

                bool success = Save(fileName, data);

                if (success)
                {
                    Console.WriteLine($"[DbcManager] ✅ Update: {fileName} - ID {id}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore update {fileName}:  {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // DELETE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Elimina un record per ID
        /// </summary>
        public static bool Delete<T>(string fileName, int id) where T : class, new()
        {
            try
            {
                var data = Load<T>(fileName);
                var idProp = typeof(T).GetProperty("ID");

                if (idProp == null)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Tipo {typeof(T).Name} non ha proprietà ID");
                    return false;
                }

                var toRemove = data.FirstOrDefault(x => (int)idProp.GetValue(x) == id);
                if (toRemove == null)
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Record ID {id} non trovato");
                    return false;
                }

                data.Remove(toRemove);
                bool success = Save(fileName, data);

                if (success)
                {
                    Console.WriteLine($"[DbcManager] ✅ Delete: {fileName} - ID {id}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore delete {fileName}: {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // ✅ NUOVO METODO:  PULIZIA DATI VECCHI
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// ✅ Rimuove dati precedenti a una data specifica (es.ieri)
        /// Utile per mantenere solo palinsesto attuale
        /// </summary>
        public static bool RemoveOldData<T>(string fileName, Func<T, DateTime> dateSelector, DateTime cutoffDate) where T : class, new()
        {
            try
            {
                var data = Load<T>(fileName);
                int originalCount = data.Count;

                // Filtra solo dati >= cutoffDate
                var filteredData = data.Where(item => dateSelector(item) >= cutoffDate).ToList();

                int removedCount = originalCount - filteredData.Count;

                if (removedCount > 0)
                {
                    bool success = Save(fileName, filteredData);

                    if (success)
                    {
                        Console.WriteLine($"[DbcManager] ✅ Rimossi {removedCount} record vecchi da {fileName}");
                    }

                    return success;
                }

                Console.WriteLine($"[DbcManager] ℹ️ Nessun dato vecchio da rimuovere in {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore rimozione dati vecchi {fileName}: {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // UTILITY - Backup
        // ═══════════════════════════════════════════════════════════

        public static bool BackupDatabase(int stationID)
        {
            try
            {
                var stations = Load<Station>("ADV_Config.dbc");
                var station = stations.FirstOrDefault(s => s.StationID == stationID);

                if (station == null || string.IsNullOrEmpty(station.DatabasePath))
                {
                    Console.WriteLine($"[DbcManager] ⚠️ Emittente {stationID} non trovata");
                    return false;
                }

                string backupFolder = Path.Combine(ConfigManager.BACKUP_PATH,
                    $"{station.StationName}_{DateTime.Now:yyyyMMdd_HHmmss}");

                Directory.CreateDirectory(backupFolder);

                var dbFiles = Directory.GetFiles(station.DatabasePath, "*.dbc");

                foreach (var file in dbFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(backupFolder, fileName);
                    File.Copy(file, destFile, true);
                }

                Console.WriteLine($"[DbcManager] ✅ Backup creato:  {backupFolder} ({dbFiles.Length} file)");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbcManager] ❌ Errore backup: {ex.Message}");
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // HELPER METHODS
        // ═══════════════════════════════════════════════════════════

        private static string[] SplitCsvLine(string line)
        {
            var result = new List<string>();
            var currentValue = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            result.Add(currentValue.ToString());
            return result.ToArray();
        }

        private static string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }

        private static object ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (targetType.IsValueType)
                    return Activator.CreateInstance(targetType);
                return null;
            }

            if (targetType == typeof(string))
                return value;

            if (targetType == typeof(int))
                return int.Parse(value);

            if (targetType == typeof(bool))
                return value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase);

            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);

            if (targetType == typeof(TimeSpan))
                return TimeSpan.Parse(value);

            if (targetType == typeof(decimal))
                return decimal.Parse(value);

            if (targetType == typeof(double))
                return double.Parse(value);

            return Convert.ChangeType(value, targetType);
        }

        // ═══════════════════════════════════════════════════════════
        // MODELLI DATI - COMPLETI E FINALI
        // ═══════════════════════════════════════════════════════════

        public class Station
        {
            public int ID { get; set; }
            public int StationID { get; set; }
            public string StationName { get; set; }
            public string Frequency { get; set; }
            public string LogoPath { get; set; }

            // Percorsi specifici emittente
            public string DatabasePath { get; set; }
            public string MediaPath { get; set; }
            public string ReportsPath { get; set; }

            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public Station()
            {
                StationName = "";
                Frequency = "";
                LogoPath = "";
                DatabasePath = "";
                MediaPath = "";
                ReportsPath = "";
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class TimeSlot
        {
            public int ID { get; set; }
            public int StationID { get; set; }
            public string SlotTime { get; set; }
            public string SlotName { get; set; }
            public int CategoryID { get; set; }
            public string OpeningFile { get; set; }
            public string InfraSpotFile { get; set; }
            public string ClosingFile { get; set; }
            public int ExpectedAudience { get; set; }
            public int MaxDuration { get; set; }
            public int Priority { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public TimeSlot()
            {
                SlotTime = "00:00:00";
                SlotName = "";
                CategoryID = 0;
                OpeningFile = "";
                InfraSpotFile = "";
                ClosingFile = "";
                ExpectedAudience = 0;
                MaxDuration = 120;
                Priority = 1;
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class Client
        {
            public int ID { get; set; }
            public string ClientCode { get; set; }
            public string ClientName { get; set; }
            public string CompanyName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string VATNumber { get; set; }
            public string Notes { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public Client()
            {
                ClientCode = "";
                ClientName = "";
                CompanyName = "";
                Address = "";
                City = "";
                PostalCode = "";
                Phone = "";
                Email = "";
                VATNumber = "";
                Notes = "";
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class Spot
        {
            public int ID { get; set; }
            public int ClientID { get; set; }
            public string SpotCode { get; set; }
            public string SpotTitle { get; set; }
            public string FilePath { get; set; }
            public int Duration { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime ValidTo { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public Spot()
            {
                SpotCode = "";
                SpotTitle = "";
                FilePath = "";
                Duration = 0;
                ValidFrom = DateTime.Now;
                ValidTo = DateTime.Now.AddYears(1);
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class Category
        {
            public int ID { get; set; }
            public string CategoryCode { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
            public string Color { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public Category()
            {
                CategoryCode = "";
                CategoryName = "";
                Description = "";
                Color = "#808080";
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class Campaign
        {
            public int ID { get; set; }
            public int StationID { get; set; }
            public int ClientID { get; set; }
            public int SpotID { get; set; }
            public int CategoryID { get; set; }
            public string CampaignCode { get; set; }
            public string CampaignName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int DailyPasses { get; set; }
            public string TimeFrom { get; set; }
            public string TimeTo { get; set; }
            public bool Monday { get; set; }
            public bool Tuesday { get; set; }
            public bool Wednesday { get; set; }
            public bool Thursday { get; set; }
            public bool Friday { get; set; }
            public bool Saturday { get; set; }
            public bool Sunday { get; set; }
            public string DistributionMode { get; set; }
            public string ManualSlots { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public Campaign()
            {
                CampaignCode = "";
                CampaignName = "";
                StartDate = DateTime.Now;
                EndDate = DateTime.Now.AddMonths(1);
                DailyPasses = 0;
                TimeFrom = "00:00:00";
                TimeTo = "23:59:59";
                Monday = true;
                Tuesday = true;
                Wednesday = true;
                Thursday = true;
                Friday = true;
                Saturday = true;
                Sunday = true;
                DistributionMode = "BALANCED";
                ManualSlots = "";
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }

        public class Schedule
        {
            public int ID { get; set; }
            public int StationID { get; set; }
            public DateTime ScheduleDate { get; set; }
            public string SlotTime { get; set; }
            public int SequenceOrder { get; set; }
            public string FileType { get; set; }
            public string FilePath { get; set; }
            public int ClientID { get; set; }
            public int SpotID { get; set; }
            public int CampaignID { get; set; }
            public int Duration { get; set; }
            public bool IsManual { get; set; }

            public Schedule()
            {
                ScheduleDate = DateTime.Now;
                SlotTime = "00:00:00";
                SequenceOrder = 0;
                FileType = "";
                FilePath = "";
                ClientID = 0;
                SpotID = 0;
                CampaignID = 0;
                Duration = 0;
                IsManual = false;
            }
        }

        public class AirDirectorPlaylistItem
        {
            public int ID { get; set; }
            public DateTime Date { get; set; }
            public string SlotTime { get; set; }
            public int SequenceOrder { get; set; }
            public string FileType { get; set; } // OPENING, SPOT, INFRASPOT, CLOSING
            public string FilePath { get; set; }
            public int Duration { get; set; }
            public string ClientName { get; set; }
            public string SpotTitle { get; set; }

            public AirDirectorPlaylistItem()
            {
                Date = DateTime.Now;
                SlotTime = "";
                FileType = "";
                FilePath = "";
                ClientName = "";
                SpotTitle = "";
            }
        }

        public class ConflictRule
        {
            public int ID { get; set; }
            public int StationID { get; set; }
            public int Category1ID { get; set; }
            public int Category2ID { get; set; }
            public int MinimumMinutesGap { get; set; }
            public string RuleType { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }

            public ConflictRule()
            {
                StationID = 0;
                MinimumMinutesGap = 60;
                RuleType = "SAME_SLOT";
                IsActive = true;
                CreatedDate = DateTime.Now;
            }
        }
    }
}