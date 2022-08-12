using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NbsCodeChallenges
{
   public static class TelephoneNumber
   {
        public static void Run()
        {
            var number1 = "01202760582";
            var number2 = "03454569594";

            Console.WriteLine(FormatForDisplay(number1));
            Console.WriteLine(FormatForLink(number1));

            Console.WriteLine(FormatForDisplay(number2));
            Console.WriteLine(FormatForLink(number2));
        }


        public static string FormatForDisplay(string telephoneNumber)
        {
            return Format(telephoneNumber, new TelephoneConfiguration());
        }

        public static string FormatForLink(string telephoneNumber)
        {
            var config = new TelephoneConfiguration
            {
                Delimiter = "-",
                FurtherFormat = number => $"tel:00-44-{ Regex.Replace(number, @"^0", "") }"
            };

            return Format(telephoneNumber, config);
        }

        //public TelephoneBuilder WithCountryCode(byte countryCode = 44)
        //{
        //    _countryCode = $"+{ countryCode }";

        //    Regex.Replace(_mainNumber, @"^0", "");

        //    return this;
        //}     

        private static string Format(string telephoneNumber, TelephoneConfiguration config)
        {
            var format = config.Format ?? DeriveFormatFromNumber(telephoneNumber, TelephoneFormats.UkNumbers);

            var regexParts = format.Split("-").Select(part => $"(\\d{{{ part }}})");
            var regexPattern = string.Join("", regexParts);

            var replacementParts = regexParts.Select((part, index) => $"${ index + 1 }");
            var replacement = string.Join(config.Delimiter, replacementParts);

            var formattedMainNumber = Regex.Replace(telephoneNumber, regexPattern, replacement);

            return config.FurtherFormat(formattedMainNumber);
        }

        private static string DeriveFormatFromNumber(string telephoneNumber, IEnumerable<PhoneNumber> mappings)
        {
            return mappings
                .Aggregate((accumulator, number) =>
                    new Regex(number.Pattern).IsMatch(telephoneNumber) ? number : accumulator)
                .Format;
        }
    }

    public static class TelephoneFormats
    {
        // http://www.area-codes.org.uk/formatting.php
        public static PhoneNumber[] UkNumbers = new PhoneNumber[]
        {
            new PhoneNumber(@"^01", "5-5"),
            new PhoneNumber(@"^01", "5-6"),
            new PhoneNumber(@"^011", "4-3-4"),
            new PhoneNumber(@"^01\d1", "4-3-4"),
            new PhoneNumber(@"^013397", "6-5"),
            new PhoneNumber(@"^013398", "6-5"),
            new PhoneNumber(@"^013873", "6-5"),
            new PhoneNumber(@"^015242", "6-5"),
            new PhoneNumber(@"^015394", "6-5"),
            new PhoneNumber(@"^015395", "6-5"),
            new PhoneNumber(@"^015396", "6-5"),
            new PhoneNumber(@"^016973", "6-5"),
            new PhoneNumber(@"^016974", "6-5"),
            new PhoneNumber(@"^016977", "6-5"),
            new PhoneNumber(@"^016977", "6-5"),
            new PhoneNumber(@"^017683", "6-5"),
            new PhoneNumber(@"^017684", "6-5"),
            new PhoneNumber(@"^017687", "6-5"),
            new PhoneNumber(@"^019467", "6-5"),
            new PhoneNumber(@"^019755", "6-5"),
            new PhoneNumber(@"^019756", "6-5"),
            new PhoneNumber(@"^02", "3-4-4"),
            new PhoneNumber(@"^03", "4-3-4"),
            new PhoneNumber(@"^05", "5-6"),
            new PhoneNumber(@"^07", "5-6"),
            new PhoneNumber(@"^0800", "4-6"),
            new PhoneNumber(@"^08", "4-3-4"),
            new PhoneNumber(@"^09", "4-3-4")
        };
    }

    public class PhoneNumber
    {
        public PhoneNumber(string pattern, string format)
        {
            Pattern = pattern;
            Format = format;
        }

        public string Pattern { get; set; }
        public string Format { get; set; }
    }

    // spacing
    // delimiter
    // extra formatting eg: tel:
    // country code

    public class TelephoneConfiguration
    {
        public string Format { get; set; }
        public Func<string, string> FurtherFormat { get; set; } = number => number;
        public string Delimiter { get; set; } = " ";
    }
}
