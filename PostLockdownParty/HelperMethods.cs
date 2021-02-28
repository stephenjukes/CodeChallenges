using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostLockDownParty
{
    // Add to PermutationsResolver abstract class
    public static class HelperMethods
    {
        public static GuestCombination ResolveTableHappiness(IEnumerable<Guest> guestPermutation)
        {
            var guestsWithNeighbours = ResolveNeighbours(guestPermutation);

            var guestCombination = new GuestCombination
            {
                Happiness = guestsWithNeighbours.SelectMany(g => g.Neighbours.Select(n => n.Happiness)).Sum(),
                Guests = guestsWithNeighbours
            };

            return guestCombination;
        }

        public static IEnumerable<Guest> ResolveNeighbours(IEnumerable<Guest> guestPermutation)
        {
            var guestsWithPositions = guestPermutation.Select((guest, index) => 
            {
                return new Guest { Id = guest.Id, Name = guest.Name, Position = index, Neighbours = guest.Neighbours };
            });

            var guestsWithNeigbours = guestsWithPositions.Select(guest => 
            {
                var neighbours = guestsWithPositions
                    .Where(g => Math.Abs(guest.Position - g.Position) == 1 || Math.Abs(guest.Position - g.Position) == guestPermutation.Count() - 1)
                    .Select(n => n.Name);

                var neighbourData = guestPermutation.FirstOrDefault(g => g.Name == guest.Name).Neighbours
                    .Where(n => neighbours.Contains(n.Neighbour)).ToList();

                return new Guest { Id = guest.Id, Name = guest.Name, Position = guest.Position, Neighbours = neighbourData };
            });

            return guestsWithNeigbours;
        }  
    }
}
