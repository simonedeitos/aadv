using AirADV.Forms;
using AirADV.Services;
using AirADV.Services.Licensing;
using AirADV.Services.Localization;
using System;
using System.Windows.Forms;

namespace AirADV
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("   AirADV - Gestionale Pubblicitario v1.0.0");
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine();

                // ✅ 1.Inizializza ConfigManager
                Console.WriteLine("[Program] 🔧 Inizializzazione ConfigManager...");
                ConfigManager.Initialize();

                Console.WriteLine($"[Program] 📂 Database: {ConfigManager.DatabasePath}");
                Console.WriteLine($"[Program] 📂 Languages: {ConfigManager.LANGUAGES_PATH}");
                Console.WriteLine();

                // ✅ 2.Pulizia backup (se abilitato)
                if (ConfigManager.AutoBackup)
                {
                    Console.WriteLine("[Program] 🧹 Pulizia backup obsoleti...");
                    ConfigManager.CleanOldBackups();
                }

                // ✅ 3.Inizializza LanguageManager con nome file salvato
                string savedLanguage = ConfigManager.Language; // Es: "Italian" o "English"
                if (string.IsNullOrEmpty(savedLanguage))
                {
                    savedLanguage = "Italian"; // Default
                }

                Console.WriteLine($"[Program] 🌐 Caricamento lingua: {savedLanguage}");
                LanguageManager.Initialize(savedLanguage);

                Console.WriteLine();
                Console.WriteLine("[Program] ✅ Inizializzazione completata");
                Console.WriteLine();

                // ✅ 3.5 Verifica Licenza
                Console.WriteLine("[AirADV] Verifica licenza...");

                if (!LicenseManager.IsLicenseValid())
                {
                    Console.WriteLine("[AirADV] ⚠️ Licenza non valida, apertura form di attivazione...");
                    using (var licenseForm = new LicenseForm())
                    {
                        DialogResult result = licenseForm.ShowDialog();
                        if (result != DialogResult.OK)
                        {
                            Console.WriteLine("[AirADV] ❌ Attivazione licenza annullata. Chiusura applicazione.");
                            return;
                        }
                    }
                    Console.WriteLine("[AirADV] ✅ Licenza attivata con successo");
                }
                else
                {
                    Console.WriteLine("[AirADV] ✅ Licenza valida");
                }

                // ✅ 4.Avvio applicazione
                Console.WriteLine("[Program] 🚀 Avvio MainForm...");
                Application.Run(new MainForm());

                // ✅ 5.Chiusura
                Console.WriteLine();
                Console.WriteLine("[Program] 🔄 Chiusura applicazione...");
                ConfigManager.Dispose();
                Console.WriteLine("[Program] ✅ Applicazione chiusa");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("❌ ERRORE CRITICO");
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine($"Messaggio: {ex.Message}");
                Console.WriteLine($"Tipo: {ex.GetType().Name}");
                Console.WriteLine();
                Console.WriteLine("Stack Trace:");
                Console.WriteLine(ex.StackTrace);

                MessageBox.Show(
                    $"Errore critico all'avvio:\n\n{ex.Message}\n\nDettagli salvati nel log.",
                    "Errore AirADV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Salva log
                try
                {
                    string logFile = System.IO.Path.Combine(
                        ConfigManager.LogsPath,
                        $"crash_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                    );

                    string logContent = $"AirADV Crash Report\n" +
                                      $"Date: {DateTime.Now: yyyy-MM-dd HH: mm:ss}\n\n" +
                                      $"Message: {ex.Message}\n" +
                                      $"Type: {ex.GetType().Name}\n\n" +
                                      $"Stack Trace:\n{ex.StackTrace}";

                    System.IO.File.WriteAllText(logFile, logContent);
                    Console.WriteLine($"[Program] 📝 Log salvato: {logFile}");
                }
                catch (Exception logEx)
                {
                    Console.WriteLine($"[Program] ⚠️ Impossibile salvare log: {logEx.Message}");
                }
            }
        }
    }
}