using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace day2_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var boxIds = File.ReadLines("Input.txt").ToList();

            var similarIds = GetSimilarIds(boxIds);

            var solution =
                similarIds.boxId1.Zip(similarIds.boxId2, (c1, c2) => (c1: c1, c2: c2))
                                 .Where(letters => letters.c1 == letters.c2)
                                 .Select(letters => letters.c1)
                                 .Aggregate(new StringBuilder(),
                                            (sb, c) => sb.Append(c),
                                            sb => sb.ToString());
            
            Console.WriteLine(solution);
        }

        static (string boxId1, string boxId2) GetSimilarIds(IEnumerable<string> boxIds)
        {
            foreach (string boxId1 in boxIds)
            {
                foreach (string boxId2 in boxIds)
                {
                    var difference = 
                        boxId1.Zip(boxId2, (c1, c2) => (c1: c1, c2: c2))
                              .Select(letters => letters.c1 == letters.c2 ? 0 : 1)
                              .Sum();
                    
                    if(difference == 1) {
                        return (boxId1, boxId2);
                    }
                }
            }

            throw new Exception("No similar IDs found!");
        }
    }
}
