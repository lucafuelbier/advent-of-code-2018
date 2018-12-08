using System;
using System.IO;
using System.Collections.Generic;

namespace day5_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymer = File.ReadAllText("Input.txt");

            var stack = new Stack<char>();

            foreach (char c in polymer)
            {
                if (stack.Count == 0)
                {
                    stack.Push(c);
                }
                else if (IsOpposite(stack.Peek(), c))
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(c);
                }
            }

            var solution = stack.Count;

            Console.WriteLine(solution);
        }

        static bool IsOpposite(char x, char y)
        {
            return Math.Abs(x.CompareTo(y)) == 32;
        }
    }
}
