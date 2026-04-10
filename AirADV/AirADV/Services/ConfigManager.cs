using System;
using System.IO;

namespace AirADV.Services
{
    public static class ConfigManager
    {
        // ═══════════════════════════════════════════════════════════
        // PERCORSI BASE
        // ═══════════════════════════════════════════════════════════

        public static readonly string BASE_PATH = @"C:\AirADV";
        public static readonly string DATABASE_PATH = Path.Combine(BASE_PATH, "Database");
        public static readonly string STATIONS_PATH = Path.Combine(BASE_PATH, "Stations");
        public static readonly string MEDIA_PATH = Path.Combine(BASE_PATH, "Media");
        public static readonly string REPORTS_PATH = Path.Combine(BASE_PATH, "Reports");
        public static readonly string BACKUP_PATH = Path.Combine(BASE_PATH, "Backups");
        public static readonly string LOGS_PATH = Path.Combine(BASE_PATH, "Logs");
        public static readonly string LANGUAGES_PATH = Path.Combine(BASE_PATH, "Languages"); // ✅ AGGIUNTO
        private const string REGISTRY_KEY_AIRADV = @"SOFTWARE\AirADV";
        private const string MODE_VALUE_NAME = "StationMode";


        // ✅ Alias per compatibilità con codice vecchio
        public static string DatabasePath => DATABASE_PATH;
        public static string LogsPath => LOGS_PATH;

        // ═══════════════════════════════════════════════════════════
        // PERCORSI DINAMICI EMITTENTE CORRENTE
        // ═══════════════════════════════════════════════════════════

        public static string CurrentStationDatabasePath
        {
            get
            {
                if (CurrentStationID <= 0)
                    return DATABASE_PATH;

                try
                {
                    var stations = DbcManager.Load<DbcManager.Station>(Path.Combine(DATABASE_PATH, "ADV_Config.dbc"));
                    var station = stations.Find(s => s.StationID == CurrentStationID);

                    if (station != null && !string.IsNullOrEmpty(station.DatabasePath))
                    {
                        return station.DatabasePath;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] Errore CurrentStationDatabasePath: {ex.Message}");
                }

                return DATABASE_PATH;
            }
        }

        public static string CurrentStationMediaPath
        {
            get
            {
                if (CurrentStationID <= 0)
                    return MEDIA_PATH;

                try
                {
                    var stations = DbcManager.Load<DbcManager.Station>(Path.Combine(DATABASE_PATH, "ADV_Config.dbc"));
                    var station = stations.Find(s => s.StationID == CurrentStationID);

                    if (station != null && !string.IsNullOrEmpty(station.MediaPath))
                    {
                        return station.MediaPath;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] Errore CurrentStationMediaPath: {ex.Message}");
                }

                return MEDIA_PATH;
            }
        }

        public static string CurrentStationReportsPath
        {
            get
            {
                if (CurrentStationID <= 0)
                    return REPORTS_PATH;

                try
                {
                    var stations = DbcManager.Load<DbcManager.Station>(Path.Combine(DATABASE_PATH, "ADV_Config.dbc"));
                    var station = stations.Find(s => s.StationID == CurrentStationID);

                    if (station != null && !string.IsNullOrEmpty(station.ReportsPath))
                    {
                        return station.ReportsPath;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] Errore CurrentStationReportsPath: {ex.Message}");
                }

                return REPORTS_PATH;
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CONFIGURAZIONE
        // ═══════════════════════════════════════════════════════════

        private static int _currentStationID = 0;
        public static int CurrentStationID
        {
            get { return _currentStationID; }
            set { _currentStationID = value; }
        }

        private static string _language = "it-IT";
        public static string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private static bool _autoBackup = true;
        public static bool AutoBackup
        {
            get { return _autoBackup; }
            set { _autoBackup = value; }
        }

        private static int _backupRetentionDays = 30;
        public static int BackupRetentionDays
        {
            get { return _backupRetentionDays; }
            set { _backupRetentionDays = value; }
        }

        private static string _mediaLibraryPath = "";
        public static string MediaLibraryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_mediaLibraryPath))
                    return CurrentStationMediaPath;

