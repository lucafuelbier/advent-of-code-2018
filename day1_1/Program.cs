using System;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = File.ReadLines("Input.txt")
                               .Select(fc => Int32.Parse(fc))
                               .Aggregate((fc1, fc2) => fc1 + fc2);
            
            Console.WriteLine(solution);
        }
    }
}
