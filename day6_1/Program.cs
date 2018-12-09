using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day6_1
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
            
            Console.WriteLine($"Coordinates: {minX} {maxX} {minY} {maxY}");

            var areas = coordinates.ToDictionary(c => c, c => 0);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Coordinate closestCoordinate = ComputeClosestCoordinate(coordinates, x, y);

                    if (closestCoordinate == null)
                    {
                        continue;
                    }

                    if (x == minX || x == maxX || y == minY || y == maxY)
                    {
                        areas.Remove(closestCoordinate);
                    }
                    else if(areas.ContainsKey(closestCoordinate))
                    {
                        areas[closestCoordinate] += 1;
                    }
                }
            }

            var solution = areas.Values.Max();

            Console.WriteLine("Areas:");
            areas.Values.ToList().ForEach(Console.WriteLine);

            Console.WriteLine("Solution:");
            Console.WriteLine(solution);
        }

        static (int x, int y) ParseCoordinate(string input)
        {
            Regex parsingRegex = new Regex(@"^(?<x>\d+),\s(?<y>\d+)$");

            Match match = parsingRegex.Match(input);

            int x = Int32.Parse(match.Groups["x"].Value);
            int y = Int32.Parse(match.Groups["y"].Value);

            return (x, y);
        }

        static Coordinate ComputeClosestCoordinate(IEnumerable<Coordinate> coordinates, int x, int y)
        {
            Coordinate closestCoordinate = null;
            int closestManhattanDistance = -1;
            bool multipleClosest = false;

            foreach (Coordinate coordinate in coordinates)
            {
                int manhattanDistance = Math.Abs(x - coordinate.X) + Math.Abs(y - coordinate.Y);

                if (closestCoordinate == null)
                {
                    closestCoordinate = coordinate;
                    closestManhattanDistance = manhattanDistance;
                }
                else if (manhattanDistance < closestManhattanDistance)
                {
                    closestCoordinate = coordinate;
                    closestManhattanDistance = manhattanDistance;
                    multipleClosest = false;
                }
                else if (manhattanDistance == closestManhattanDistance)
                {
                    multipleClosest = true;
                }
            }

            return multipleClosest ? null : closestCoordinate;
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
