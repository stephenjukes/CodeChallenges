using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static NbsCodeChallenges.AdventOfCode2021.Day_12;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_12
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_12_Test1.txt");
            var challengeInput = Helpers.GetInput("Day_12.txt");
            var input = testInput;

            var nodes = input
                .SelectMany(line => line.Split("-"))
                .Distinct()
                .Select(value => new Node(value, new Regex(@"[a-z]").IsMatch(value) ? 1 : 999))
                .ToArray()
                .PopulateNeighbours(input);

            var allPaths = new List<NodePath>();
            
            foreach(var smallNode in nodes.Where(n => n.IsSmall && !n.IsStart && !n.IsEnd))
            {
                var nodeSet = nodes
                    .Select(n => n.Value == smallNode.Value? new Node(n.Value, 2) : n)
                    .ToArray()
                    .PopulateNeighbours(input);

                var start = nodeSet.First(n => n.IsStart);
                var paths = TraverseFrom(new NodePath(start));

                allPaths.AddRange(paths);
            };

            var distinctPaths = allPaths
                .Select(p => string.Join("", p.Nodes))
                .Distinct();

            Console.WriteLine(distinctPaths.Count());
        }

        private static IEnumerable<NodePath> TraverseFrom(NodePath path)
        {
            var paths = new List<NodePath>();
            var origin = path.Nodes.Last();

            foreach (var neighbour in origin.Neighbours)
            {
                if (path.Nodes.Where(n => n.Value == neighbour.Value).Count() >= neighbour.MaximumVisits)
                {
                    continue;
                }

                var newPath = new NodePath(path.Nodes.Concat(neighbour));

                if (neighbour.IsEnd)
                {
                    paths.Add(newPath);
                    continue;
                }

                paths.AddRange(TraverseFrom(newPath));
            }

            return paths;
        }
    }

    public class Node
    {
        public Node(string value, int maximumVisits)
        {
            Value = value;
            MaximumVisits = maximumVisits;
            Neighbours = new List<Node>();
        }

        public string Value { get; set; }
        public List<Node> Neighbours { get; set; }
        public bool IsSmall => new Regex(@"[a-z]").IsMatch(Value);
        public bool IsStart => Value == NodeType.Start;
        public bool IsEnd => Value == NodeType.End;
        public int MaximumVisits { get; set; }
        public override string ToString()
        {
            return Value;
        }
    }

    public class NodePath
    {
        public NodePath(params Node[] nodes)
        {
            Nodes = nodes.ToList();
        }

        public NodePath(IEnumerable<Node> nodes)
        {
            Nodes = nodes.ToList();
        }

        public List<Node> Nodes { get; set; }

        public override string ToString()
        {
            return string.Join(", ", Nodes);
        }
    }

    public static class NodeType
    {
        public static string Start = "start";
        public static string End = "end";
    }

    public static class Day12Extensions
    {
        public static IEnumerable<Node> PopulateNeighbours(this Node[] nodes, string[] input)
        {
            foreach (var node in nodes)
            {
                node.Neighbours.Clear();
            }

            foreach (var line in input)
            {
                var nodePair = line.Split("-");

                var node1Index = Array.FindIndex(nodes, n => n.Value == nodePair[0]);
                var node2Index = Array.FindIndex(nodes, n => n.Value == nodePair[1]);

                nodes[node1Index].Neighbours.Add(nodes[node2Index]);
                nodes[node2Index].Neighbours.Add(nodes[node1Index]);
            }

            return nodes;
        }
    }
}


//public static class Day_12
//{
//    public static void Run()
//    {
//        var testInput = Helpers.GetInput("Day_12_Test3.txt");
//        var challengeInput = Helpers.GetInput("Day_12.txt");
//        var input = challengeInput;

//        var nodes = input
//            .SelectMany(line => line.Split("-"))
//            .Distinct()
//            .Select(value => new Node(value))
//            .ToArray();

//        foreach (var line in input)
//        {
//            var nodePair = line.Split("-");

//            var node1Index = Array.FindIndex(nodes, n => n.Value == nodePair[0]);
//            var node2Index = Array.FindIndex(nodes, n => n.Value == nodePair[1]);

//            nodes[node1Index].Neighbours.Add(nodes[node2Index]);
//            nodes[node2Index].Neighbours.Add(nodes[node1Index]);
//        }

//        var start = nodes.First(n => n.Value == NodeType.Start);

//        var paths = TraverseFrom(new Path(start));

//        Console.WriteLine(paths.Count());
//    }

//    private static IEnumerable<Path> TraverseFrom(Path path)
//    {
//        var paths = new List<Path>();
//        var origin = path.Nodes.Last();

//        foreach (var neighbour in origin.Neighbours)
//        {
//            if (neighbour.NodeSize == NodeType.Small && path.Nodes.Any(n => n.Value == neighbour.Value))
//            {
//                continue;
//            }

//            var newPath = new Path(path.Nodes.Concat(neighbour));

//            if (neighbour.Value == NodeType.End)
//            {
//                paths.Add(newPath);
//                continue;
//            }

//            paths.AddRange(TraverseFrom(newPath));
//        }

//        return paths;
//    }



//    public class Node
//    {
//        public Node(string value, IEnumerable<Node> neighbours)
//        {
//            Value = value;
//            Neighbours = neighbours.ToList();
//        }

//        public Node(string value)
//        {
//            Value = value;
//            Neighbours = new List<Node>();
//        }

//        public string Value { get; set; }
//        public List<Node> Neighbours { get; set; }
//        public string NodeSize => new Regex(@"[A-Z]").IsMatch(Value)
//            ? NodeType.Large
//            : NodeType.Small;


//        public override string ToString()
//        {
//            return Value;
//        }
//    }

//    public class Path
//    {
//        public Path(params Node[] nodes)
//        {
//            Nodes = nodes.ToList();
//        }

//        public Path(IEnumerable<Node> nodes)
//        {
//            Nodes = nodes.ToList();
//        }

//        public List<Node> Nodes { get; set; }

//        public override string ToString()
//        {
//            return string.Join(", ", Nodes);
//        }
//    }

//    public static class NodeType
//    {
//        public static string Start = "start";
//        public static string End = "end";
//        public static string Large = "Large";
//        public static string Small = "Small";
//    }
//}