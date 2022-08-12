using NbsCodeChallenges.AdventOfCode2021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_25
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_25_Test.txt");
            //var challengeInput = Helpers.GetInput("Day_25.txt");
            var input = testInput;

            var seabedMap = input
                .Select((row, rowIndex) => row
                    .Select((value, valueIndex) => new BasePosition<Cucumber>
                    {
                        Value = value == '.' ? null : new Cucumber { Direction = value },
                        Row = rowIndex,
                        Column = valueIndex
                    }).ToArray())
                .ToArray();

            var cucumbers = seabedMap
                .SelectMany(p => p)
                .Where(p => CucumberAt(p) != null);

            var rows = seabedMap.Count();
            var columns = seabedMap.First().Count();

            var cucumberSets = new CucumberSet[]
            {
                new CucumberSet
                {
                    Direction = '>',
                    MoveFunc = (origin, positions) => positions[origin.Row][(origin.Column + 1) % columns]
                },

                new CucumberSet
                {
                    Direction = 'v',
                    MoveFunc = (origin, positions) => positions[(origin.Row + 1) % rows][origin.Column]
                }
            };

            var i = 0;
            do
            {
                var cucumberSet = cucumberSets[i % cucumberSets.Length];

                var movableCucumbers = cucumbers
                    .Select(p => new BasePosition<Cucumber> // attempt to clone
                    {
                        Value = CucumberAt(p),
                        Row = p.Row,
                        Column = p.Column
                    })
                    .Where(p => CucumberAt(p).Direction == cucumberSet.Direction)
                    .Select(p =>
                        {
                            CucumberAt(p).Destination = cucumberSet.MoveFunc(p, seabedMap);
                            return p;
                        })
                    .Where(p => CucumberAt(p).CanMove);


                foreach(var position in movableCucumbers)
                {
                    var cucumber = CucumberAt(position);

                    var newCoordinate = cucumber.Destination;

                    seabedMap[newCoordinate.Row][newCoordinate.Column].Value = cucumber;
                    seabedMap[position.Row][position.Column].Value = null;

                    Console.WriteLine($"{ position } to { newCoordinate }");
                    Display(seabedMap, i);
                }

                i++;
            }
            while (cucumbers.Any(p => CucumberAt(p).CanMove));
        }

        private static void Display(BasePosition<Cucumber>[][] seabedMap, int i)
        {
            var direction = i == 0 ? "Initial State" : i % 2 == 0 ? "South" : "East";
            Console.WriteLine($"Step {Math.Ceiling((double)i / 2)} - {direction}");

            var map = string.Join(
                "\n",
                seabedMap.Select(row => string.Join(
                    "",
                    row.Select(p => CucumberAt(p)?.Direction ?? '.'))));

            Console.WriteLine(map);
            Console.WriteLine();
        }

        private static Cucumber CucumberAt(BasePosition<Cucumber> position) => position.Value;

    }

    public class Cucumber
    {
        public char Direction { get; set; }
        public BasePosition<Cucumber> Destination { get; set; }
        public bool CanMove => Destination?.Value == null;

        public override string ToString()
        {
            return Direction.ToString() ?? ".";
        }
    }

    public class CucumberSet
    {
        public char Direction { get; set; }
        public Func<BasePosition<Cucumber>, BasePosition<Cucumber>[][], BasePosition<Cucumber>> MoveFunc { get; set; }
    }
}
