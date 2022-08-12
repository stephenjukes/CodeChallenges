using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Day_14
    {
        public static void Run()
        {
            var testInput = Helpers.GetInput("Day_14_Test.txt");
            // var challengeInput = Helpers.GetInput("Day_14.txt");
            var input = testInput;

            var inputParts = string.Join("\n", input).Split("\n\n");

            var template = inputParts.First();
           
            var insertionRules = inputParts.Last().Split("\n")
                .Select(line => line.Split(" -> "))
                .ToDictionary(line => line.First(), line => line[0][0].ToString() + line.Last() + line[0][1].ToString() )
                ;

            var steps = 10;
            var charCombinationCount = 2;

            for (var step = 0; step < steps; step++)
            {
               
                var templateMappings = template.GetMappings(charCombinationCount, step, insertionRules);
                template = templateMappings.ToNewTemplate(template);

                var newIsertionRules = templateMappings.ToDictionary(
                    m => m, 
                    m => m.GetMappings(charCombinationCount, step, insertionRules).ToNewTemplate(template));

                insertionRules = newIsertionRules;
                charCombinationCount = insertionRules.Keys.First().Count();
            }

            var characterFrequencies = template
                .GroupBy(character => character)
                .OrderByDescending(group => group.Count());

            var mostFrequent = characterFrequencies.First();
            var leastFrequent = characterFrequencies.Last();
            var part1Solution = characterFrequencies.First().Count() - characterFrequencies.Last().Count();

            Console.WriteLine(part1Solution);
        }

        //private static IEnumerable<string> GetMappings(string template, int charCombinationCount, int step, Dictionary<string, string> insertionRules)
        //{
        //    return template
        //        .Select((character, i) => i < template.Length - (charCombinationCount - 1)
        //            ? insertionRules[template.Substring(i, charCombinationCount)]
        //            : string.Empty)
        //        .Where(mapping => !string.IsNullOrEmpty(mapping));
        //}
    }

    public class Mapping
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class Day14Extensions
    {
        public static IEnumerable<string> GetMappings(this string template, int charCombinationCount, int step, Dictionary<string, string> insertionRules)
        {
            var skip = Math.Pow(2, step);

            return template
                .Select((character, i) => i % skip == 0 && i < template.Length - (charCombinationCount - 1)
                    ? insertionRules[template.Substring(i, charCombinationCount)]
                    : string.Empty)
                .Where(mapping => !string.IsNullOrEmpty(mapping));
        }

        public static string ToNewTemplate(this IEnumerable<string> mappings, string template)
        {
            return template.First() + string.Join("", mappings.Select(m => m.Substring(1)));
        }
    }
}


//public static void Run()
//{
//    var testInput = Helpers.GetInput("Day_14_Test.txt");
//    var challengeInput = Helpers.GetInput("Day_14.txt");
//    var input = challengeInput;

//    var inputParts = string.Join("\n", input).Split("\n\n");

//    var template = inputParts.First();

//    var insertionRules = inputParts.Last().Split("\n")
//        .Select(line => line.Split(" -> "))
//        .ToDictionary(line => line.First(), line => line.Last());

//    var steps = 40;

//    for (var i = 0; i < steps; i++)
//    {
//        template = template.First() + string.Join("",
//            template.Select((character, i) => i < template.Length - 1
//                ? insertionRules[$"{character}{template[i + 1]}"] + template[i + 1]
//                : string.Empty));
//    }

//    var characterFrequencies = template
//        .GroupBy(character => character)
//        .OrderByDescending(group => group.Count());

//    var mostFrequent = characterFrequencies.First();
//    var leastFrequent = characterFrequencies.Last();
//    var part1Solution = characterFrequencies.First().Count() - characterFrequencies.Last().Count();

//    Console.WriteLine(part1Solution);
//}