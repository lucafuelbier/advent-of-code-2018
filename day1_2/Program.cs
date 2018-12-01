using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day1_2
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var frequencies = new HashSet<int>();
            var fchanges = File.ReadLines("Input.txt")
                               .Select(fc => Int32.Parse(fc))
                               .Cycle();

            int prev = 0;
            int? solution = null;

            foreach (var fc in fchanges)
            {
                int fq = prev += fc;

                if (frequencies.Contains(fq))
                {
                    solution = fq;
                    break;
                }

                frequencies.Add(fq);
                prev = fq;
            }

            Console.WriteLine(solution);
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> c)
        {
            while (true)
            {
                foreach (T v in c)
                {
                    yield return v;
                }
            }
        }
    }
}
