using NbsCodeChallenges.Utilities;
using System;
using System.Text;

namespace NbsCodeChallenges.LongestPalindrome
{
    public static class Palindromes
    {
        private static string _input;
        private static int _inputLength;
        private static StringBuilder _currentPalindrome = new StringBuilder();
        private static string _longestPalindrome;
        private static int _largestOffset;

        public static void Run()
        {
            var rawInput = Helpers.GetInput("LongestPalindrome\\input.txt");
            _input = string.Join("", rawInput);
            _inputLength = _input.Length;

            for (var i = 1; i < _inputLength; i++)             
            {
                bool isDoubleRoot;  // eg: aa
                bool isTripleRoot;  // eg: aba

                try
                {
                    isDoubleRoot = _input[i] == _input[i - 1];
                    isTripleRoot = _input[i + 1] == _input[i - 1];
                }
                catch
                {
                    break;
                }

                if (isDoubleRoot)
                {
                    DerivePalindromeFromCentre(
                        initialise: palindrome => palindrome.Append(_input[i] + _input[i]),
                        getPreceding: offset => _input[i - offset - 1],
                        getFollowing: offset => _input[i + offset]);
                }

                if (isTripleRoot)
                {
                    DerivePalindromeFromCentre(
                        initialise: palindrome => palindrome.Append(_input[i]),
                        getPreceding: offset => _input[i - offset],
                        getFollowing: offset => _input[i + offset]);
                }
            }

            Console.WriteLine(_longestPalindrome);
        }

        private static void DerivePalindromeFromCentre(
            Action<StringBuilder> initialise,
            Func<int, char> getPreceding,
            Func<int, char> getFollowing)
        {
            char preceding;
            char following;

            _currentPalindrome.Clear();
            initialise(_currentPalindrome);

            for (var offset = 1; offset < _inputLength; offset++)
            {
                try
                {
                    preceding = getPreceding(offset);
                    following = getFollowing(offset);
                }
                // break if inspection goes beyond limits of collection
                catch
                {
                    RecordIfLargest(offset, _currentPalindrome);
                    break;
                }

                // break if the substring is no longer palindromic
                if (char.ToLower(preceding) != char.ToLower(following))
                {
                    RecordIfLargest(offset, _currentPalindrome);
                    break;
                }

                // otherwise continue extending for further inspection
                _currentPalindrome.Insert(0, preceding);
                _currentPalindrome.Append(following);
            }
        }

        private static void RecordIfLargest(int offset, StringBuilder palindrome)
        {
            if (offset > _largestOffset)
            {
                _longestPalindrome = palindrome.ToString();
                _largestOffset = offset;
            }
        }
    }
}
