using System;
using System.Collections.Generic;
using System.Linq;
using AirADV.Services;

namespace AirADV.Services
{
    /// <summary>
    /// Engine completo per generazione, validazione e gestione schedulazioni
    /// ✅ NUOVO: Supporto evita ripetizioni e distribuzione uniforme
    /// </summary>
    public class ScheduleEngine
    {
        // ═══════════════════════════════════════════════════════════
        // GENERAZIONE SCHEDULAZIONE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// ✅ AGGIORNATO: Genera schedulazione automatica con NUOVE OPZIONI
        /// </summary>
        public List<DailySchedule> GenerateAutomatic(
            DbcManager.Campaign campaign,
            List<DbcManager.TimeSlot> availableSlots,
            bool avoidRepetition = true,      // ✅ NUOVO
            bool distributePasses = true)      // ✅ NUOVO
        {
            var result = new List<DailySchedule>();

            try
            {
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
                Console.WriteLine($"[ScheduleEngine] Generazione AUTOMATICA campagna: {campaign.CampaignName}");
                Console.WriteLine($"[ScheduleEngine] Periodo: {campaign.StartDate:dd/MM/yyyy} - {campaign.EndDate:dd/MM/yyyy}");
                Console.WriteLine($"[ScheduleEngine] Passaggi/giorno: {campaign.DailyPasses}");
                Console.WriteLine($"[ScheduleEngine] Fascia oraria: {campaign.TimeFrom} - {campaign.TimeTo}");
                Console.WriteLine($"[ScheduleEngine] Modalità:  {campaign.DistributionMode}");
                Console.WriteLine($"[ScheduleEngine] ✅ Evita ripetizioni: {avoidRepetition}");
                Console.WriteLine($"[ScheduleEngine] ✅ Distribuzione uniforme: {distributePasses}");

                // ✅ Filtra slot in base alla fascia oraria
                TimeSpan timeFrom = TimeSpan.Parse(campaign.TimeFrom);
                TimeSpan timeTo = TimeSpan.Parse(campaign.TimeTo);

                var validSlots = availableSlots.Where(s =>
                {
                    TimeSpan slotTime = TimeSpan.Parse(s.SlotTime);
                    return slotTime >= timeFrom && slotTime <= timeTo && s.IsActive;
                }).OrderBy(s => s.SlotTime).ToList();

                Console.WriteLine($"[ScheduleEngine] Slot disponibili nella fascia oraria: {validSlots.Count}");

                if (validSlots.Count == 0)
                {
                    Console.WriteLine($"[ScheduleEngine] ❌ ERRORE: Nessun punto orario disponibile nella fascia {campaign.TimeFrom}-{campaign.TimeTo}");
                    return result;
                }

                // ✅ Verifica se i passaggi richiesti sono compatibili
                if (campaign.DailyPasses > validSlots.Count)
                {
                    Console.WriteLine($"[ScheduleEngine] ⚠️ ATTENZIONE: Richiesti {campaign.DailyPasses} passaggi ma disponibili solo {validSlots.Count} slot");
                    Console.WriteLine($"[ScheduleEngine] Riduzione automatica a {validSlots.Count} passaggi/giorno");
                }

                // ✅ NUOVO: Genera con memoria orari del giorno precedente per evitare ripetizioni
                HashSet<string> previousDaySlots = new HashSet<string>();
                int totalDays = 0;

                for (DateTime date = campaign.StartDate.Date; date <= campaign.EndDate.Date; date = date.AddDays(1))
                {
                    if (!IsDayEnabled(campaign, date.DayOfWeek))
                        continue;

                    var dailySchedule = new DailySchedule
                    {
                        Date = date,
                        IsConfirmed = false,
                        IsModified = false
                    };

                    // ✅ Evita ripetizioni: escludi gli slot usati il giorno precedente.
                    // Determina la lista primaria (non ripetuti) e, se necessario, la lista secondaria
                    // (slot da ieri da usare solo come completamento).
                    List<DbcManager.TimeSlot> slotsForToday;
                    List<DbcManager.TimeSlot>? repeatedSlots = null; // solo nel caso di fallback
                    int needed = Math.Min(campaign.DailyPasses, validSlots.Count);

                    if (avoidRepetition && previousDaySlots.Count > 0)
                    {
                        var availableToday = validSlots.Where(s => !previousDaySlots.Contains(s.SlotTime)).ToList();

                        if (availableToday.Count >= needed)
                        {
                            // Ci sono abbastanza slot non usati ieri: usa solo quelli
                            slotsForToday = availableToday;
                        }
                        else
                        {
                            // Fallback: non ci sono abbastanza slot non ripetuti.
                            // Usa TUTTI quelli non ripetuti (slotsForToday) come priorità massima
                            // e conserva quelli ripetuti separatamente per il completamento.
                            slotsForToday = availableToday;
                            repeatedSlots = validSlots.Where(s => previousDaySlots.Contains(s.SlotTime)).ToList();
                        }
                    }
                    else
                    {
                        slotsForToday = validSlots;
                    }

                    // ✅ Distribuisci passaggi
                    if (repeatedSlots != null)
                    {
                        // Caso fallback avoidRepetition: distribuisci prima i non-ripetuti,
                        // poi completa con i ripetuti scegliendo quelli più distanti dagli orari già assegnati.
                        List<string> primary;
                        if (distributePasses && slotsForToday.Count > 0)
                        {
                            primary = campaign.DistributionMode == "AUDIENCE"
                                ? DistributeByAudience(slotsForToday, slotsForToday.Count)
                                : DistributeBalanced(slotsForToday, slotsForToday.Count);
                        }
                        else
                        {
                            primary = slotsForToday.Select(s => s.SlotTime).ToList();
                        }

                        int remainingNeeded = needed - primary.Count;
                        if (remainingNeeded > 0)
                        {
                            // Scegli gli slot ripetuti più distanti dagli orari già assegnati oggi
                            var primaryTimes = primary.Select(t => TimeSpan.Parse(t)).ToList();
                            var secondary = repeatedSlots
                                .OrderByDescending(s =>
                                {
                                    var st = TimeSpan.Parse(s.SlotTime);
                                    return primaryTimes.Count > 0
                                        ? primaryTimes.Min(pt => Math.Abs((st - pt).TotalMinutes))
                                        : 0.0;
                                })
                                .Take(remainingNeeded)
                                .Select(s => s.SlotTime)
                                .ToList();

                            dailySchedule.TimeSlots = primary.Concat(secondary)
                                .OrderBy(t => TimeSpan.Parse(t))
                                .ToList();
                        }
                        else
                        {
                            dailySchedule.TimeSlots = primary.OrderBy(t => TimeSpan.Parse(t)).ToList();
                        }

                        Console.WriteLine($"[ScheduleEngine] Fallback avoidRepetition: {primary.Count} slot non-ripetuti + {needed - primary.Count} ripetuti (più distanti)");
                    }
                    else if (distributePasses)
                    {
                        // Distribuzione uniforme lungo tutta la giornata
                        if (campaign.DistributionMode == "BALANCED")
                        {
                            dailySchedule.TimeSlots = DistributeBalanced(slotsForToday, campaign.DailyPasses);
                        }
                        else if (campaign.DistributionMode == "AUDIENCE")
                        {
                            dailySchedule.TimeSlots = DistributeByAudience(slotsForToday, campaign.DailyPasses);
                        }
                        else
                        {
                            dailySchedule.TimeSlots = DistributeBalanced(slotsForToday, campaign.DailyPasses);
                        }
                    }
                    else
                    {
                        // Senza distribuzione:  prendi i primi N slot disponibili
                        dailySchedule.TimeSlots = slotsForToday
                            .Take(Math.Min(campaign.DailyPasses, slotsForToday.Count))
                            .Select(s => s.SlotTime)
                            .ToList();
                    }

                    result.Add(dailySchedule);
                    // Salva gli orari usati oggi per il confronto domani
                    previousDaySlots = new HashSet<string>(dailySchedule.TimeSlots);
                    totalDays++;
                }

                int totalPasses = result.Sum(d => d.TimeSlots.Count);
                Console.WriteLine($"[ScheduleEngine] ✅ Generati {totalDays} giorni di schedulazione");
                Console.WriteLine($"[ScheduleEngine] ✅ Totale passaggi: {totalPasses}");
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleEngine] ❌ ERRORE generazione automatica: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Genera schedulazione manuale
        /// </summary>
        public List<DailySchedule> GenerateManual(DbcManager.Campaign campaign, List<string> selectedSlots)
        {
            var result = new List<DailySchedule>();

            try
            {
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
                Console.WriteLine($"[ScheduleEngine] Generazione MANUALE campagna: {campaign.CampaignName}");
                Console.WriteLine($"[ScheduleEngine] Punti orari selezionati: {selectedSlots.Count}");

                int totalDays = 0;
                for (DateTime date = campaign.StartDate.Date; date <= campaign.EndDate.Date; date = date.AddDays(1))
                {
                    if (!IsDayEnabled(campaign, date.DayOfWeek))
                        continue;

                    var dailySchedule = new DailySchedule
                    {
                        Date = date,
                        TimeSlots = new List<string>(selectedSlots),
                        IsConfirmed = false,
                        IsModified = false
                    };

                    result.Add(dailySchedule);
                    totalDays++;
                }

                int totalPasses = result.Sum(d => d.TimeSlots.Count);
                Console.WriteLine($"[ScheduleEngine] ✅ Generati {totalDays} giorni di schedulazione manuale");
                Console.WriteLine($"[ScheduleEngine] ✅ Totale passaggi: {totalPasses}");
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleEngine] ❌ ERRORE generazione manuale:  {ex.Message}");
            }

            return result;
        }

