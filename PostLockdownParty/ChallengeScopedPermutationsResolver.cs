using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostLockDownParty
{
    internal class ChallengeScopedPermutationsResolver : IPermutationsResolver
    {
        public IEnumerable<GuestCombination> GetPermutations(IEnumerable<Guest> guests)
        {
            var guestCombinations = new List<GuestCombination>();
            var startCombination = 1234567; // Should really be Math.Pow(<base>, guestCount - 2);
            var endCombination = 8000000;   // Should really be Math.Pow(<base>, guestCount - 1);

            for (var i = startCombination; i < endCombination; i++)
            {
                var trialCombination = i.ToString().PadLeft(guests.Count(), '0').ToCharArray();
                if (Invalid(guests.Count(), trialCombination)) continue;

                var guestCombination = ResolveGuests(guests, trialCombination);
                guestCombinations.Add(HelperMethods.ResolveTableHappiness(guestCombination));
            }

            return guestCombinations;
        }

        private static bool Invalid(int guestCount, char[] trialCombination)
        {
            var allGuestsAreIncluded = trialCombination.Distinct().Count() == guestCount;
            var areAllValidGuestIds = trialCombination.All(c => int.Parse(c.ToString()) < guestCount);

            return !allGuestsAreIncluded || !areAllValidGuestIds;
        }

        private static IEnumerable<Guest> ResolveGuests(IEnumerable<Guest> guests, char[] trialCombination)
        {
            return trialCombination
                .Select(c => guests.FirstOrDefault(g => g.Id == int.Parse(c.ToString())))
                .Select((guest, index) =>
                {
                    guest.Position = index;
                    return guest;
                });
        }
    }
}
