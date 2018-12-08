using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

// Not happy with this solution

namespace day4_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex recordRegex = new Regex(@"^\[(?<date>.{10})\s\d{2}:(?<minutes>\d{2})\]\s(?<type>Guard\s#(?<id>\d+)\sbegins\sshift|falls\sasleep|wakes\sup)$");

            var records = File.ReadLines("Input.txt")
                              .OrderBy(raw => raw)
                              .Select(raw => recordRegex.Match(raw))
                              .Select(mapRecord);

            var guardBehavior = CalculateGuardBehvior(records);

            var guard = guardBehavior.Select(gb => (gb.Key, gb.Value.Sum()))
                                     .OrderByDescending(g => g.Item2)
                                     .Select(g => g.Item1)
                                     .First();

            var minute = guardBehavior[guard].Zip(Enumerable.Range(0, 59), (m, p) => (m, p))
                                             .OrderByDescending(x => x.Item1)
                                             .Select(x => x.Item2)
                                             .First();
            
            var solution = guard * minute;

            Console.WriteLine(solution);
        }

        static Record mapRecord(Match match)
        {
            string date = match.Groups["date"].Value;
            int minutes = Int32.Parse(match.Groups["minutes"].Value);

            string typeRaw = match.Groups["type"].Value;
            string idRaw = match.Groups["id"].Value;

            int? id = null;

            if (!String.IsNullOrEmpty(idRaw))
            {
                id = Int32.Parse(idRaw);
            }

            RecordType type = RecordType.GuardId;

            if (typeRaw == "falls asleep")
            {
                type = RecordType.FallAsleep;
            }
            else if (typeRaw == "wakes up")
            {
                type = RecordType.WakeUp;
            }

            return new Record
            {
                Date = date,
                Minutes = minutes,
                Type = type,
                GuardId = id
            };
        }

        static Dictionary<int, int[]> CalculateGuardBehvior(IEnumerable<Record> records)
        {
            var queue = new Queue<Record>(records);
            var guardBehavior = new Dictionary<int, int[]>();

            var currentRecord = queue.Dequeue();
            var currentAsleep = false;
            var currentGuardId = currentRecord.GuardId.Value;
            var currentMinutes = currentRecord.Minutes;

            while (true)
            {
                if (currentRecord.Minutes == currentMinutes && (queue.Count > 0))
                {
                    switch (currentRecord.Type)
                    {
                        case RecordType.GuardId:
                            currentGuardId = currentRecord.GuardId.Value;
                            if (!guardBehavior.ContainsKey(currentGuardId))
                            {
                                guardBehavior.Add(currentGuardId, new int[60]);
                            }
                            currentAsleep = false;
                            currentRecord = queue.Dequeue();
                            break;
                        case RecordType.FallAsleep:
                            currentAsleep = true;
                            currentRecord = queue.Dequeue();
                            break;
                        case RecordType.WakeUp:
                            currentAsleep = false;
                            currentRecord = queue.Dequeue();
                            break;
                    }
                }

                if (currentAsleep)
                {
                    guardBehavior[currentGuardId][currentMinutes] += 1;
                }

                if (currentMinutes == 59 && queue.Count == 0)
                {
                    break;
                }

                currentMinutes = (currentMinutes + 1) % 60;
            }

            return guardBehavior;
        }
    }

    class Record
    {
        public string Date { get; set; }
        public int Minutes { get; set; }
        public RecordType Type { get; set; }
        public int? GuardId { get; set; }
    }

    enum RecordType
    {
        GuardId,
        WakeUp,
        FallAsleep
    }
}