        // ═══════════════════════════════════════════════════════════
        // ALGORITMI DISTRIBUZIONE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// ✅ MIGLIORATO: Distribuzione bilanciata con algoritmo più preciso
        /// </summary>
        private List<string> DistributeBalanced(List<DbcManager.TimeSlot> slots, int passes)
        {
            var result = new List<string>();

            if (slots.Count == 0 || passes == 0)
                return result;

            // Limita passaggi al numero di slot disponibili
            int effectivePasses = Math.Min(passes, slots.Count);

            if (effectivePasses >= slots.Count)
            {
                // Se richiesti tanti passaggi quanti sono gli slot, usa tutti
                result = slots.Select(s => s.SlotTime).ToList();
            }
            else
            {
                // ✅ ALGORITMO BUCKET-CENTER: seleziona il centro di ciascun bucket equidistante
                // Divide i slot in effectivePasses bucket di dimensione uguale e prende il centro di ognuno.
                // step >= 1 garantisce bucket distinti → nessuna collisione, nessun raggruppamento.
                double step = (double)slots.Count / effectivePasses;

                for (int i = 0; i < effectivePasses; i++)
                {
                    int index = (int)(step * i + step / 2.0);
                    index = Math.Min(index, slots.Count - 1);
                    result.Add(slots[index].SlotTime);
                }

                // Ordina per orario crescente
                result = result.OrderBy(t => TimeSpan.Parse(t)).ToList();
            }

            Console.WriteLine($"[ScheduleEngine] Distribuzione BILANCIATA: {result.Count} passaggi su {slots.Count} slot disponibili");
            return result;
        }

