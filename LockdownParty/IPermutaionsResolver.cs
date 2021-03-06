﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NbsCodeChallenges.LockdownParty
{
    public interface IPermutationsResolver
    {
        public IEnumerable<GuestCombination> GetPermutations(IEnumerable<Guest> guests);
    }
}
