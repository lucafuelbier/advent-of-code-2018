using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace day8_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllText("Input.txt")
                              .Split(" ")
                              .Select(Int32.Parse);

            var root = BuildNodes(numbers.GetEnumerator());

            var solution = root.CalculateValue();

            Console.WriteLine(solution);
        }

        static Node BuildNodes(IEnumerator<int> numbers)
        {
            numbers.MoveNext();
            int childCount = numbers.Current;

            numbers.MoveNext();
            int metadataCount = numbers.Current;

            Node node = new Node();

            for (int i = 0; i < childCount; i++)
            {
                Node child = BuildNodes(numbers);
                node.AddChild(child);
            }

            for (int i = 0; i < metadataCount; i++)
            {
                numbers.MoveNext();
                int metadata = numbers.Current;
                node.AddMetadata(metadata);
            }

            return node;
        }
    }

    class Node
    {
        public int ChildCount { get; set; }
        public int MetadataCount { get; set; }
        public List<Node> Children { get; set; }
        public List<int> Metadata { get; set; }

        public Node()
        {
            Children = new List<Node>();
            Metadata = new List<int>();
        }

        public void AddChild(Node child)
        {
            ChildCount += 1;
            Children.Add(child);
        }

        public void AddMetadata(int metadata)
        {
            MetadataCount += 1;
            Metadata.Add(metadata);
        }

        public int CalculateValue()
        {
            if (ChildCount == 0)
            {
                return Metadata.Sum();
            }

            int acc = 0;

            foreach (int reference in Metadata)
            {
                if (reference > ChildCount)
                {
                    continue;
                }

                acc += Children[reference - 1].CalculateValue();
            }

            return acc;
        }
    }
}
