using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges
{
    public class CellularAutomata
    {
        public static void Run()
        {
            var initialConfiguration = "0100100110010001000100011100110110001010";
            var targetConfiguration = "0001100011111100111111110000111001100000";
            var targetStep = 40;
            var maxRuleSet = 256;

            var rule = 0;
            while (rule < maxRuleSet)
            {
                var ruleSet = GetRuleSet(rule);
                var finalConfiguration = GetFinalConfiguration(initialConfiguration.ToCharArray(), targetStep, ruleSet);

                if (string.Join("", finalConfiguration) == targetConfiguration) break;

                rule++;
            }

            Console.WriteLine($"Solution: {rule}\n");

            var solutionOutput = Output(initialConfiguration.ToCharArray(), targetStep, GetRuleSet(rule));
            Console.WriteLine(solutionOutput);
        }

        private static Dictionary<string, char> GetRuleSet(int rule)
        {
            return ToBinaryString(rule, 8)
                .Select((digit, index) => new { Key = ToBinaryString(7 - index, 3), Value = digit })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static char[] GetFinalConfiguration(char[] configuration, int targetStep, Dictionary<string, char> ruleSet)
        {
            var nextConfiguraion = GetNextConfiguration(configuration, targetStep, ruleSet);

            return targetStep == 1
                ? nextConfiguraion
                : GetFinalConfiguration(nextConfiguraion, targetStep - 1, ruleSet);
        }

        private static string Output(char[] configuration, int targetStep, Dictionary<string, char> ruleSet)
        {
            var nextConfiguraion = GetNextConfiguration(configuration, targetStep, ruleSet);

            var oneSymbol = "X";
            var zeroSymbol = " ";

            return targetStep == 1
                ? Format(nextConfiguraion, oneSymbol, zeroSymbol)
                : Format(configuration, oneSymbol, zeroSymbol) + "\n" + Output(nextConfiguraion, targetStep - 1, ruleSet);
        }

        private static char[] GetNextConfiguration(char[] config, int targetStep, Dictionary<string, char> rules)
        {
            return config
                .Select((digit, i) =>
                {
                    var ruleKey = $"{ config[Mod(i - 1, config.Length)] }{ config[i] }{ config[Mod(i + 1, config.Length)] }";
                    return rules[ruleKey];
                }).ToArray();
        }

        private static string ToBinaryString(int decimalInteger, int padding = 0)
            => Convert.ToString(decimalInteger, 2).PadLeft(padding, '0');

        private static int Mod(int n, int m) => ((n % m) + m) % m;

        private static string Format(char[] configuration, string oneSymbol, string zeroSymbol)
        {
            var mapping = configuration.Select(digit => digit == '1' ? oneSymbol : zeroSymbol);
            return string.Join("", mapping);
        }
    }
}
