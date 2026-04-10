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
        private static string _currentLanguageFileName = "Italian"; // ✅ NUOVO:  nome file corrente
        private static string _languagesPath = "";

        public static event EventHandler LanguageChanged;

        public static string CurrentCulture => _currentCulture;
        public static string CurrentLanguageFileName => _currentLanguageFileName; // ✅ NUOVO
        public static string CurrentLanguage => _currentLanguageFileName;
        public static string CurrentLanguageName { get; private set; } = "Italiano";

        /// <summary>
        /// ✅ AGGIORNATO: Inizializza usando nome file (es: "Italian")
        /// </summary>
        public static void Initialize(string languageFileName = "Italian")
        {
            try
            {
                // ✅ PERCORSO 1: Cartella progetto (sviluppo/debug)
                string projectLanguagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Languages");

                // ✅ PERCORSO 2: Cartella sistema (produzione)
                string systemLanguagesPath = Path.Combine(ConfigManager.BASE_PATH, "Languages");

                // ✅ Priorità: usa cartella progetto se esiste, altrimenti sistema
                if (Directory.Exists(projectLanguagesPath))
                {
                    _languagesPath = projectLanguagesPath;
                    Console.WriteLine($"[LanguageManager] ✅ Usando cartella PROGETTO: {_languagesPath}");
                }
                else if (Directory.Exists(systemLanguagesPath))
                {
                    _languagesPath = systemLanguagesPath;
                    Console.WriteLine($"[LanguageManager] ✅ Usando cartella SISTEMA: {_languagesPath}");
                }
                else
                {
                    // Crea cartella sistema
                    _languagesPath = systemLanguagesPath;
                    Directory.CreateDirectory(_languagesPath);
                    Console.WriteLine($"[LanguageManager] ✅ Cartella sistema creata: {_languagesPath}");

                    // Crea file italiano di default
                    CreateDefaultItalianFile();
                }

                // ✅ DEBUG: Mostra file trovati
                if (Directory.Exists(_languagesPath))
                {
                    var iniFiles = Directory.GetFiles(_languagesPath, "*.ini");
                    Console.WriteLine($"[LanguageManager] 📂 File .ini trovati: {iniFiles.Length}");
                    foreach (var file in iniFiles)
                    {
                        Console.WriteLine($"[LanguageManager]   - {Path.GetFileName(file)}");
                    }
                }

                // ✅ Carica lingua usando nome file
                LoadLanguageByFileName(languageFileName);

                Console.WriteLine($"[LanguageManager] ✅ Inizializzato - Lingua: {CurrentLanguageName} (file: {_currentLanguageFileName})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] ❌ Errore inizializzazione: {ex.Message}");
                Console.WriteLine($"[LanguageManager] StackTrace: {ex.StackTrace}");

                // Fallback:  crea dizionario minimo per evitare crash
                _strings["Common.OK"] = "OK";
                _strings["Common.Cancel"] = "Annulla";
                _strings["Common.Error"] = "Errore";
            }
        }

        /// <summary>
        /// ✅ NUOVO: Carica lingua usando nome file (es: "Italian", "English")
        /// </summary>
        public static void LoadLanguageByFileName(string fileName)
        {
            try
            {
                _strings.Clear();
                _missingKeys.Clear();

                // Costruisci percorso completo
                string filePath = Path.Combine(_languagesPath, $"{fileName}.ini");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ File non trovato: {fileName}.ini");

                    // Fallback a Italian
                    filePath = Path.Combine(_languagesPath, "Italian.ini");

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"[LanguageManager] ⚠️ Italian.ini non trovato, lo creo");
                        CreateDefaultItalianFile();
                        filePath = Path.Combine(_languagesPath, "Italian.ini");
                    }

                    fileName = "Italian";
                }

                // Carica file INI
                LoadIniFile(filePath);

                // Aggiorna valori correnti
                _currentLanguageFileName = fileName;
                _currentCulture = _strings.ContainsKey("Language.Code") ? _strings["Language.Code"] : "it-IT";
                CurrentLanguageName = _strings.ContainsKey("Language.Name") ? _strings["Language.Name"] : fileName;

                // Notifica cambio lingua
                LanguageChanged?.Invoke(null, EventArgs.Empty);

                Console.WriteLine($"[LanguageManager] ✅ Lingua caricata: {CurrentLanguageName}");
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
        /// ⚠️ DEPRECATO: Usa LoadLanguageByFileName invece
        /// </summary>
        [Obsolete("Usa LoadLanguageByFileName invece")]
        public static void LoadLanguage(string culture)
        {
            // Converti codice cultura in nome file
            string fileName = culture switch
            {
                "it-IT" => "Italian",
                "en-US" => "English",
                "en-GB" => "English",
                "es-ES" => "Spanish",
                "fr-FR" => "French",
                "de-DE" => "German",
                _ => "Italian"
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
                int sectionIdx = existing.IndexOf("\n[MissingKeys]", StringComparison.Ordinal);
                if (sectionIdx < 0)
                    sectionIdx = existing.IndexOf("[MissingKeys]", StringComparison.Ordinal);
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
        /// ⚠️ DEPRECATO:  Usa LoadLanguageByFileName invece
        /// </summary>
        [Obsolete("Usa LoadLanguageByFileName invece")]
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
                // ✅ Usa _languagesPath già inizializzato
                if (string.IsNullOrEmpty(_languagesPath))
                {
                    Console.WriteLine("[LanguageManager] ⚠️ _languagesPath non inizializzato, chiamo Initialize");
                    Initialize();
                }

                if (!Directory.Exists(_languagesPath))
                {
                    Console.WriteLine($"[LanguageManager] ⚠️ Cartella non esiste: {_languagesPath}");
                    Directory.CreateDirectory(_languagesPath);
                    CreateDefaultItalianFile();
                }

                var files = Directory.GetFiles(_languagesPath, "*.ini");
                Console.WriteLine($"[LanguageManager] 📂 GetAvailableLanguages - Trovati {files.Length} file .ini");

                foreach (var file in files)
                {
                    try
                    {
                        var info = ReadLanguageInfo(file);
                        if (info != null)
                        {
                            languages.Add(info);
                            Console.WriteLine($"[LanguageManager]   ✅ Lingua:  {info.Name} (file: {Path.GetFileNameWithoutExtension(file)})");
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

                // Se nessun file trovato, crea italiano di default
                if (languages.Count == 0)
                {
                    Console.WriteLine("[LanguageManager] ⚠️ Nessuna lingua trovata, creo Italian.ini");
                    CreateDefaultItalianFile();

                    languages.Add(new LanguageInfo
                    {
                        Code = "it-IT",
                        Name = "Italiano",
                        Author = "AirADV Team",
                        Version = "1.0.0",
                        FilePath = Path.Combine(_languagesPath, "Italian.ini")
                    });
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
        /// Crea file Italian.ini di default
        /// </summary>
        private static void CreateDefaultItalianFile()
        {
            try
            {
                string filePath = Path.Combine(_languagesPath, "Italian.ini");

                if (File.Exists(filePath))
                    return;

                var content = new StringBuilder();
                content.AppendLine("[Language]");
                content.AppendLine("Name=Italiano");
                content.AppendLine("Code=it-IT");
                content.AppendLine("Author=AirADV Team");
                content.AppendLine("Version=1.0.0");
                content.AppendLine();
                content.AppendLine("[Common]");
                content.AppendLine("Save=💾 Salva");
                content.AppendLine("Cancel=✖ Annulla");
                content.AppendLine("Apply=✓ Applica");
                content.AppendLine("Close=Chiudi");
                content.AppendLine("OK=OK");
                content.AppendLine("Yes=Sì");
                content.AppendLine("No=No");
                content.AppendLine("Add=➕ Aggiungi");
                content.AppendLine("Delete=🗑️ Elimina");
                content.AppendLine("Edit=✏️ Modifica");
                content.AppendLine("Refresh=🔄 Aggiorna");
                content.AppendLine("Active=Attivo");
                content.AppendLine();
                content.AppendLine("[Configuration]");
                content.AppendLine("WindowTitle=⚙️ Configurazione");
                content.AppendLine("TabGeneral=🌐 Generale");
                content.AppendLine("TabPaths=📁 Percorsi");
                content.AppendLine("TabBackup=💾 Backup");
                content.AppendLine();
                content.AppendLine("[MainForm]");
                content.AppendLine("WindowTitle=AirADV - Gestionale Pubblicitario");
                content.AppendLine("Title=Gestionale Pubblicitario v1.0.0");
                content.AppendLine("BtnClients=👥 Clienti");
                content.AppendLine("BtnTimeSlots=🕐 Punti Orari");
                content.AppendLine("BtnCategories=🏷️ Categorie");
                content.AppendLine("BtnSchedule=📅 Palinsesto");
                content.AppendLine("BtnReports=📊 Report");
                content.AppendLine("BtnSettings=⚙️ Impostazioni");
                content.AppendLine("BtnChangeStation=🔄 Cambia Emittente");
                content.AppendLine("BtnExit=✖ Esci");
                content.AppendLine();
                content.AppendLine("[Messages]");
                content.AppendLine("ConfirmDelete=Sei sicuro di voler eliminare questo elemento?");
                content.AppendLine("SaveSuccess=Salvataggio completato con successo!");
                content.AppendLine("SaveError=Errore durante il salvataggio.");
                content.AppendLine("NoSelection=Nessun elemento selezionato.");

                File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);

                Console.WriteLine($"[LanguageManager] ✅ File Italian.ini creato: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LanguageManager] Errore creazione Italian.ini: {ex.Message}");
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