        /// <summary>
        /// Distribuzione basata su audience:  priorità alle fasce con maggiore audience
        /// </summary>
        private List<string> DistributeByAudience(List<DbcManager.TimeSlot> slots, int passes)
        {
            var result = new List<string>();

            if (slots.Count == 0 || passes == 0)
                return result;

            // Ordina per priorità (1 = alta audience, 3 = bassa audience)
            var sortedSlots = slots.OrderBy(s => s.Priority).ThenBy(s => s.SlotTime).ToList();

            // Limita passaggi al numero di slot disponibili
            int effectivePasses = Math.Min(passes, sortedSlots.Count);

            // Prendi i primi N slot con maggiore audience
            for (int i = 0; i < effectivePasses; i++)
            {
                result.Add(sortedSlots[i].SlotTime);
            }

            // Riordina per orario crescente
            result = result.OrderBy(t => TimeSpan.Parse(t)).ToList();

            int priority1 = result.Count(t => sortedSlots.First(s => s.SlotTime == t).Priority == 1);
            int priority2 = result.Count(t => sortedSlots.First(s => s.SlotTime == t).Priority == 2);
            int priority3 = result.Count(t => sortedSlots.First(s => s.SlotTime == t).Priority == 3);

            Console.WriteLine($"[ScheduleEngine] Distribuzione AUDIENCE: {result.Count} passaggi (P1:{priority1}, P2:{priority2}, P3:{priority3})");
            return result;
        }

