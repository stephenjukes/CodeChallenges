using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PostLockDownParty
{
    class Program
    {
        static void Main(string[] args)
        {
            var guestRelationships = ReadJsonFile("./data.json");
            var guests = GetRelationshipsByGuest(guestRelationships); 
            var guestCount = guests.Count();
            var startCombination = 1234567; // Should really be Math.Pow(<base>, guestCount - 2);
            var endCombination = 8000000;   // Should really be Math.Pow(<base>, guestCount - 1);
            var guestCombinations = new List<GuestCombination>();

            for (var i = startCombination; i < endCombination; i++)
            {
                var trialCombination = i.ToString().PadLeft(guestCount, '0').ToCharArray();
                if (Invalid(guestCount, trialCombination)) continue;

                var guestCombination = ResolveGuests(guests, trialCombination);
                var guestsWithNeighbours = ResolveNeighbours(guestCombination);
                var combinationHappiness = guestsWithNeighbours.Sum(g => g.Neighbours.Sum(n => n.Happiness));

                guestCombinations.Add(new GuestCombination
                {
                    Happiness = combinationHappiness,
                    Guests = guestsWithNeighbours
                });
            }

            var optimalCombination = guestCombinations
                .OrderByDescending(combination => combination.Happiness)
                .FirstOrDefault();

            Display(optimalCombination);
        }

        private static List<GuestRelationship> ReadJsonFile(string path)
        {
            List<GuestRelationship> data;
            using (var reader = new StreamReader(path))
            {
                var json = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<List<GuestRelationship>>(json);
            }

            return data;
        }

        private static IEnumerable<Guest> GetRelationshipsByGuest(List<GuestRelationship> guestRelationships)
        {
            return guestRelationships
                .GroupBy(c => new { c.Name })
                .Select((group, index) => new Guest { Id = index, Name = group.Key.Name, Neighbours = group.ToList() });
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

        private static IEnumerable<Guest> ResolveNeighbours(IEnumerable<Guest> guestCombination)
        {
            return guestCombination
                .Select(guest => {
                    var neighbours = guestCombination
                        .Where(g => Math.Abs(guest.Position - g.Position) == 1 || Math.Abs(guest.Position - g.Position) == guestCombination.Count() - 1)
                        .Select(n => n.Name);

                    var neighbourData = guestCombination.FirstOrDefault(g => g.Name == guest.Name).Neighbours
                        .Where(n => neighbours.Contains(n.Neighbour)).ToList();

                    return new Guest
                    {
                        Id = guest.Id,
                        Name = guest.Name,
                        Position = guest.Position,
                        Neighbours = neighbourData
                    };
                });
        }

        private static void Display(GuestCombination optimalCombination)
        {
            Console.WriteLine($"Optimum combination with total happiness of {optimalCombination.Happiness}");

            foreach (var guest in optimalCombination.Guests)
            {
                var neighbours = string.Join(", ", guest.Neighbours.Select(n => $"{n.Neighbour}: {n.Happiness}"));
                var guestHappiness = guest.Neighbours.Sum(n => n.Happiness);
                Console.WriteLine($"Guest: {guest.Name},\t Happiness: {guestHappiness.ToString().PadLeft(5)},\t Neighbours: {neighbours} ");
            }
        }
    }

    class GuestRelationship
    {
        public string Name { get; set; }
        public int Happiness { get; set; }
        public string Neighbour { get; set; }
    }

    class Guest
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public IEnumerable<GuestRelationship> Neighbours { get; set; }
    }

    class GuestCombination
    {
        public int Happiness { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
    }

}