                return _mediaLibraryPath;
            }
            set { _mediaLibraryPath = value; }
        }

        public static string PdfOutputPath => CurrentStationReportsPath;

        private static bool _autoSave = false;
        public static bool AutoSave
        {
            get { return _autoSave; }
            set { _autoSave = value; }
        }

        private static int _outputDeviceNumber = -1;
        public static int OutputDeviceNumber
        {
            get { return _outputDeviceNumber; }
            set { _outputDeviceNumber = value; }
        }

        private static int _miniPlayerVolume = 80;
        public static int MiniPlayerVolume
        {
            get { return _miniPlayerVolume; }
            set { _miniPlayerVolume = value; }
        }

        private static int _lastCampaignNumber = 0;
        public static int LastCampaignNumber
        {
            get { return _lastCampaignNumber; }
            set { _lastCampaignNumber = value; }
        }

        // ═══════════════════════════════════════════════════════════
        // METODI
        // ═══════════════════════════════════════════════════════════

        static ConfigManager()
        {
            Initialize();
        }

        public static void Initialize()
        {
            try
            {
                EnsureDirectoriesExist();
                Load();
                Console.WriteLine("[ConfigManager] ✅ Inizializzazione completata");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore inizializzazione: {ex.Message}");
            }
        }

        public static void EnsureDirectoriesExist()
        {
            try
            {
                Directory.CreateDirectory(BASE_PATH);
                Directory.CreateDirectory(DATABASE_PATH);
                Directory.CreateDirectory(STATIONS_PATH);
                Directory.CreateDirectory(MEDIA_PATH);
                Directory.CreateDirectory(REPORTS_PATH);
                Directory.CreateDirectory(BACKUP_PATH);
                Directory.CreateDirectory(LOGS_PATH);
                Directory.CreateDirectory(LANGUAGES_PATH); // ✅ AGGIUNTO

                Console.WriteLine($"[ConfigManager] ✅ Cartelle base create/verificate");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore creazione cartelle: {ex.Message}");
            }
        }

        public static bool CreateStationFolders(string stationName, int stationID)
        {
            try
            {
                string safeName = string.Join("_", stationName.Split(Path.GetInvalidFileNameChars()));
                string stationFolder = Path.Combine(STATIONS_PATH, safeName);

                Directory.CreateDirectory(stationFolder);
                Directory.CreateDirectory(Path.Combine(stationFolder, "Database"));
                Directory.CreateDirectory(Path.Combine(stationFolder, "Media"));
                Directory.CreateDirectory(Path.Combine(stationFolder, "Reports"));

                Console.WriteLine($"[ConfigManager] ✅ Cartelle create per emittente: {stationName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore creazione cartelle emittente: {ex.Message}");
                return false;
            }
        }

        public static void CleanOldBackups()
        {
            try
            {
                if (!Directory.Exists(BACKUP_PATH))
                    return;

                var cutoffDate = DateTime.Now.AddDays(-BackupRetentionDays);
                var backupFolders = Directory.GetDirectories(BACKUP_PATH);

                int deletedCount = 0;

                foreach (var folder in backupFolders)
                {
                    var dirInfo = new DirectoryInfo(folder);
                    if (dirInfo.CreationTime < cutoffDate)
                    {
                        try
                        {
                            Directory.Delete(folder, true);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ConfigManager] ⚠️ Impossibile eliminare backup {folder}: {ex.Message}");
                        }
                    }
                }

                if (deletedCount > 0)
                {
                    Console.WriteLine($"[ConfigManager] ✅ Eliminati {deletedCount} backup obsoleti (> {BackupRetentionDays} giorni)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore pulizia backup: {ex.Message}");
            }
        }

        public static string StationMode
        {
            get
            {
                try
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_AIRADV, false))
                    {
                        if (key != null)
                        {
                            var value = key.GetValue(MODE_VALUE_NAME);
                            if (value != null)
                            {
                                string mode = value.ToString();
                                if (mode == "Radio" || mode == "Radio-TV")
                                    return mode;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] ❌ Errore lettura StationMode dal registro: {ex.Message}");
                }
                return "Radio"; // Default
            }
            set
            {
                try
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(REGISTRY_KEY_AIRADV, true))
                    {
                        key.SetValue(MODE_VALUE_NAME, value, Microsoft.Win32.RegistryValueKind.String);
                        Console.WriteLine($"[ConfigManager] ✅ StationMode salvato nel registro: {value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] ❌ Errore scrittura StationMode nel registro: {ex.Message}");
                }
            }
        }

        public static void Load()
        {
            try
            {
                string configPath = Path.Combine(DATABASE_PATH, "ADV_AppConfig.dbc");

                if (!File.Exists(configPath))
                {
                    Console.WriteLine("[ConfigManager] File config non trovato, creo default");
                    Save();
                    return;
                }

                var lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var parts = line.Split('=');
                    if (parts.Length != 2)
                        continue;

                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    switch (key)
                    {
                        case "CurrentStationID":
                            int.TryParse(value, out _currentStationID);
                            break;
                        case "Language":
                            _language = value;
                            break;
                        case "AutoBackup":
                            bool.TryParse(value, out _autoBackup);
                            break;
                        case "BackupRetentionDays":
                            int.TryParse(value, out _backupRetentionDays);
                            break;
                        case "MediaLibraryPath":
                            _mediaLibraryPath = value;
                            break;
                        case "AutoSave":
                            bool.TryParse(value, out _autoSave);
                            break;
                        case "OutputDeviceNumber":
                            int.TryParse(value, out _outputDeviceNumber);
                            break;
                        case "MiniPlayerVolume":
                            int.TryParse(value, out _miniPlayerVolume);
                            break;
                        case "LastCampaignNumber":
                            int.TryParse(value, out _lastCampaignNumber);
                            break;
                    }
                }

                Console.WriteLine($"[ConfigManager] ✅ Configurazione caricata");
                Console.WriteLine($"  - Emittente corrente: {_currentStationID}");
                Console.WriteLine($"  - Lingua: {_language}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore caricamento config: {ex.Message}");
            }
        }

        public static void Save()
        {
            try
            {
                string configPath = Path.Combine(DATABASE_PATH, "ADV_AppConfig.dbc");

                var lines = new[]
                {
                    "# AirADV Configuration File",
                    $"# Last Updated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    "",
                    $"CurrentStationID={_currentStationID}",
                    $"Language={_language}",
                    $"AutoBackup={_autoBackup}",
                    $"BackupRetentionDays={_backupRetentionDays}",
                    $"MediaLibraryPath={_mediaLibraryPath}",
                    $"AutoSave={_autoSave}",
                    $"OutputDeviceNumber={_outputDeviceNumber}",
                    $"MiniPlayerVolume={_miniPlayerVolume}",
                    $"LastCampaignNumber={_lastCampaignNumber}"
                };

                File.WriteAllLines(configPath, lines);
                Console.WriteLine("[ConfigManager] ✅ Configurazione salvata");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ❌ Errore salvataggio config:  {ex.Message}");
            }
        }

        public static void Dispose()
        {
            try
            {
                Save();
                Console.WriteLine("[ConfigManager] ✅ Dispose completato");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ConfigManager] ⚠️ Errore dispose: {ex.Message}");
            }
        }
    }
}