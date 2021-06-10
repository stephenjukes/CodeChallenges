using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.TheLift
{
    public class Lift_Challenge
    {
        public static void Run()
        {
            var queues = new int[][] { };   // require population
            var capacity = 8;

            var visited = GetVisitedFloors(queues, capacity);
            Console.WriteLine(string.Join(", ", visited));
        }

        public static int[] GetVisitedFloors(int[][] queues, int capacity)
        {
            var floors = CreateFloors(queues);
            var lift = new Lift(capacity);

            var visited = lift.CollectPassengers(floors);

            return visited;
        }

        private static Floor[] CreateFloors(int[][] queues)
        {
            return queues
                .Select((queue, index) => new Floor
                {
                    Level = index,
                    Queue = CreatePersonQueue(index, queue),
                    CallButton = CreateCallButtons(index, queue)
                })
                .ToArray();
        }

        private static List<Person> CreatePersonQueue(int level, int[] destinationQueue)
        {
            return destinationQueue
                .Select(destination => new Person
                {
                    Destination = destination,
                    Direction = destination > level ? Direction.Up : Direction.Down
                })
                .ToList();
        }

        private static Dictionary<Direction, CallButton> CreateCallButtons(int level, int[] destinationQueue)
        {
            return new Dictionary<Direction, CallButton>()
                {
                    { Direction.Up, new CallButton { IsActive = destinationQueue.Any(destination => destination > level) } },
                    { Direction.Down, new CallButton { IsActive = destinationQueue.Any(destination => destination < level) } }
                };
        }
    }

    public class Lift
    {
        private Direction[] _directions = new Direction[] { Direction.Up, Direction.Down };
        private List<int> _destinations = new List<int>();
        private List<int> _visited = new List<int>() { 0 };
        private List<Person> _passengers = new List<Person>();
        private int _trip = 0;
        private Floor[] _floors;
        private int _capacity;
        private Direction _direction;

        public Lift(int capacity)
        {
            _capacity = capacity;
        }

        public int[] CollectPassengers(Floor[] floors)
        {
            _floors = floors;

            var existActiveCallButtons = true;
            while (existActiveCallButtons)
            {
                _direction = _directions[_trip++ % 2];
                existActiveCallButtons = HandleTrip();
            }

            if (_visited.Last() != 0) _visited.Add(0);

            return _visited.ToArray();
        }

        private bool HandleTrip()
        {
            _destinations = GetWaitingFloors();

            while (_destinations.Any())
            {
                HandleFloorInterchange();
            }

            return _floors
                .SelectMany(floor => floor.CallButton.Values)
                .Any(button => button.IsActive);
        }

        private List<int> GetWaitingFloors()
        {
            return _floors
                .Where(floor => floor.CallButton[_direction].IsActive)
                .Select(floor => floor.Level)
                .OrderBy(level => _direction == Direction.Up ? level : -level)
                .ToList();
        }

        private void HandleFloorInterchange()
        {
            var currentFloor = _floors[_destinations.First()];
            if (_visited.Last() != currentFloor.Level) _visited.Add(currentFloor.Level);

            DisembarkPassengers(currentFloor);
            EmbarkPassengers(currentFloor);
            UpdateCallButton(currentFloor);
        }

        private void DisembarkPassengers(Floor currentFloor)
            => _passengers.RemoveAll(passenger => passenger.Destination == currentFloor.Level);

        private void EmbarkPassengers(Floor currentFloor)
        {
            var boarding = currentFloor.Queue
                .Where(awaiting => awaiting.Direction == _direction)
                .Take(_capacity - _passengers.Count);   // Limits but does not require this number

            _passengers.AddRange(boarding);
            currentFloor.Queue = currentFloor.Queue.Where(awaiting => !boarding.Contains(awaiting)).ToList();

            var newDestinations = boarding.Select(passenger => passenger.Destination).ToList();
            _destinations = UpdatePassengerDestinations(currentFloor, newDestinations);
        }

        private List<int> UpdatePassengerDestinations(Floor currentFloor, List<int> newDestinations)
        {
            return _destinations
                .Where(destination => destination != currentFloor.Level)    // remove current floor
                .Concat(newDestinations)                                    // add new destinations
                .Distinct()
                .OrderBy(level => _direction == Direction.Up ? level : -level)
                .ToList();
        }

        private void UpdateCallButton(Floor currentFloor)
            // Consider refactoring to method due to repeated functionality in setup
            => currentFloor.CallButton[_direction].IsActive = currentFloor.Queue.Any(awaiting => awaiting.Direction == _direction);
    }

    public class Floor
    {
        public int Level { get; set; }
        public Dictionary<Direction, CallButton> CallButton { get; set; }
        public List<Person> Queue { get; set; }
    }

    public class CallButton
    {
        public bool IsActive { get; set; }
    }

    public class Person
    {
        public int Destination { get; set; }
        public Direction Direction { get; set; }
    }

    public enum Direction
    {
        Up,
        Down
    }
}
