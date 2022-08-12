using System;
using System.Collections.Generic;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021.Input
{
    public class Route<T>
    {
        public IEnumerable<BasePosition<T>> Positions { get; set; }
    }
}
