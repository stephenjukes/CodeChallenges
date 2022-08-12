using NbsCodeChallenges.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NbsCodeChallenges
{
    public class BuildYourLaptop
    {
        private static Step[] _steps;

        public static void Run()
        {
            var path = Path.Combine(Config.BaseUrl, "BuildYourOwnLaptop", "dependencies.txt");

            // get steps and dependencies from file
            var stepConditions = File
                .ReadAllLines(path)
                .Select(line => new Regex(@"Step ([A-Z]) must be finished before step ([A-Z]) can begin").Match(line).Groups)
                .Select(group => new StepCondition { StepName = group[2].Value, DependencyName = group[1].Value });

            // set up dictionary for all steps names
            var stepsByValue = stepConditions
                .Aggregate(new List<string>(), (accumulator, stepCondition) =>
                    {
                        accumulator.Add(stepCondition.StepName);
                        accumulator.Add(stepCondition.DependencyName);

                        return accumulator;
                    })
                .Distinct()
                .ToDictionary(s => s, s => new Step { Name = s });

            // populate step dependencies
            foreach(var stepCondition in stepConditions)
            {
                var dependency = stepsByValue[stepCondition.DependencyName];
                stepsByValue[stepCondition.StepName].Dependencies.Add(dependency);
            }

            _steps = stepsByValue.Values.ToArray();

            var orderedSteps = RecurseSteps();
            
            var isValidOrder = stepConditions.All(stepCondition => 
                orderedSteps.IndexOf(stepCondition.DependencyName) < orderedSteps.IndexOf(stepCondition.StepName));

            Console.WriteLine(orderedSteps);
            Console.WriteLine($"IsValid: {isValidOrder}");
        }

        private static string RecurseSteps(string previousStepName = null)
        {
            var nextViableSteps = _steps
                .Where(s =>
                    (previousStepName == null || s.Dependencies.Select(d => d.Name).Contains(previousStepName))
                    && s.Dependencies.All(d => d.Complete))
                .OrderBy(s => s.Name);

            var steps = nextViableSteps.Select(step =>
            {
                step.Complete = true;
                return step.Name + RecurseSteps(step.Name);
            });

            return string.Join("", steps);
        }
    }

    public class StepCondition
    {
        public string StepName { get; set; }
        public string DependencyName { get; set; }
    }

    public class Step
    {
        public string Name { get; set; }
        public bool Complete { get; set; }
        public List<Step> Dependencies { get; } = new List<Step>();

        public override string ToString()
        {
            return Name;
        }
    }
}
