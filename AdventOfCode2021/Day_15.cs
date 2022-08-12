using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_15
    {
        private static int _leastRisk;

        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_15_Test.txt");
            var challengeInput = Helpers.GetInput("Day_15.txt");
            var input = challengeInput;

            var positions = input
                .ToPositionCollection(value => int.Parse(value))
                .WithOrthogonalNeighbours();
            
            var start = positions.Single(p => p.Row == 0 && p.Column == 0);
            var end = positions.Single(p => p.Row == input.Length - 1 && p.Column == input.First().Length - 1);
            _leastRisk = input.Count() * input.First().Count() * 9;

            var shortestRoute = GetShortestRoute(start, end, 0, positions);
            var leastRisk = shortestRoute.Sum(p => p.Value) - start.Value;

            Console.WriteLine(_leastRisk);
        }

        private static IEnumerable<BasePosition<int>> GetShortestRoute(
            BasePosition<int> origin,
            BasePosition<int> end,
            int currentRisk,
            IEnumerable<BasePosition<int>> positions)
        {
            currentRisk += origin.Value;

            if (currentRisk > _leastRisk)
            {
                return new BasePosition<int>[0];
            }

            if (origin == end)
            {
                _leastRisk = currentRisk;
                return new BasePosition<int>[] { end };
            }

            return origin.Neighbours
                .Where(n => n.Column > origin.Column || n.Row > origin.Row)
                .Select(n => origin.Concat(GetShortestRoute(n, end, currentRisk, positions)))
                .Where(route => route.Last().Value != default)
                .OrderBy(route => route.Sum(position => position.Value))
                .First();
        }
    }
}


// SUCCESSFUL FOR SMALL MATRICES

//private static IEnumerable<BasePosition<int>> GetShortestRoute(
//    BasePosition<int> origin,
//    BasePosition<int> end,
//    IEnumerable<BasePosition<int>> positions)
//{
//    if (origin == end)
//    {
//        return new BasePosition<int>[] { end };
//    }

//    return origin.Neighbours
//        .Where(n => n.Column > origin.Column || n.Row > origin.Row)
//        .Select(n => origin.Concat(GetShortestRoute(n, end, positions)))
//        .OrderBy(route => route.Sum(position => position.Value))
//        .First();
//}
