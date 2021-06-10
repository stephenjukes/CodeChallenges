using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges
{
    class OldPostOfficeTrip
    {
        public static void Run()
        {
            var stringMap = new string[]
            {
            "O..x......",
            "xx.....xx.",
            ".x....xx.x",
            "x.x.......",
            "...xx..xx.",
            "...x..x...",
            "....x...x.",
            "x.xx....x.",
            "..xx.xx.xx",
            "....xx...H"
            };

            var map = PathFinder.ConvertToCellMap(stringMap);

            var directions = new Func<Cell, Cell[][], Cell>[]
            {
                (cell, map) => map[cell.Row + 1][cell.Column],  // South
                (cell, map) => map[cell.Row][cell.Column + 1]   // East
            };

            var pathfinder = new PathFinder(map, directions);

            var office = map.First().First();
            var hotel = map.Last().Last();
            var routes = pathfinder.GetRoutes(office, hotel);

            var days = Math.Floor((double)routes.Count() / 2);
            Console.WriteLine(days);
        }
    }

    public class PathFinder
    {
        private Cell[][] _map;
        private Func<Cell, Cell[][], Cell>[] _directions;

        public PathFinder(Cell[][] map, Func<Cell, Cell[][], Cell>[] directions)
        {
            _map = map;
            _directions = directions;
        }

        public static Cell[][] ConvertToCellMap(string[] stringMap)
        {
            return stringMap
                .Select((row, rowIndex) => row
                    .Select((column, columnIndex) =>
                        new Cell { Row = rowIndex, Column = columnIndex, Symbol = column }).ToArray()).ToArray();
        }

        public IEnumerable<Route> GetRoutes(Cell origin, Cell destination)
            => GetRoutes(new Route { Cells = new List<Cell> { origin } }, destination);

        private IEnumerable<Route> GetRoutes(Route route, Cell destination)
        {
            var currentCell = route.Cells.Last();
            var completedRoutes = new List<Route>();

            if (currentCell == destination) return new Route[] { route };

            foreach (var direction in _directions)
            {
                var isValidRoute = TryMove(currentCell, direction, out var nextCell);

                if (isValidRoute)
                {
                    route.Cells.Add(nextCell);
                    completedRoutes = completedRoutes.Concat(GetRoutes(route, destination)).ToList();
                }
            }

            return completedRoutes;
        }

        private bool TryMove(Cell origin, Func<Cell, Cell[][], Cell> direction, out Cell destination)
        {
            destination = null;

            try
            {
                destination = direction(origin, _map);
            }
            catch
            {
                return false;
            }

            if (destination.Symbol == 'x') return false;

            return true;
        }
    }

    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public char Symbol { get; set; }
    }

    public class Route
    {
        public List<Cell> Cells { get; set; }
    }
}
