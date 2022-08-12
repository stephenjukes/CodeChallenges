using System;
using System.Collections.Generic;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_06
    {
        public static void Run()
        {
            var test = new List<int> { 3, 4, 3, 1, 2 };
            var challenge = new List<int> { 1, 1, 1, 1, 1, 5, 1, 1, 1, 5, 1, 1, 3, 1, 5, 1, 4, 1, 5, 1, 2, 5, 1, 1, 1, 1, 3, 1, 4, 5, 1, 1, 2, 1, 1, 1, 2, 4, 3, 2, 1, 1, 2, 1, 5, 4, 4, 1, 4, 1, 1, 1, 4, 1, 3, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 5, 4, 4, 2, 4, 5, 2, 1, 5, 3, 1, 3, 3, 1, 1, 5, 4, 1, 1, 3, 5, 1, 1, 1, 4, 4, 2, 4, 1, 1, 4, 1, 1, 2, 1, 1, 1, 2, 1, 5, 2, 5, 1, 1, 1, 4, 1, 2, 1, 1, 1, 2, 2, 1, 3, 1, 4, 4, 1, 1, 3, 1, 4, 1, 1, 1, 2, 5, 5, 1, 4, 1, 4, 4, 1, 4, 1, 2, 4, 1, 1, 4, 1, 3, 4, 4, 1, 1, 5, 3, 1, 1, 5, 1, 3, 4, 2, 1, 3, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 5, 1, 1, 1, 1, 3, 1, 1, 5, 1, 1, 4, 1, 1, 3, 1, 1, 5, 2, 1, 4, 4, 1, 4, 1, 2, 1, 1, 1, 1, 2, 1, 4, 1, 1, 2, 5, 1, 4, 4, 1, 1, 1, 4, 1, 1, 1, 5, 3, 1, 4, 1, 4, 1, 1, 3, 5, 3, 5, 5, 5, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 4, 2, 1, 1, 4, 5, 3, 1, 1, 5, 5, 1, 1, 2, 1, 4, 1, 3, 5, 1, 1, 1, 5, 2, 2, 1, 4, 2, 1, 1, 4, 1, 3, 1, 1, 1, 3, 1, 5, 1, 5, 1, 1, 4, 1, 2, 1 };
            var input = test;

            var days = 80;
            var initialTimer = 7;

            var inputGroups = input.GroupBy(n => n);

            var populationSummaries = new int[initialTimer + 2]
                .SelectMany((time, i) => new PopulationSummary[]
                    {
                        new PopulationSummary
                        {
                            Time = i,
                            IsAdult = true,
                            Count = inputGroups.FirstOrDefault(g => g.Key == i)?.Count() ?? 0
                        },

                        new PopulationSummary { Time = i, IsAdult = false}
                    })
                .ToDictionary(p => ToKey(p), p => p);

            for (var day = 0; day < days; day++)
            {
                var previousFishAtZero = populationSummaries
                    .Where(kvp => kvp.Value.Time == 0)
                    .Sum(kvp => kvp.Value.Count); // adults and children

                foreach (var summary in populationSummaries.Values)
                {
                    // decrement timers
                    if (summary.Time < initialTimer + 1)
                    {
                        var previousDaySummary = populationSummaries[ToKey(summary.IsAdult, summary.Time + 1)];
                        populationSummaries[ToKey(summary)].Count = previousDaySummary.Count;
                    }

                    // reset timers
                    if (summary.IsAdult && summary.Time == initialTimer - 1)
                    {
                        populationSummaries[ToKey(summary)].Count += previousFishAtZero;
                    }

                    // reproduce
                    if (!summary.IsAdult && summary.Time == initialTimer + 1)
                    {
                        var offspring = populationSummaries[ToKey(isAdult: true, initialTimer - 1)];
                        populationSummaries[ToKey(summary)].Count = offspring.Count;
                    }
                }

                // Log(day + 1, populationSummaries.Values);
            }

            var population = populationSummaries.Values.Sum(s => s.Count);
            Console.WriteLine(population);
        }

        private static string ToKey(PopulationSummary summary) => ToKey(summary.IsAdult, summary.Time);
        private static string ToKey(bool isAdult, int time) => $"{isAdult}-{time}";

        private static void Log(int day, IEnumerable<PopulationSummary> summaries)
        {
            var logging = summaries
                .GroupBy(s => s.Time)
                .Select(group => $"Time {group.Key} count: {group.Sum(s => s.Count)}");

            Console.WriteLine();
            Console.WriteLine($"Day { day }");
            Console.WriteLine(string.Join("\n", logging));
        }
    }

    public class PopulationSummary
    {
        public int Time { get; set; }
        public bool IsAdult { get; set; }
        public long Count { get; set; }

        public override string ToString()
        {
            return (IsAdult ? "Adult" : "Baby") + $"-{ Time }: { Count }";
        }
    }
}



// Simple solution for fewer days

//var days = 80;
//var initialTimer = 7;
//var population = input.Select(time => new LanternFish { Time = time, isAdult = true });

//        for (var i = 0; i<days; i++)
//        {
//            population = population.Select(fish =>
//            {
//                if (fish.Time > 0)
//                {
//                    fish.Time = fish.Time - 1;
//                }
//                else
//                {
//                    fish.Time = initialTimer - 1;
//                    fish.isAdult = true;
//                }

//                return fish;
//            });

//            var newFish = population.Count(fish => fish.Time == initialTimer - 1 && fish.isAdult);
//            population = population.Concat(new LanternFish[newFish].Select(fish => new LanternFish { Time = initialTimer + 1 }));
//        }

//        Console.WriteLine(population.Count());
//    }

//    public class LanternFish
//    {
//    public int Time { get; set; }
//    public bool isAdult { get; set; }

//    public override string ToString()
//    {
//        return $"{Time} {isAdult}";
//    }
//}
