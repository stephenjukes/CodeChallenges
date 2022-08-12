using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_18
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_18_Test.txt");
            var challengeInput = Helpers.GetInput("Day_18.txt");
            var input = testInput;

            var snailFishNodes = input.Select(row =>
            {
                var deserialised = Helpers.Deserialize(row);
                return CreateSnailFishNodes(deserialised);
            });

            // Part 1
            var part1Solution = snailFishNodes
                .Aggregate(AddSnailFishNodes)
                .GetMagnitude();

            Console.WriteLine(part1Solution);

            // Part 2
            var part2Solution = snailFishNodes
                .SelectMany(nodeA => snailFishNodes
                    .Select(nodeB => AddSnailFishNodes(nodeA, nodeB).GetMagnitude()))
                .OrderByDescending(magnitude => magnitude)
                .First();

            Console.WriteLine(part2Solution);
        }

        private static SnailFishNode CreateSnailFishNodes(object obj)
        {
            // https://stackoverflow.com/questions/10745542/object-to-string-array
            var collection = obj as IEnumerable;
            if (collection != null)
            {
                var children = ((IEnumerable)obj).Cast<object>();

                return new SnailFishNode
                {
                    Left = CreateSnailFishNodes(children.First()),
                    Right = CreateSnailFishNodes(children.Last())
                };
            }

            return new SnailFishNode
            {
                Value = (int)(long)obj
            };
        }

        private static SnailFishNode AddSnailFishNodes(SnailFishNode left, SnailFishNode right)
        {
            var newNode = new SnailFishNode
            {
                Left = left.Clone(),
                Right = right.Clone()
            };

            SnailFishNode explosion;
            SnailFishNode toSplit = null;
            
            do
            {
                explosion = FindNodeToExplode(newNode);

                if (explosion != null)
                {
                    HandleExplosion(explosion, newNode);
                    continue;
                }

                toSplit = FindNodeToSplit(newNode);

                if (toSplit != null)
                {
                    HandleSplit(toSplit);
                    continue;
                }
            }
            while (explosion != null || toSplit != null);

            return newNode;
        }

        private static SnailFishNode FindNodeToExplode(SnailFishNode node, int depth = 0)
        {
            if (node == null)
            {
                return null;
            }

            if (depth == 4 && node.Value == null) // ie: has left and right values
            {
                return node;
            }

            return FindNodeToExplode(node.Left, depth + 1) ?? FindNodeToExplode(node.Right, depth + 1);
        }

        private static void HandleExplosion(SnailFishNode explosion, SnailFishNode node)
        {
            var nodeValues = node
                .ToCollectionOfNodeValues()
                .ToList();

            // handle left
            var explosionLeftNodeIndex = nodeValues.IndexOf(explosion.Left);
            var leftAdjacent = explosionLeftNodeIndex > 0 ? nodeValues[explosionLeftNodeIndex - 1] : null;
            if (leftAdjacent != null)
            {
                leftAdjacent.Value += explosion.Left.Value;
            }

            // handle right
            var explosionRightNodeIndex = nodeValues.IndexOf(explosion.Right);
            var rightAdjacent = explosionRightNodeIndex < nodeValues.Count - 1 ? nodeValues[explosionRightNodeIndex + 1] : null;
            if (rightAdjacent != null)
            {
                rightAdjacent.Value += explosion.Right.Value;
            }

            // handle exploded node
            explosion.Value = 0;
            explosion.Left = null;
            explosion.Right = null;
        }

        private static SnailFishNode FindNodeToSplit(SnailFishNode node)
        {
            return node
                .ToCollectionOfNodeValues()
                .FirstOrDefault(n => n.Value > 9);
        }

        private static void HandleSplit(SnailFishNode toSplit)
        {
            var toSplitValue = (double)toSplit.Value;

            toSplit.Value = null;
            toSplit.Left = new SnailFishNode { Value = (int)Math.Floor(toSplitValue / 2) };
            toSplit.Right = new SnailFishNode { Value = (int)Math.Ceiling(toSplitValue / 2) };
        }
    }

    public class SnailFishNode : IEquatable<SnailFishNode>
    {
        public SnailFishNode()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int? Value { get; set; }
        public SnailFishNode Left { get; set; }
        public SnailFishNode Right { get; set; }

        public override string ToString()
            => $"Value: {Value}";

        public bool Equals([AllowNull] SnailFishNode other)
            => this.Id == other.Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }

    public static class Day18Extensions
    {
        public static IEnumerable<SnailFishNode> ToCollectionOfNodeValues(this SnailFishNode node)
            => ToSnailFishCollection(node, node => node.Value != null);

        public static IEnumerable<SnailFishNode> ToSnailFishCollection(this SnailFishNode node, Func<SnailFishNode, bool> isRequired)
        {
            return node != null
                ? node
                    .Concat(node.Left.ToSnailFishCollection(isRequired))
                    .Concat(node.Right.ToSnailFishCollection(isRequired))
                    .Where(isRequired)
                : Enumerable.Empty<SnailFishNode>();
        }

        public static SnailFishNode Clone(this SnailFishNode node)
        {
            return new SnailFishNode
            {
                Id = node.Id,
                Value = node.Value,
                Left = node.Left?.Clone(),
                Right = node.Right?.Clone()
            };
        }

        public static int GetMagnitude(this SnailFishNode node)
        {
            return node.Value ?? 3 * GetMagnitude(node.Left) + 2 * GetMagnitude(node.Right);
        }
    }
}
