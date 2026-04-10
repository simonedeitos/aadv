using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AirADV.Services.Localization
{
    /// <summary>
    /// Gestione multilingua tramite file .ini
    /// </summary>
    public static class LanguageManager
    {
        private static Dictionary<string, string> _strings = new Dictionary<string, string>();
        private static Dictionary<string, string> _missingKeys = new Dictionary<string, string>();
        private static string _currentCulture = "it-IT";
        private static string _currentLanguageFileName = "Italiano"; // nome file corrente
        private static string _languagesPath = "";

        public static event EventHandler LanguageChanged;

        public static string CurrentCulture => _currentCulture;
        /// <summary>
        /// Nome file lingua corrente (es: "Italian", "English")
        /// </summary>
        public static string CurrentLanguageFileName => _currentLanguageFileName; // ✅ NUOVO
        /// <summary>
        /// Alias di CurrentLanguageFileName per compatibilità con pattern AirManager
        /// </summary>
        public static string CurrentLanguage => _currentLanguageFileName;
        public static string CurrentLanguageName { get; private set; } = "Italiano";

        /// <summary>
        /// Inizializza usando nome file (es: "Italiano", "English")
        /// Legge sempre da exe\Resources\Languages
        /// </summary>
        public static void Initialize(string languageFileName = "Italiano")
        {
            try
            {
                // Legge SEMPRE da exe\Resources\Languages
                _languagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Languages");

                Console.WriteLine($"[LanguageManager] Cartella lingue: {_languagesPath}");

                if (!Directory.Exists(_languagesPath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ Cartella non trovata: {_languagesPath}");
                    // Fallback minimo in memoria, non crea file
                    _strings["Common.OK"] = "OK";
                    _strings["Common.Cancel"] = "Annulla";
                    _strings["Common.Error"] = "Errore";
                    return;
                }

                // Mostra file trovati
                var iniFiles = Directory.GetFiles(_languagesPath, "*.ini");
                Console.WriteLine($"[LanguageManager] File .ini trovati: {iniFiles.Length}");
                foreach (var file in iniFiles)
                {
                    Console.WriteLine($"[LanguageManager]   - {Path.GetFileName(file)}");
                }

                // Carica lingua usando nome file
                LoadLanguageByFileName(languageFileName);

                Console.WriteLine($"[LanguageManager] Inizializzato - Lingua: {CurrentLanguageName} (file: {_currentLanguageFileName})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] ❌ Errore inizializzazione: {ex.Message}");

                // Fallback: dizionario minimo per evitare crash
                _strings["Common.OK"] = "OK";
                _strings["Common.Cancel"] = "Annulla";
                _strings["Common.Error"] = "Errore";
            }
        }

        /// <summary>
        /// Carica lingua usando nome file (es: "Italiano", "English", "Deutsch")
        /// </summary>
        public static void LoadLanguageByFileName(string fileName)
        {
            try
            {
                _strings.Clear();
                _missingKeys.Clear();

                // Assicurati che il percorso sia inizializzato
                if (string.IsNullOrEmpty(_languagesPath))
                {
                    _languagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Languages");
                }

                // Costruisci percorso completo
                string filePath = Path.Combine(_languagesPath, $"{fileName}.ini");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ File non trovato: {fileName}.ini");

                    // Fallback a Italiano
                    string itPath = Path.Combine(_languagesPath, "Italiano.ini");
                    if (File.Exists(itPath))
                    {
                        filePath = itPath;
                        fileName = "Italiano";
                        Console.WriteLine($"[LanguageManager] Fallback a Italiano.ini");
                    }
                    else
                    {
                        Console.WriteLine($"[LanguageManager] ⚠️ Nessun file lingua trovato, uso fallback in memoria");
                        _currentLanguageFileName = fileName;
                        CurrentLanguageName = fileName;
                        _strings["Common.OK"] = "OK";
                        _strings["Common.Cancel"] = "Annulla";
                        _strings["Common.Error"] = "Errore";
                        return;
                    }
                }

                // Carica file INI
                LoadIniFile(filePath);

                // Aggiorna valori correnti
                _currentLanguageFileName = fileName;
                _currentCulture = _strings.ContainsKey("Language.Code") ? _strings["Language.Code"] : "it-IT";
                CurrentLanguageName = _strings.ContainsKey("Language.Name") ? _strings["Language.Name"] : fileName;

                // Notifica cambio lingua
                LanguageChanged?.Invoke(null, EventArgs.Empty);

                Console.WriteLine($"[LanguageManager] Lingua caricata: {CurrentLanguageName}");
                Console.WriteLine($"[LanguageManager]   File: {fileName}.ini");
                Console.WriteLine($"[LanguageManager]   Code: {_currentCulture}");
                Console.WriteLine($"[LanguageManager]   Chiavi: {_strings.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] ❌ Errore LoadLanguageByFileName: {ex.Message}");
            }
        }

        /// <summary>
        /// Deprecato: Usa LoadLanguageByFileName invece
        /// </summary>
        [Obsolete("Usa LoadLanguageByFileName invece")]
        public static void LoadLanguage(string culture)
        {
            // Converti codice cultura in nome file
            string fileName = culture switch
            {
                "it-IT" => "Italiano",
                "en-US" => "English",
                "en-GB" => "English",
                "es-ES" => "Español",
                "fr-FR" => "Français",
                "de-DE" => "Deutsch",
                _ => "Italiano"
            };

            LoadLanguageByFileName(fileName);
        }

        /// <summary>
        /// Legge file INI e popola dizionario
        /// </summary>
        private static void LoadIniFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
                string currentSection = "";

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();

                    // Ignora commenti e righe vuote
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                        continue;

                    // Sezione [SectionName]
                    if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    {
                        currentSection = trimmed.Substring(1, trimmed.Length - 2);
                        continue;
                    }

                    // Chiave=Valore
                    int equalPos = trimmed.IndexOf('=');
                    if (equalPos > 0)
                    {
                        string key = trimmed.Substring(0, equalPos).Trim();
                        string value = trimmed.Substring(equalPos + 1).Trim();

                        // Chiave completa:  Section.Key
                        string fullKey = string.IsNullOrEmpty(currentSection)
                            ? key
                            : $"{currentSection}.{key}";

                        _strings[fullKey] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] Errore lettura INI: {ex.Message}");
            }
        }

        /// <summary>
        /// Ottiene una stringa tradotta
        /// </summary>
        public static string Get(string key, string defaultValue = "")
        {
            if (_strings.ContainsKey(key))
            {
                return _strings[key];
            }

            // Traccia chiavi mancanti
            string fallback = string.IsNullOrEmpty(defaultValue) ? key : defaultValue;
            _missingKeys[key] = fallback;

            // Log solo in debug
#if DEBUG
            Console.WriteLine($"[LanguageManager] ⚠️ Chiave mancante: {key}");
#endif

            return fallback;
        }

        /// <summary>
        /// Alias per compatibilità con pattern AirManager - chiama Get()
        /// </summary>
        public static string GetString(string key, string defaultValue = "")
        {
            return Get(key, defaultValue);
        }

        /// <summary>
        /// Cambia la lingua e notifica l'evento LanguageChanged
        /// </summary>
        public static void SetLanguage(string languageName)
        {
            LoadLanguageByFileName(languageName);
        }

        /// <summary>
        /// Restituisce il dizionario delle chiavi mancanti per diagnostica
        /// </summary>
        public static Dictionary<string, string> GetMissingKeys()
        {
            return new Dictionary<string, string>(_missingKeys);
        }

        /// <summary>
        /// Salva le chiavi mancanti nel file lingua corrente sotto la sezione [MissingKeys]
        /// </summary>
        public static void SaveMissingKeysToFile()
        {
            if (_missingKeys.Count == 0 || string.IsNullOrEmpty(_languagesPath))
                return;

            try
            {
                string filePath = Path.Combine(_languagesPath, $"{_currentLanguageFileName}.ini");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ File non trovato per SaveMissingKeysToFile: {filePath}");
                    return;
                }

                // Leggi contenuto esistente
                string existing = File.ReadAllText(filePath, Encoding.UTF8);

                // Rimuovi eventuale sezione [MissingKeys] precedente
                int sectionIdx = existing.IndexOf("[MissingKeys]", StringComparison.Ordinal);
                if (sectionIdx >= 0)
                    existing = existing.Substring(0, sectionIdx).TrimEnd();

                var sb = new StringBuilder(existing);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("[MissingKeys]");
                foreach (var kvp in _missingKeys)
                {
                    sb.AppendLine($"{kvp.Key}={kvp.Value}");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                Console.WriteLine($"[LanguageManager] ✅ Chiavi mancanti salvate: {_missingKeys.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] ❌ Errore SaveMissingKeysToFile: {ex.Message}");
            }
        }

        /// <summary>
        /// Deprecato: Usa SetLanguage invece
        /// </summary>
        [Obsolete("Usa SetLanguage invece")]
        public static void ChangeLanguage(string culture)
        {
            LoadLanguage(culture);
            ConfigManager.Language = _currentLanguageFileName;
            ConfigManager.Save();
        }

        /// <summary>
        /// Ottiene lista lingue disponibili
        /// </summary>
        public static List<LanguageInfo> GetAvailableLanguages()
        {
            var languages = new List<LanguageInfo>();

            try
            {
                if (string.IsNullOrEmpty(_languagesPath))
                {
                    _languagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Languages");
                }

                if (!Directory.Exists(_languagesPath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ Cartella non esiste: {_languagesPath}");
                    return languages;
                }

                var files = Directory.GetFiles(_languagesPath, "*.ini");
                Console.WriteLine($"[LanguageManager] GetAvailableLanguages - Trovati {files.Length} file .ini");

                foreach (var file in files)
                {
                    try
                    {
                        var info = ReadLanguageInfo(file);
                        if (info != null)
                        {
                            languages.Add(info);
                            Console.WriteLine($"[LanguageManager]   Lingua: {info.Name} (file: {Path.GetFileNameWithoutExtension(file)})");
                        }
                        else
                        {
                            Console.WriteLine($"[LanguageManager]   ⚠️ Info non valide: {Path.GetFileName(file)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LanguageManager]   ❌ Errore lettura {Path.GetFileName(file)}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] ❌ Errore GetAvailableLanguages: {ex.Message}");
            }

            return languages.OrderBy(l => l.Name).ToList();
        }

        /// <summary>
        /// Legge info lingua da file
        /// </summary>
        private static LanguageInfo ReadLanguageInfo(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8).Take(20);
                var info = new LanguageInfo { FilePath = filePath };

                foreach (var line in lines)
                {
                    string trimmed = line.Trim();

                    if (trimmed.StartsWith("Code="))
                        info.Code = trimmed.Substring(5).Trim();
                    else if (trimmed.StartsWith("Name="))
                        info.Name = trimmed.Substring(5).Trim();
                    else if (trimmed.StartsWith("Author="))
                        info.Author = trimmed.Substring(7).Trim();
                    else if (trimmed.StartsWith("Version="))
                        info.Version = trimmed.Substring(8).Trim();
                }

                // Valida info minime
                if (string.IsNullOrEmpty(info.Code) || string.IsNullOrEmpty(info.Name))
                {
                    info.Code = Path.GetFileNameWithoutExtension(filePath);
                    info.Name = info.Code;
                }

                return info;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Classe info lingua
        /// </summary>
        public class LanguageInfo
        {
            public string Code { get; set; } = "";
            public string Name { get; set; } = "";
            public string Author { get; set; } = "";
            public string Version { get; set; } = "1.0.0";
            public string FilePath { get; set; } = "";

            public override string ToString() => Name;
        }
    }
}