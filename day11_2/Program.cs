using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day11_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int gridSerialNumber = 7347;

            int[,] grid = new int[300, 300];

            // Calculate powerlevels
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    int cellX = x + 1;
                    int cellY = y + 1;
                    int rackId = cellX + 10;
                    int powerlevel = rackId * cellY;
                    powerlevel = powerlevel + gridSerialNumber;
                    powerlevel = powerlevel * rackId;
                    powerlevel = (powerlevel / 100) % 10;
                    powerlevel = powerlevel - 5;

                    grid[x, y] = powerlevel;
                }
            }

            (int x, int y, int s, int p) bestSquare = (-1, -1, -1, Int32.MinValue);

            // Find best fuel square
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    int maxSize = Math.Min(300 - x, 300 - y);

                    for (int size = 1; size <= maxSize; size++)
                    {
                        int powerlevel = 0;

                        for (int xs = x; xs < x + size; xs++)
                        {
                            for (int ys = y; ys < y + size; ys++)
                            {
                                powerlevel += grid[xs, ys];
                            }
                        }

                        if (powerlevel > bestSquare.p)
                        {
                            bestSquare = (x + 1, y + 1, size, powerlevel);
                            Console.WriteLine($"Found new best: {bestSquare.x},{bestSquare.y},{bestSquare.s},{bestSquare.p}");
                        }
                    }
                }
            }

            Console.WriteLine($"{bestSquare.x},{bestSquare.y},{bestSquare.s}");
        }
    }
}
