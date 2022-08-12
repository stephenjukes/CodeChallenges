using System;
using System.Collections.Generic;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_11
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_11_Test.txt");
            var challengeInput = Helpers.GetInput("Day_11.txt");
            var input = challengeInput;

            var part1Calculator = new OctopusCalculator(new Calculation
            {
                OctopusCollection = GetOctopiFromFileInput(input),
                ConditionFunc = calc => calc.Step < 100,
                SummaryFunc = calc => calc.OctopusCollection.Sum(o => o.Flashes)
            });

            var part2Calculator = new OctopusCalculator(new Calculation
            {
                OctopusCollection = GetOctopiFromFileInput(input),
                ConditionFunc = calc => calc.OctopusCollection.Any(o => o.IsCharging),
                SummaryFunc = calc => calc.Step
            });

            Console.WriteLine($"Part 1 Summary: { part1Calculator.GetSummary() }");
            Console.WriteLine($"Part 2 Summary: { part2Calculator.GetSummary() }");
        }

        private static IEnumerable<Octopus> GetOctopiFromFileInput(string[] input)
        {
            var octopi = new List<Octopus>();

            for(var row = 0; row < input.Length; row++)
            {
                for(var column = 0; column < input.First().Length; column++)
                {
                    octopi.Add(new Octopus
                    {
                        Energy = int.Parse(input[row][column].ToString()),
                        Row = row,
                        Column = column,
                        IsCharging = true
                    });
                }
            }

            foreach (var octopus in octopi)
            {
                octopus.Neighbours = GetNeighbours(octopus, octopi);
            }

            return octopi;
        }

        private static IEnumerable<Octopus> GetNeighbours(Octopus octopus, IEnumerable<Octopus> octopusCollection)
        {
            var neigbours = octopusCollection.Where(o => 
                o.Row >= octopus.Row - 1 &&
                o.Row <= octopus.Row + 1 &&
                o.Column >= octopus.Column - 1 &&
                o.Column <= octopus.Column + 1 &&
                !(o.Row == octopus.Row && o.Column == octopus.Column));

            return neigbours;
        }
    }

    public class OctopusCalculator
    {
        private Calculation _calc;
        public OctopusCalculator(Calculation calc)
        {
            _calc = calc;
        }

        public int GetSummary()
        {
            while (_calc.ConditionFunc(_calc))
            {
                foreach (var octopus in _calc.OctopusCollection)
                {
                    octopus.IsCharging = true;
                    octopus.Energy++;
                }

                foreach (var octopus in _calc.OctopusCollection.Where(o => o.Energy > 9))
                {
                    Flash(octopus);
                }

                _calc.Step++;
            }

            return _calc.SummaryFunc(_calc);
        }

        private void Flash(Octopus octopus)
        {
            octopus.Flashes++;
            octopus.Energy = 0;
            octopus.IsCharging = false;

            foreach (var neighbour in octopus.Neighbours.Where(o => o.IsCharging))
            {
                neighbour.Energy++;

                if (neighbour.Energy > 9)
                {
                    Flash(neighbour);
                }
            }
        }
    }

    public class Calculation
    {
        public int Step { get; set; }
        public IEnumerable<Octopus> OctopusCollection { get; set; }
        public Func<Calculation, bool> ConditionFunc { get; set; }
        public Func<Calculation, int> SummaryFunc { get; set; }
    }

    public class Octopus
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Energy { get; set; }
        public IEnumerable<Octopus> Neighbours { get; set; }
        public bool IsCharging { get; set; }
        public int Flashes { get; set; }

        public override string ToString()
        {
            return Energy.ToString();
        }
    }
}
