using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static  class Day_13
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_13_Test.txt");
            var challengeInput = Helpers.GetInput("Day_13.txt");
            var input = testInput;

            var inputParts = string.Join("\n", input).Split("\n\n");

            var dots = inputParts[0].Split("\n")
                .Select(line => line.Split(","))
                .Select(line => new Dot(int.Parse(line[0]), int.Parse(line[1])));

            var foldInstructions = inputParts[1].Split("\n");

            var dotsAfterFirstFold = GetFoldSummary(dots, new string[] { foldInstructions.First() }, dots => dots.Count().ToString());
            Console.WriteLine($"Dots showing after the first fold: { dotsAfterFirstFold }");

            var finalDotArrangement = GetFoldSummary(dots, foldInstructions, dots => Display(dots));
            Console.WriteLine($"Final dot arrangement:\n{ finalDotArrangement }");
        }

        private static string GetFoldSummary(IEnumerable<Dot> dots, string[] foldInstructions, Func<IEnumerable<Dot>, string> summaryFunc)
        {
            foreach (var foldInstruction in foldInstructions)
            {
                var instruction = new Regex(@"\w=\d+").Match(foldInstruction).Value;
                var instructionParts = instruction.Split("=");
                var direction = instructionParts.First();
                var linePosition = int.Parse(instructionParts.Last());
         
                var paperHalf = (direction == "x"
                    ? dots.GroupBy(dot => dot.Column < linePosition)
                    : dots.GroupBy(dot => dot.Row < linePosition))
                    .ToDictionary(g => g.Key == true ? PaperHalf.BeforeFold : PaperHalf.AfterFold, g => g.ToList());

                foreach (var transferringDot in paperHalf[PaperHalf.AfterFold])
                {
                    var newDot = direction == "x"
                        ? new Dot(2 * linePosition - (transferringDot.Column), transferringDot.Row)
                        : new Dot(transferringDot.Column, 2 * linePosition - (transferringDot.Row));

                    paperHalf[PaperHalf.BeforeFold].Add(newDot);
                }

                dots = paperHalf[PaperHalf.BeforeFold].Distinct();
            }

            return summaryFunc(dots);
        }

        private static string Display(IEnumerable<Dot> dots)
        {
            var paper = new Dot[
                dots.Select(d => d.Row).Max() + 1,
                dots.Select(d => d.Column).Max() + 1];

            foreach (var dot in dots)
            {
                paper[dot.Row, dot.Column] = dot;
            }

            var rowDisplays = new List<string>();
            for (var row = 0; row < paper.GetLength(0); row++)
            {
                var rowDisplay = new StringBuilder();
                for (var column = 0; column < paper.GetLength(1); column++)
                {
                    rowDisplay.Append(paper[row, column] is Dot ? "#" : ".");
                }

                rowDisplays.Add(rowDisplay.ToString());
            }

            return string.Join("\n", rowDisplays);
        }

        public class Dot : IEquatable<Dot>
        {
            public Dot(int column, int row)
            {
                Column = column;
                Row = row;
            }

            public int Column { get; set; }
            public int Row { get; set; }
           
            public bool Equals([AllowNull] Dot other)
            {
                return Column == other.Column && Row == other.Row;
            }

            public override int GetHashCode()
            {
                return Column.GetHashCode() ^ Row.GetHashCode();
            }

            public override string ToString()
            {
                return $"{Column}, {Row}";
            }
        }

        public enum PaperHalf
        {
            BeforeFold,
            AfterFold
        }
    }
}
