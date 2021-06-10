using System;
using System.Text;
using System.Drawing;
using System.Linq;

namespace NbsCodeChallenges
{
    public static class Steganography
    {
        public static void Run()
        {
            var image = new Bitmap("./challenge.png");
            var leastSignificantBits = new StringBuilder();

            for(var y = 0; y < image.Height; y++)
            {
                for(var x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    foreach (var value in new int[] { pixel.R, pixel.G, pixel.B })
                    {
                        leastSignificantBits.Append(GetLeastSignificantBit(value));
                    }
                }
            }

            var characters = leastSignificantBits.ToString()
                .Select((c, i) => new { Value = c, Index = i })         // add indices for grouping
                .GroupBy(c => c.Index / 8)                              // group into byte lengths
                .Select(group => group.OrderByDescending(c => c.Index)) // reverse bits within each byte, because ...
                .Select(group => group.Select(g => g.Value))            // remove indices
                .Select(group => string.Join("", group))                // convert char array 'byte' to string
                .Select(group => Convert.ToInt32(group, 2))             // convert to decimal
                .Select(integer => (char)integer);                      // convert to character

            var stringResult = string.Join("", characters);
            var termination = stringResult.IndexOf('\0');
            var message = termination > 0 ? stringResult.Substring(0, termination) : "termination not found";

            Console.WriteLine(message);
        }

        private static char GetLeastSignificantBit(int colorValue) 
            => Convert.ToString(colorValue, 2).ToCharArray().Last(); 
    }
}
