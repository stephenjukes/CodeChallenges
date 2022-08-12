using NbsCodeChallenges.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NbsCodeChallenges
{
    public static class MorseCode
    {
        private static readonly int letterInterval = 3;
        private static readonly int wordInterval = 7;

        private static readonly DelimeterSet dateTimeDelimeters = new DelimeterSet(null, ", ", null);
        private static readonly DelimeterSet sentenceDelimeters = new DelimeterSet(new string(' ', 12), "\n", "\n");
        private static readonly DelimeterSet wordDelimeters = new DelimeterSet(null, "\t", " ");
        private static readonly DelimeterSet letterDelimeters = new DelimeterSet(null, " ", string.Empty);

        private static readonly Dictionary<int, string> intervalToMark = new Dictionary<int, string>()
        {
            { 1, "." },
            { 3, "-" }
        };

        public static void Run()
        {
            var morseDecoder = File.ReadLines(Config.BaseUrl + "MorseCode\\MorseCodeKey.txt")
                .Select(translation => translation.Split("|").Select(keyValue => keyValue.Trim()).ToArray())
                .ToDictionary(t => t[1], t => t[0]);

            var datetimes = File.ReadLines(Config.BaseUrl + "MorseCode\\DateTimes.txt").ToArray();
            var datetimeSentences = GetDatetimeSentences(datetimes);
            var unitInterval = GetUnitInterval(datetimeSentences.SelectMany(s => s).ToArray());
            var morseSentences = datetimeSentences.Select(sentence => ResolveMorseFromDateTimes(sentence, unitInterval));

            var message = DecodeMorse(
                string.Join(sentenceDelimeters.Code, morseSentences),
                new DelimeterSet[] { sentenceDelimeters, wordDelimeters, letterDelimeters },
                letter => morseDecoder[letter]);

            Console.WriteLine(message); 
        }

        private static IEnumerable<DateTime[]> GetDatetimeSentences(string[] data)
        {
            return string.Join(dateTimeDelimeters.Code, data)
                 .Split(sentenceDelimeters.ParseDatetimeFile)
                 .Select(sentence => sentence
                     .Split(dateTimeDelimeters.Code)
                     .Where(dt => DateTime.TryParse(dt, out var datetime))
                     .Select(dt => DateTime.Parse(dt))
                     .ToArray());
        }

        private static int GetUnitInterval(DateTime[] dateTimes)
        {
            int? shortest = null;

            for (var i = 1; i < dateTimes.Length; i++)
            {
                var intervalLength = (int)(dateTimes[i] - dateTimes[i - 1]).TotalMilliseconds;
                if (shortest == null || intervalLength < shortest) shortest = intervalLength;
            }

            return (int)shortest;
        }

        private static string ResolveMorseFromDateTimes(DateTime[] sentence, int unitInterval)
        {
            var intervals = new List<string>();

            for (var i = 1; i < sentence.Length; i++)
            {
                var interval = (sentence[i] - sentence[i - 1]).TotalMilliseconds;
                var unitBasedInterval = (int)interval / unitInterval;

                var symbol = GetSymbol(unitBasedInterval, i);
                if (symbol != null) intervals.Add(symbol);
            }

            return string.Join(string.Empty, intervals);
        }

        private static string GetSymbol(int unitInterval, int index)
        {
            var isMark = index % 2 != 0;

            if (isMark)
            {
                return intervalToMark[unitInterval];
            }
            else
            {
                if (unitInterval == letterInterval) return letterDelimeters.Code;
                if (unitInterval == wordInterval) return wordDelimeters.Code;
                
                return null;    // space between marks
            }
        }

        private static string DecodeMorse(string message, IEnumerable<DelimeterSet> delimeterSets, Func<string, string> decoderFunc)
        {
            var delimeterSet = delimeterSets.FirstOrDefault();

            var decoded = message
                .Split(delimeterSet.Code)
                .Where(entity => !string.IsNullOrEmpty(entity))
                .Select(entity => delimeterSets.Count() == 1
                    ? decoderFunc(entity)
                    : DecodeMorse(entity, delimeterSets.Skip(1), decoderFunc));
                    
            return string.Join(delimeterSet.Decoded, decoded);
        }
    }

    public class DelimeterSet
    {
        public DelimeterSet(string parseDateTimes, string code, string decoded)
        {
            ParseDatetimeFile = parseDateTimes;
            Code = code;
            Decoded = decoded;
        }

        public string ParseDatetimeFile { get; set; }
        public string Code { get; set; }
        public string Decoded { get; set; }
    }
}
