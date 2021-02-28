using System;
using System.Collections.Generic;
using System.Text;

namespace PostLockDownParty
{
    interface IPermutationsResolver
    {
        public IEnumerable<GuestCombination> GetPermutations(IEnumerable<Guest> guests);
    }
}
