using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostLockDownParty
{
    public class GoogleScopedPermutationsResolver : IPermutationsResolver
    {
        public IEnumerable<GuestCombination> GetPermutations(IEnumerable<Guest> guests)
        {
            return GetPermutations(guests.ToList(), new List<Guest>());
        }

        private List<GuestCombination> GetPermutations(IEnumerable<Guest> remaining, IEnumerable<Guest> permutation)
        {
            if (!remaining.Any()) return new List<GuestCombination> { HelperMethods.ResolveTableHappiness(permutation) };
                
            return remaining.SelectMany(guest =>
            {
                var newRemaining = remaining.Where(g => g.Name != guest.Name).ToList();
                var newPermutation = permutation.Select(g => g).Concat(new Guest[] { guest }).ToList();

                return GetPermutations(newRemaining, newPermutation);
            }).ToList();
        }
    }
}
