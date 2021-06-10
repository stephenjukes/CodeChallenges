using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges
{
    public class FactoriesAndPowerplants
    {
        public static void Run()
        {
            var rawInput = new int[] { 0, -1, 0, 0, -3, 0, 0, 1, 0, 0, 8, 0, 0, 0, 0, -9, 6, 0, 9, 9, 0, 0, 0, 0, 0, 1, -6, 4, 0, 3, 4, 0, 0, 0, -4, 0, 0, 0, 0, -4, 0, -2, 0, 0, 5, 0, 0, 0, 7, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -6, 0, 0, 0, 0, 7, -4, 0, 0, 0, 2, 6, 0, 8, 0, 6, 2, 2, 0, 8, 0, 0, 0, 6, 2, 2, 0, 2, 0, -7, 0, 0, 0, 0, 8, 6, 0, 0, 0, 0, 1, 6, 0, 0, 0, 0, 0, -7, 7, 0, 0, 0, 0, 3, 6, 8, -9, 2, 1, 2, 0, 0, 0, -8, 0, 6, 4, 0, 0, 7, 4, 0, 5, 6, 9, 0, 4, 6, 0, 6, 0, 6, 0, 0, 0, 0, -6, 7, 7, 0, 0, 0, 0, 0, 0, 8, 0, 0, -7, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 2, 0, 0, 2, 4, 0, 0, 4, 0, 0, 4, 0, 0, 0, 6, -1, 0, 0, 3, 10, 0, 0, 0, -5 };
            var testInput = new int[] { 2, -5, 0, 2, 0, 0, 0, 4, 0, 0, -1, 4, 0, 0, 0, 0, -3 };

            var cities = rawInput.Select((power, index) => new City { Index = index, Capacity = power }).ToArray();
            var factories = cities.Where(city => city.Capacity < 0);
            var nearestViablePowerSourcesByFactory = GetNearestViablePowerSourcesByFactory(factories, cities);
            var networkCandidates = GetAllNetworkCandidates(nearestViablePowerSourcesByFactory);

            var optimumNetwork = GetValidNetworks(networkCandidates, cities)
                .OrderBy(network => network.Count(isConnected => isConnected))
                .FirstOrDefault()
                .ToArray();

            Display(optimumNetwork, cities);
        }

        private static IGrouping<City, PowerSourceOption>[] GetNearestViablePowerSourcesByFactory(IEnumerable<City> factories, City[] cities)
        {
            var nearestToLeft = factories.Select(f => GetNearestPowerSourceOption(f, cities, CaptureCities.ToLeft));
            var nearestToRight = factories.Select(f => GetNearestPowerSourceOption(f, cities, CaptureCities.ToRight));
            var nearestToBothSides = factories.Select(f => GetNearestPowerSourceOption(f, cities, CaptureCities.ToBothSides));
           
            return nearestToLeft.Concat(nearestToRight).Concat(nearestToBothSides)
                .Where(option => option != null)
                .GroupBy(option => new { option.Source, option.LeftMostIndex, option.RightMostIndex })   // remove duplicates from 'both side' and 'single side' searches
                .Select(option => new PowerSourceOption(
                    option.Key.Source, option.Key.LeftMostIndex, option.Key.RightMostIndex))
                .GroupBy(option => option.Source)
                .ToArray();
        }

        private static PowerSourceOption GetNearestPowerSourceOption(City city, City[] cities, CityCaptureFuncs captureFuncs)
        {
            for (var i = 0; captureFuncs.HasRemainingOptions(cities.Length, city.Index, i); i++)
            {
                var toSkip = captureFuncs.ToSkip(city.Index, i);
                var toTake = captureFuncs.ToTake(city.Index, i);

                var potentialSources = cities.Skip(toSkip).Take(toTake);
                var combinedPower = potentialSources.Sum(c => c.Capacity);

                if (combinedPower >= 0)
                {
                    return NewPowerSourceOption(city, potentialSources);
                }
            }

            return null;
        }

        private static PowerSourceOption NewPowerSourceOption(City city, IEnumerable<City> potentialSources)
        {
            var powerSources = potentialSources
                .Where(city => city.Capacity > 0)
                .Select(powerSource => powerSource.Index)
                .OrderBy(i => i);

            return new PowerSourceOption
            {
                Source = city,
                LeftMostIndex = Math.Min(city.Index, powerSources.First()),
                RightMostIndex = Math.Max(city.Index, powerSources.Last())
            };
        }

        
        private static IEnumerable<IEnumerable<PowerSourceOption>> GetAllNetworkCandidates(IEnumerable<IEnumerable<PowerSourceOption>> factories)
        {
            // This recursive method combines elements from multiple collections, similar to polynomial multiplication 
            // eg: (a + b)(c + d) = ab + ac + bc + bd
            if (factories.Count() == 1) return factories;

            var factoriesMinusFirst = factories.Skip(1).Take(factories.Count());

            return factories.First()
                .SelectMany(option => GetAllNetworkCandidates(factoriesMinusFirst)
                    .Select(f => f.Concat(new PowerSourceOption[] { option })));
        }

        private static IEnumerable<IEnumerable<bool>> GetValidNetworks(IEnumerable<IEnumerable<PowerSourceOption>> networkCandidates, City[] cities)
        {
            var validNetworks = new List<IEnumerable<bool>>();

            foreach (var networkCandidate in networkCandidates)
            {
                var cables = networkCandidate.Aggregate(new bool[cities.Length - 1], (network, option) => 
                    ConnectCables(option.LeftMostIndex, option.RightMostIndex, network));

                var industrialIslands = ResolveIndustryIslands(cities, cables);
                
                var isValidNetwork = industrialIslands.All(island => island.Sum(city => city.Capacity) >= 0); // ie: +ve powerplants equal or exceed -ve factories
                if (isValidNetwork) validNetworks.Add(cables);
            }

            return validNetworks;
        }

        static private bool[] ConnectCables(int lowerIndex, int upperIndex, bool[] cables)
        {
            return cables
                .Select((isConnected, i) => i.IsBetween(lowerIndex, upperIndex) ? true : isConnected)
                .ToArray();
        }

        static private IEnumerable<IEnumerable<City>> ResolveIndustryIslands(City[] cities, bool[] cables)
        {
            var islands = new List<List<City>>();
            var currentIsland = new List<City>();

            for (var i = 0; i < cables.Length; i++)
            {
                // cables go to the right from each city
                var isNewIsland = i == 0 || (cables[i - 1] == false && cables[i] == true);
                var isContinuationOfIsland = i == 0 || cables[i - 1] == true;
                var isCompleteIsland = (i > 0 && cables[i - 1] == true && cables[i] == false);

                if (isNewIsland) currentIsland = new List<City> { cities[i] };
                if (isContinuationOfIsland) currentIsland.Add(cities[i]);
                if (isCompleteIsland) islands.Add(currentIsland);
            }

            return islands;
        }

        private static void Display(bool[] network, City[] cities)
        {
            var cityConnections = cities.Select((city, i) =>
            {
                var isConnection = i < network.Length && network[i] == true;
                var hasIndustry = city.Capacity != 0;

                return (hasIndustry ? city.Capacity.ToString() : "") + (isConnection ? "__" : "  ");
            });

            Console.WriteLine($"Cables: {network.Count(isConnected => isConnected)}\n");
            Console.WriteLine(string.Join("", cityConnections));
        }
    }

    public class City
    {
        public int Index { get; set; }
        public int Capacity { get; set; }
    }

    public class PowerSourceOption
    {
        public PowerSourceOption() {}

        public PowerSourceOption(City source, int leftmostIndex, int rightmostIndex)
        {
            Source = source;
            LeftMostIndex = leftmostIndex;
            RightMostIndex = rightmostIndex;
        }

        public City Source { get; set; }
        public int LeftMostIndex { get; set; }
        public int RightMostIndex { get; set; }
    }

    public static class ExtensionMethods
    {
        public static bool IsBetween(this int queried, int lower, int upper) => queried >= lower && queried < upper;
    }

    public class CityCaptureFuncs
    {
        public string FuncType { get; set; }
        public Func<int, int, int> ToSkip { get; set; }
        public Func<int, int, int> ToTake { get; set; }
        public Func<int, int, int, bool> HasRemainingOptions { get; set; }
    }

    public static class CaptureCities
    {
        public static CityCaptureFuncs ToLeft = new CityCaptureFuncs
        {
            FuncType = "ToLeft",
            HasRemainingOptions = (cities, cityIndex, i) => i <= cityIndex,
            ToSkip = (cityIndex, i) => Math.Max(0, cityIndex - i),
            ToTake = (cityIndex, i) => i <= cityIndex ? i + 1 : cityIndex + 1
        };

        public static CityCaptureFuncs ToRight = new CityCaptureFuncs
        {
            FuncType = "ToRight",
            HasRemainingOptions = (cities, cityIndex, i) => i <= (cities - cityIndex),
            ToSkip = (cityIndex, i) => cityIndex,
            ToTake = (cityIndex, i) => i + 1
        };

        public static CityCaptureFuncs ToBothSides = new CityCaptureFuncs
        {
            FuncType = "BothSides",
            HasRemainingOptions = (cities, cityIndex, i) => i <= Math.Max(cityIndex, cities - cityIndex),
            ToSkip = (cityIndex, i) => Math.Max(0, cityIndex - i),
            ToTake = (cityIndex, i) => i <= cityIndex ? i * 2 + 1 : cityIndex + i + 1
        };
    }
}