        // ═══════════════════════════════════════════════════════════
        // VALIDAZIONE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Valida schedulazione e rileva conflitti
        /// </summary>
        public List<ConflictWarning> Validate(int stationID, DateTime date)
        {
            var warnings = new List<ConflictWarning>();

            try
            {
                var schedules = LoadSchedule(stationID, date);
                var conflictRules = DbcManager.Load<DbcManager.ConflictRule>("ADV_ConflictRules.dbc")
                    .Where(r => r.StationID == stationID && r.IsActive)
                    .ToList();

                var campaigns = DbcManager.Load<DbcManager.Campaign>("ADV_Campaigns.dbc");
                var timeSlots = DbcManager.Load<DbcManager.TimeSlot>("ADV_TimeSlots.dbc")
                    .Where(t => t.StationID == stationID)
                    .ToList();

                // Raggruppa per slot
                var slotGroups = schedules.Where(s => s.FileType == "SPOT").GroupBy(s => s.SlotTime);

                foreach (var slotGroup in slotGroups)
                {
                    var spotsInSlot = slotGroup.ToList();
                    var timeSlot = timeSlots.FirstOrDefault(t => t.SlotTime == slotGroup.Key);

                    if (timeSlot == null)
                        continue;

                    // ✅ Verifica durata massima
                    int totalDuration = spotsInSlot.Sum(s => s.Duration);
                    if (totalDuration > timeSlot.MaxDuration)
                    {
                        warnings.Add(new ConflictWarning
                        {
                            Type = "DURATION_EXCEEDED",
                            Message = $"Durata eccessiva nello slot {slotGroup.Key}: {totalDuration}s > {timeSlot.MaxDuration}s",
                            Severity = "ERROR",
                            SlotTime = slotGroup.Key
                        });
                    }

                    // ✅ Verifica stessa categoria nello stesso slot
                    var categories = new Dictionary<int, int>();
                    foreach (var schedule in spotsInSlot)
                    {
                        var campaign = campaigns.FirstOrDefault(c => c.ID == schedule.CampaignID);
                        if (campaign != null)
                        {
                            if (!categories.ContainsKey(campaign.CategoryID))
                                categories[campaign.CategoryID] = 0;
                            categories[campaign.CategoryID]++;
                        }
                    }

                    foreach (var cat in categories.Where(c => c.Value > 1))
                    {
                        warnings.Add(new ConflictWarning
                        {
                            Type = "CATEGORY_DUPLICATE",
                            Message = $"Più spot ({cat.Value}) della stessa categoria nello slot {slotGroup.Key}",
                            Severity = "WARNING",
                            SlotTime = slotGroup.Key
                        });
                    }

                    // ✅ Verifica regole conflitto tra categorie
                    foreach (var rule in conflictRules)
                    {
                        var cat1Spots = spotsInSlot.Where(s =>
                        {
                            var c = campaigns.FirstOrDefault(camp => camp.ID == s.CampaignID);
                            return c?.CategoryID == rule.Category1ID;
                        }).ToList();

                        var cat2Spots = spotsInSlot.Where(s =>
                        {
                            var c = campaigns.FirstOrDefault(camp => camp.ID == s.CampaignID);
                            return c?.CategoryID == rule.Category2ID;
                        }).ToList();

                        if (cat1Spots.Any() && cat2Spots.Any())
                        {
                            warnings.Add(new ConflictWarning
                            {
                                Type = "CATEGORY_CONFLICT",
                                Message = $"Conflitto categorie nello slot {slotGroup.Key} (separazione min:  {rule.MinimumMinutesGap}min)",
                                Severity = "WARNING",
                                SlotTime = slotGroup.Key
                            });
                        }
                    }
                }

                Console.WriteLine($"[ScheduleEngine] Validazione {date: dd/MM/yyyy}:  {warnings.Count} avvisi rilevati");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleEngine] Errore validazione: {ex.Message}");
            }

