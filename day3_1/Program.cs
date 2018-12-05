using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day3_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputRegex = new Regex(@"^#\d+ @ (?<xoff>\d+),(?<yoff>\d+): (?<xcut>\d+)x(?<ycut>\d+)$");

            var claims = File.ReadLines("Input.txt")
                             .Select(x => inputRegex.Match(x))
                             .Select(mapClaim);

            int[,] fabric = new int[1000, 1000];

            foreach (var claim in claims)
            {
                for (int x = claim.xoff; x < claim.xoff + claim.xcut; x++)
                {
                    for (int y = claim.yoff; y < claim.yoff + claim.ycut; y++)
                    {
                        fabric[x, y] += 1;
                    }
                }
            }

            int overlap = 0;

            foreach (int usage in fabric)
            {
                if (usage > 1)
                {
                    overlap += 1;
                }
            }

            Console.WriteLine(overlap);
        }

        static (int xoff, int yoff, int xcut, int ycut) mapClaim(Match match)
        {
            if (match.Success)
            {
                int xoff = Int32.Parse(match.Groups["xoff"].Value);
                int yoff = Int32.Parse(match.Groups["yoff"].Value);
                int xcut = Int32.Parse(match.Groups["xcut"].Value);
                int ycut = Int32.Parse(match.Groups["ycut"].Value);

                return (xoff, yoff, xcut, ycut);
            }
            else
            {
                throw new Exception("Unsuccessful match.");
            }
        }
    }
}
