using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public class BasePosition<T> : IEquatable<BasePosition<T>>
    {
        public BasePosition()
        {
        }

        public BasePosition(T value, int row, int column)
        {
            Value = value;
            Row = row;
            Column = column;
        }

        public T Value { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public IEnumerable<BasePosition<T>> Neighbours { get; set; }

        public bool Equals([AllowNull] BasePosition<T> other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value}@({Row}, {Column})";
        }
    }
}
