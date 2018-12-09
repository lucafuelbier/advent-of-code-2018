using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day6_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");

            var coordinates = input.Select(ParseCoordinate)
                                   .Zip(
                                       Enumerable.Range(1, input.Length),
                                       ((int x, int y) pos, int id) => new Coordinate(id, pos.x, pos.y)
                                    )
                                    .ToList();

            var minX = coordinates.OrderBy(c => c.X)
                                  .First()
                                  .X;

            var maxX = coordinates.OrderByDescending(c => c.X)
                                  .First()
                                  .X;

            var minY = coordinates.OrderBy(c => c.Y)
                                  .First()
                                  .Y;

            var maxY = coordinates.OrderByDescending(c => c.Y)
                                  .First()
                                  .Y;

            int areaSize = 0;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (TotalManhattanDistanceLessThan10000(coordinates, x, y))
                    {
                        areaSize += 1;
                    }
                }
            }

            Console.WriteLine(areaSize);
        }

        static (int x, int y) ParseCoordinate(string input)
        {
            Regex parsingRegex = new Regex(@"^(?<x>\d+),\s(?<y>\d+)$");

            Match match = parsingRegex.Match(input);

            int x = Int32.Parse(match.Groups["x"].Value);
            int y = Int32.Parse(match.Groups["y"].Value);

            return (x, y);
        }

        static bool TotalManhattanDistanceLessThan10000(IEnumerable<Coordinate> coordinates, int x, int y)
        {
            int totalManhattanDistance = 0;

            foreach (Coordinate coordinate in coordinates)
            {
                int manhattanDistance = Math.Abs(x - coordinate.X) + Math.Abs(y - coordinate.Y);

                totalManhattanDistance += manhattanDistance;
            }

            return totalManhattanDistance < 10000;
        }
    }

    class Coordinate
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int id, int x, int y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }
    }
}
