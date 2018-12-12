using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// For finding the Message i used VSCodes Minimap

namespace day10_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = File.ReadLines("Input.txt")
                             .Select(ParsePoint)
                             .ToList();

            // Approximated by looking at the input
            foreach (Point point in points)
            {
                point.AdvanceFast(10000);
            }

            int counter = 0;

            // It doesn't take as long as predicted
            while (!CheckPointsClose(points))
            {
                foreach (Point point in points)
                {
                    point.Advance();
                }
                counter += 1;
            }

            // Adjusted the loop length by hand until i got the output i wanted
            using (var fs = new StreamWriter(path: "Output.txt", append: false))
            {
                for (int i = 0; i < 10; i++)
                {
                    string message = ToMessage(points);
                    fs.Write(message);

                    foreach (Point point in points)
                    {
                        point.Advance();
                    }
                }
            }

            Console.WriteLine($"Done. Check Output.txt! Took 10000 + {counter} seconds to get to first output.");

            // Solution is 10000 + 678 + 3 = 10681
            // This puzzle was the worst
        }

        static Point ParsePoint(string input)
        {
            Regex pointRegex = new Regex(@"^position=<\s*(?<posX>-?\d+),\s*(?<posY>-?\d+)> velocity=<\s*(?<velX>-?\d+),\s*(?<velY>-?\d+)>$");

            Match pointMatch = pointRegex.Match(input);

            int posX = Int32.Parse(pointMatch.Groups["posX"].Value);
            int posY = Int32.Parse(pointMatch.Groups["posY"].Value);
            int velX = Int32.Parse(pointMatch.Groups["velX"].Value);
            int velY = Int32.Parse(pointMatch.Groups["velY"].Value);

            return new Point(posX, posY, velX, velY);
        }

        static string ToMessage(List<Point> points)
        {
            var minX = points.Select(p => p.PositionX).Min();
            var minY = points.Select(p => p.PositionY).Min();
            var maxX = points.Select(p => p.PositionX).Max();
            var maxY = points.Select(p => p.PositionY).Max();

            var lights = new HashSet<(int x, int y)>();

            foreach (Point point in points)
            {
                lights.Add((point.PositionX, point.PositionY));
            }

            StringBuilder sb = new StringBuilder();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (lights.Contains((x, y)))
                    {
                        sb.Append("#");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }

                sb.Append("\r\n");
            }

            sb.Append("\r\n");

            return sb.ToString();
        }

        // I got lucky that 100 was enough
        static bool CheckPointsClose(List<Point> points)
        {
            foreach (Point point1 in points)
            {
                foreach (Point point2 in points)
                {
                    if (Math.Sqrt(Math.Pow(point1.PositionX - point2.PositionX, 2) + Math.Pow(point1.PositionY - point2.PositionY, 2)) > 100)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    class Point
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public int VelocityX { get; }
        public int VelocityY { get; }

        public Point(int posX, int posY, int velX, int velY)
        {
            PositionY = posY;
            PositionX = posX;
            VelocityX = velX;
            VelocityY = velY;
        }

        public void Advance()
        {
            PositionX += VelocityX;
            PositionY += VelocityY;
        }

        public void AdvanceFast(int times)
        {
            PositionX += VelocityX * times;
            PositionY += VelocityY * times;
        }
    }
}
