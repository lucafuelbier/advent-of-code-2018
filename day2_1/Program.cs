using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day2_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var multiples =
                File.ReadLines("Input.txt")
                    .Select(CountMultiple)
                    .Aggregate(AddMultiples);

            var solution = multiples.dc * multiples.tc;

            Console.WriteLine(solution);

        }

        static (int dc, int tc) CountMultiple(string boxId)
        {
            // Could be shortened by using Linq #groupBy and #toDictionary

            var occurrences = new Dictionary<char, int>();

            foreach (char c in boxId)
            {
                if (occurrences.TryGetValue(c, out var occ))
                {
                    occurrences[c] = occ + 1;
                }
                else
                {
                    occurrences[c] = 1;
                }
            }

            int dc = occurrences.ContainsValue(2) ? 1 : 0;
            int tc = occurrences.ContainsValue(3) ? 1 : 0;

            return (dc, tc);
        }

        static (int dc, int tc) AddMultiples((int dc, int tc) first, (int dc, int tc) second) {
            return (first.dc + second.dc, first.tc + second.tc);
        }
    }
}
