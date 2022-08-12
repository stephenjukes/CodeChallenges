using System;
using System.Collections.Generic;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
	public static class Extenstions
	{
		public static IEnumerable<T> ToCollection<T>(this T[,] multiDimensionArray)
		{
			foreach (var element in multiDimensionArray)
			{
				yield return element;
			}
		}

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, T element)
        {
            return collection.Concat(new T[] { element });
        }

        public static IEnumerable<T> Concat<T>(this T element, IEnumerable<T> collection)
        {
            return (new T[] { element }).Concat(collection);
        }

        // Can this be made more generic?
        public static IEnumerable<BasePosition<T>> ToPositionCollection<T>(
            this string[] input,
            Func<string, T> typeConversionFunc,
            Func<string, IEnumerable<string>> splitFunc = null
            )
        {
            splitFunc = splitFunc ?? (row => row.ToCharArray().Select(c => c.ToString()));
            var positions = new List<BasePosition<T>>();
            

            for (var row = 0; row < input.Length; row++)
            {
                var columns = splitFunc(input[row]).ToArray();
                for (var column = 0; column < columns.Count(); column++)
                {
                    var position =(new BasePosition<T>(typeConversionFunc(columns[column]), row, column));

                    positions.Add(position);
                }
            }

            return positions;
        }

        public static IEnumerable<BasePosition<T>> WithAllNeighbours<T>(this IEnumerable<BasePosition<T>> positions)
        {
            foreach (var position in positions)
            {
                position.Neighbours = positions.Where(p =>
                    p.Row >= position.Row - 1 &&
                    p.Row <= position.Row + 1 &&
                    p.Column >= position.Column - 1 &&
                    p.Column <= position.Column + 1 &&
                    !(p.Row == position.Row && p.Column == position.Column));
            }

            return positions;
        }

        public static IEnumerable<BasePosition<T>> WithOrthogonalNeighbours<T>(this IEnumerable<BasePosition<T>> positions)
        {
            foreach (var position in positions)
            {
                position.Neighbours = positions.Where(p =>
                    p.Row == position.Row && p.Column == position.Column + 1 ||
                    p.Row == position.Row && p.Column == position.Column - 1 ||
                    p.Column == position.Column && p.Row == position.Row + 1 ||
                    p.Column == position.Column && p.Row == position.Row - 1);
            }

            return positions;
        }
    }
}