            return warnings;
        }

        // ═══════════════════════════════════════════════════════════
        // CRUD SCHEDULAZIONI
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Salva schedulazione con rotazione multi-spot
        /// </summary>
        public bool SaveSchedule(int stationID, List<DailySchedule> dailySchedules, DbcManager.Campaign campaign, DbcManager.Spot spot, List<DbcManager.TimeSlot> timeSlots)
        {
            try
            {
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
                Console.WriteLine($"[ScheduleEngine] Salvataggio schedulazione campagna: {campaign.CampaignName}");

                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");
                int addedEntries = 0;

                foreach (var daily in dailySchedules)
                {
                    foreach (var slotTime in daily.TimeSlots)
                    {
                        var timeSlot = timeSlots.FirstOrDefault(ts => ts.SlotTime == slotTime);
                        if (timeSlot == null)
                            continue;

                        int sequenceOrder = 1;

                        // OPENING
                        if (!string.IsNullOrEmpty(timeSlot.OpeningFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = slotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "OPENING",
                                FilePath = timeSlot.OpeningFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = 0,
                                Duration = 5,
                                IsManual = daily.IsModified
                            });
                            addedEntries++;
                        }

                        // SPOT
                        allSchedules.Add(new DbcManager.Schedule
                        {
                            StationID = stationID,
                            ScheduleDate = daily.Date,
                            SlotTime = slotTime,
                            SequenceOrder = sequenceOrder++,
                            FileType = "SPOT",
                            FilePath = spot.FilePath,
                            ClientID = campaign.ClientID,
                            SpotID = spot.ID,
                            CampaignID = campaign.ID,
                            Duration = spot.Duration,
                            IsManual = daily.IsModified
                        });
                        addedEntries++;

                        // INFRASPOT
                        if (!string.IsNullOrEmpty(timeSlot.InfraSpotFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = slotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "INFRASPOT",
                                FilePath = timeSlot.InfraSpotFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = 0,
                                Duration = 3,
                                IsManual = daily.IsModified
                            });
                            addedEntries++;
                        }

                        // CLOSING
                        if (!string.IsNullOrEmpty(timeSlot.ClosingFile))
                        {
                            allSchedules.Add(new DbcManager.Schedule
                            {
                                StationID = stationID,
                                ScheduleDate = daily.Date,
                                SlotTime = slotTime,
                                SequenceOrder = sequenceOrder++,
                                FileType = "CLOSING",
                                FilePath = timeSlot.ClosingFile,
                                ClientID = 0,
                                SpotID = 0,
                                CampaignID = 0,
                                Duration = 5,
                                IsManual = daily.IsModified
                            });
                            addedEntries++;
                        }
                    }
                }

