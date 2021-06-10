using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.LockdownParty
{
    public class PostLockdownParty
    {
        public static void Run()
        {
            var challengeScopedPermutationsResolver = new ChallengeScopedPermutationsResolver();
            var googleScopedPermutationsResolver = new GoogleScopedPermutationsResolver();

            IPermutationsResolver permutationsResolver = googleScopedPermutationsResolver;
            var guestRelationships = ReadJsonFile("./data.json");
            var guests = GetRelationshipsByGuest(guestRelationships);

            var guestCombinations = permutationsResolver
                .GetPermutations(guests)
                .Where(perm => perm.Guests.First().Name == "Alice") // TODO: Decouple name / Find a way to handle earlier
                .ToList();

            var optimalCombination = guestCombinations
                .OrderByDescending(combination => combination.Happiness)
                .FirstOrDefault();

            Display(optimalCombination);
        }

        private static IEnumerable<Guest> GetRelationshipsByGuest(List<GuestRelationship> guestRelationships)
        {
            return guestRelationships
                .GroupBy(c => new { c.Name })
                .Select((group, index) => new Guest { Id = index, Name = group.Key.Name, Neighbours = group.ToList() });
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
    }

    public class GuestRelationship
    {
        public string Name { get; set; }
        public int Happiness { get; set; }
        public string Neighbour { get; set; }
    }

    public class Guest
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public IEnumerable<GuestRelationship> Neighbours { get; set; }
    }

    public class GuestCombination
    {
        public int Happiness { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
    }
}
