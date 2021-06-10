using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges
{
    public class NewPasswordGenerator
    {
        private static readonly string _numberSystem = "abcdefghijklmnopqrstuvwxyz";

        private static readonly Func<string, bool>[] _passwordValidations = new Func<string, bool>[]
        {
            trial => new SequenceCheck((current, previous) => current == previous, length => length >= 2, quantity => quantity >= 2).Check(trial),
            trial => new SequenceCheck((current, previous) => (int)current == (int)previous + 1, length => length >= 3, quantity => quantity >= 1).Check(trial),
            trial => !trial.ToCharArray().Any(c => "oil".Contains(c))
        };

        public static void Run()
        {
            var trial = "hxbxwxba";
            var isValidPassword = false;

            while (!isValidPassword)
            {
                trial = Increment(trial);
                isValidPassword = Validate(trial);

                Console.WriteLine(trial);
            };

            Console.WriteLine($"\nNext valid password:\n{trial}");
        }

        private static string Increment(string password)
        {
            if (string.IsNullOrEmpty(password)) return _numberSystem.First().ToString();

            var baseIndexOfLastPasswordCharacter = _numberSystem.IndexOf(password.Last());
            var passwordMinusLastCharacter = password.Substring(0, password.Length - 1);

            return (password.Last() != _numberSystem.Last())
                ? passwordMinusLastCharacter + _numberSystem[baseIndexOfLastPasswordCharacter + 1]
                : Increment(passwordMinusLastCharacter) + _numberSystem.First();
        }

        private static bool Validate(string password) => !_passwordValidations.Any(rule => rule(password) == false);
    }

    public class SequenceCheck
    {
        private readonly Func<char, char, bool> _isSequenceMember;
        public SequenceCheck(Func<char, char, bool> isSequenceMember, Func<int, bool> isRequiredLength, Func<int, bool> isRequiredQuantity)
        {
            _isSequenceMember = isSequenceMember;
            IsRequiredLength = isRequiredLength;
            IsRequiredQuantity = isRequiredQuantity;
        }

        public Func<int, bool> IsRequiredLength { get; set; }
        public Func<int, bool> IsRequiredQuantity { get; set; }

        public bool Check(string password)
        {
            var sequences = new List<StringBuilder>();
            StringBuilder currentSequence = null;
            var previousCharacter = (char)0;

            foreach (var character in password.ToCharArray())
            {
                if (previousCharacter == (char)0
                        || !_isSequenceMember(character, previousCharacter)
                        || IsRequiredLength(currentSequence.Length))
                {
                    currentSequence = new StringBuilder(character.ToString());
                    sequences.Add(currentSequence);
                }
                else
                {
                    currentSequence.Append(character.ToString());
                }

                previousCharacter = character;
            }

            var validSequences = sequences.Where(s => IsRequiredLength(s.Length));
            return IsRequiredQuantity(validSequences.Count());
        }
    }
}
