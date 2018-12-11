using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace day8_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllText("Input.txt")
                              .Split(" ")
                              .Select(Int32.Parse);

            var tree = BuildTree(numbers);

            var solution = tree.SelectMany(n => n.Metadata)
                               .Sum();

            Console.WriteLine(solution);
        }

        static Tree BuildTree(IEnumerable<int> numbers)
        {
            var enumerator = numbers.GetEnumerator();

            var root = BuildNodes(enumerator);

            return new Tree(root);
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
    }

    class Tree : IEnumerable<Node>
    {
        public Node Root { get; }

        public Tree(Node root)
        {
            this.Root = root;
        }

        public IEnumerator<Node> GetEnumerator()
        {
            var nodes = new Stack<Node>();
            nodes.Push(Root);

            while (nodes.Count > 0)
            {
                var node = nodes.Pop();

                foreach (var child in node.Children)
                {
                    nodes.Push(child);
                }

                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