                bool success = DbcManager.Save("ADV_Schedule.dbc", allSchedules);
                Console.WriteLine($"[ScheduleEngine] ✅ Salvate {addedEntries} entry nel palinsesto");
                Console.WriteLine($"[ScheduleEngine] ✅ Risultato: {success}");
                Console.WriteLine($"[ScheduleEngine] ═══════════════════════════════════════");
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleEngine] ❌ ERRORE salvataggio schedulazione: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Carica schedulazione per una data specifica
        /// </summary>
        public List<DbcManager.Schedule> LoadSchedule(int stationID, DateTime date)
        {
            var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");
            return allSchedules
                .Where(s => s.StationID == stationID && s.ScheduleDate.Date == date.Date)
                .OrderBy(s => s.SlotTime)
                .ThenBy(s => s.SequenceOrder)
                .ToList();
        }

        /// <summary>
        /// Rimuove schedulazioni in un intervallo di date per una campagna
        /// </summary>
        public bool RemoveScheduleRange(int campaignID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var allSchedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");

                var toKeep = allSchedules.Where(s =>
                    s.CampaignID != campaignID ||
                    s.ScheduleDate.Date < fromDate.Date ||
                    s.ScheduleDate.Date > toDate.Date
                ).ToList();

                int removed = allSchedules.Count - toKeep.Count;
                Console.WriteLine($"[ScheduleEngine] Rimosse {removed} schedulazioni per campagna {campaignID} (dal {fromDate: dd/MM/yyyy} al {toDate:dd/MM/yyyy})");

                return DbcManager.Save("ADV_Schedule.dbc", toKeep);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScheduleEngine] Errore rimozione schedulazioni: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Conta passaggi schedulati in un periodo
        /// </summary>
        public int CountPasses(int campaignID, DateTime fromDate, DateTime toDate)
        {
            var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");
            return schedules.Count(s =>
                s.CampaignID == campaignID &&
                s.FileType == "SPOT" &&
                s.ScheduleDate.Date >= fromDate.Date &&
                s.ScheduleDate.Date <= toDate.Date
            );
        }

        /// <summary>
        /// Ottiene giorni con schedulazioni per una campagna
        /// </summary>
        public List<DateTime> GetScheduledDays(int campaignID)
        {
            var schedules = DbcManager.Load<DbcManager.Schedule>("ADV_Schedule.dbc");

            return schedules
                .Where(s => s.CampaignID == campaignID && s.FileType == "SPOT")
                .Select(s => s.ScheduleDate.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        // ═══════════════════════════════════════════════════════════
        // HELPER METHODS
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Verifica se un giorno della settimana è abilitato nella campagna
        /// </summary>
        private bool IsDayEnabled(DbcManager.Campaign campaign, DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => campaign.Monday,
                DayOfWeek.Tuesday => campaign.Tuesday,
                DayOfWeek.Wednesday => campaign.Wednesday,
                DayOfWeek.Thursday => campaign.Thursday,
                DayOfWeek.Friday => campaign.Friday,
                DayOfWeek.Saturday => campaign.Saturday,
                DayOfWeek.Sunday => campaign.Sunday,
                _ => false
            };
        }

        // ═══════════════════════════════════════════════════════════
        // MODELS INTERNI
        // ═══════════════════════════════════════════════════════════

        public class DailySchedule
        {
            public DateTime Date { get; set; }
            public List<string> TimeSlots { get; set; } = new List<string>();
            public bool IsConfirmed { get; set; }
            public bool IsModified { get; set; }
        }

        public class ConflictWarning
        {
            public string Type { get; set; }
            public string Message { get; set; }
            public string Severity { get; set; } // "ERROR", "WARNING", "INFO"
            public string SlotTime { get; set; }
        }
    }
}