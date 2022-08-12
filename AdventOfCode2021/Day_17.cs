using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_17
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_17_Test.txt");
            var challengeInput = Helpers.GetInput("Day_17.txt");
            var input = testInput;

            var targetRanges = Regex
                .Split(input.First(), @"\W\s")
                .Select(e => new Regex(@"-?\d+").Matches(e)
                    .Select(m => int.Parse(m.Value)))
                .Skip(1);

            var targetArea = new TargetArea(targetRanges);

            // Part 1
            var part1Solution = TriangularNumber(Math.Abs(targetArea.Bottom) - 1);
            Console.WriteLine($"Part 1 solution: {part1Solution}");

            // Part 2
            var minXVelocity = Math.Ceiling(TriangularRoot(targetArea.LeftLimit));
            var maxXVelocity = targetArea.RightLimit;
            var minYVelocity = targetArea.Bottom;
            var maxYVelocity = Math.Abs(targetArea.Bottom) - 1;

            var motionCalculator = new MotionCalculator();
            var validTrajectories = new List<Trajectory>();

            Func<MotionStep, bool> trajectoryContinuationFunc = step => 
                step.ByDimension("x").Position <= targetArea.RightLimit
                && step.ByDimension("y").Position >= targetArea.Bottom;

            for (var xVelocity = minXVelocity; xVelocity <= maxXVelocity; xVelocity++)
            {
                for (var yVelocity = minYVelocity; yVelocity <= maxYVelocity; yVelocity++)
                {
                    var motionDimensions = new MotionDimension[] 
                    {
                        new MotionDimension("x", 0, xVelocity, v => v > 0 ? v - 1 : 0),
                        new MotionDimension("y", 0, yVelocity, v => v - 1)
                    };

                    var initialStep = new MotionStep(motionDimensions);
                    var trajectory = motionCalculator.GetTrajectory(initialStep, trajectoryContinuationFunc);
                       
                    if (trajectory.Steps.Any(step => targetArea.IsWithinTargetArea(step)))
                    {
                        validTrajectories.Add(trajectory);
                    }
                }
            }

            var part2Solution = validTrajectories.Count();
            Console.WriteLine($"Part 2 solution: {part2Solution}");
        }

        private static int TriangularNumber(int root)
        {
            return root * (root + 1) / 2;
        }

        private static double TriangularRoot(int total)
        {
            return (Math.Pow(8 * total + 1, 0.5) - 1) / 2;
        }
    }

    public class TargetArea
    {
        public TargetArea(IEnumerable<IEnumerable<int>> targetRanges)
        {
            LeftLimit = targetRanges.First().First();
            RightLimit = targetRanges.First().Last();
            Bottom = targetRanges.Last().First();
            Top = targetRanges.Last().Last();
        }

        public int LeftLimit { get; set; }
        public int RightLimit { get; set; }
        public int Bottom { get; set; }
        public int Top { get; set; }

        public bool IsWithinTargetArea(MotionStep step)
        {
            return step.ByDimension("x").Position >= LeftLimit
                && step.ByDimension("x").Position <= RightLimit
                && step.ByDimension("y").Position <= Top
                && step.ByDimension("y").Position >= Bottom;
        }
    }
}
