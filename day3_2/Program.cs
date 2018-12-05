using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day3_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputRegex = new Regex(@"^#(?<id>\d+) @ (?<xoff>\d+),(?<yoff>\d+): (?<xcut>\d+)x(?<ycut>\d+)$");

            var claims = File.ReadAllLines("Input.txt")
                             .Select(x => inputRegex.Match(x))
                             .Select(MapClaim);

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

            var solution = claims.First(c => DoesNotOverlap(c, fabric));
            
            Console.WriteLine(solution.id);
        }

        static (string id, int xoff, int yoff, int xcut, int ycut) MapClaim(Match match)
        {
            if (match.Success)
            {
                int xoff = Int32.Parse(match.Groups["xoff"].Value);
                int yoff = Int32.Parse(match.Groups["yoff"].Value);
                int xcut = Int32.Parse(match.Groups["xcut"].Value);
                int ycut = Int32.Parse(match.Groups["ycut"].Value);
                string id = match.Groups["id"].Value;

                return (id, xoff, yoff, xcut, ycut);
            }
            else
            {
                throw new Exception("Unsuccessful match.");
            }
        }

        static bool DoesNotOverlap((string id, int xoff, int yoff, int xcut, int ycut) claim, int[,] fabric)
        {
            for (int x = claim.xoff; x < claim.xoff + claim.xcut; x++)
            {
                for (int y = claim.yoff; y < claim.yoff + claim.ycut; y++)
                {
                    if (fabric[x, y] > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
