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
                    powerlevel = (powerlevel / 10) % 100;
                    powerlevel = powerlevel - 5;

                    grid[x, y] = powerlevel;
                }
            }

            (int x, int y, int p) bestSquare = (-1, -1, Int32.MinValue);

            // Find best fuel square
            for (int x = 0; x < 297; x++)
            {
                for (int y = 0; y < 297; y++)
                {
                    int powerlevel = 0;
                    powerlevel += grid[x, y];
                    powerlevel += grid[x + 1, y];
                    powerlevel += grid[x + 2, y];
                    powerlevel += grid[x, y + 1];
                    powerlevel += grid[x + 1, y + 1];
                    powerlevel += grid[x + 2, y + 1];
                    powerlevel += grid[x, y + 2];
                    powerlevel += grid[x + 1, y + 2];
                    powerlevel += grid[x + 2, y + 2];

                    if (powerlevel > bestSquare.p)
                    {
                        bestSquare = (x + 1, y + 1, powerlevel);
                    }
                }
            }

            Console.WriteLine($"{bestSquare.x},{bestSquare.y}");
        }
    }
}
