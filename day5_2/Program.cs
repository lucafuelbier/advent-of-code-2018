using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day5_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymer = File.ReadAllText("Input.txt");
            var shortest = polymer.Length;

            foreach (char c in "abcdefghijklmnopqrstuvwxyz")
            {
                var replacementPattern = $"[{c}{Char.ToUpper(c)}]";

                var collapsedPolymer = Regex.Replace(polymer, replacementPattern, "");

                var stack = new Stack<char>();

                foreach (char p in collapsedPolymer)
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(p);
                    }
                    else if (IsOpposite(stack.Peek(), p))
                    {
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(p);
                    }
                }

                if (stack.Count < shortest)
                {
                    shortest = stack.Count;
                }
            }

            Console.WriteLine(shortest);
        }

        static bool IsOpposite(char x, char y)
        {
            return Math.Abs(x.CompareTo(y)) == 32;
        }
    }
}
