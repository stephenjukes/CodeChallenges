//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace TheLift
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var example1 = new int[][]
//            {
//                new int[] {},
//                new int[] {},
//                new int[] { 1 },
//                new int[] {}
//            };

//            var example2 = new int[][]
//            {
//                new int[] {},
//                new int[] { 5, 5, 5, 5, 5, 5, 5 },    // picks up as the lift is going up
//                new int[] { 1, },    // passes, since not travelling in the same direction as lift
//                new int[] { 1, 5, 6 },  // stops for '5' to board (1 cannot board since travelling in opposite direction. 6 cannot board since the lift is now full)
//                new int[] { 6 },     // the lift stops at this floor but '6' cannot board since the lift is full
//                new int[] { 6 },     // the lift stops at this floor, (5). '6' can now board since all '5' have now alighted
//                new int[] { },       // both 6s alight here. The lift is now empty
//                new int[] { 1 },     // 
//                new int[] { 1 },     // in spite of having no more passenger destinations, the lift continues up to collect 
//                new int[] { }
//            };

//            var test2 = new int[][]
//            {
//                new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
//                new int[] { }
//            };


//            // challenge solution: 0, 1, 4, 5, 6, 7, 11, 13, 14, 15, 16, 17, 18, 21, 22, 23, 25, 27, 29, 30, 31, 32, 33, 34, 35, 36, 37, 39, 37, 36, 33, 32, 31, 30, 29, 27, 26, 25, 23, 22, 21, 19, 17, 16, 14, 13, 11, 9, 8, 7, 6, 5, 4, 2, 1, 0, 1, 4, 5, 6, 7, 9, 10, 11, 13, 14, 15, 16, 17, 18, 22, 23, 24, 25, 27, 29, 30, 31, 36, 37, 39, 37, 36, 33, 32, 31, 30, 29, 28, 27, 26, 25, 23, 22, 18, 17, 16, 14, 13, 11, 8, 7, 6, 5, 4, 2, 0, 1, 4, 5, 6, 7, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 23, 25, 29, 30, 33, 35, 36, 39, 37, 36, 35, 34, 33, 32, 31, 30, 29, 27, 26, 25, 24, 23, 22, 20, 16, 15, 14, 13, 12, 11, 8, 7, 5, 4, 3, 1, 0, 1, 4, 5, 6, 7, 11, 13, 14, 15, 16, 17, 18, 26, 27, 28, 31, 32, 36, 37, 36, 33, 32, 31, 30, 29, 28, 27, 25, 22, 18, 14, 13, 11, 9, 7, 6, 4, 1, 4, 5, 6, 7, 10, 11, 13, 14, 15, 16, 17, 22, 28, 30, 31, 34, 36, 38, 39, 36, 35, 32, 31, 30, 29, 27, 25, 22, 18, 16, 14, 13, 11, 8, 7, 6, 2, 0, 6, 7, 9, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 23, 29, 32, 35, 31, 30, 29, 28, 27, 25, 22, 21, 18, 17, 15, 13, 11, 10, 9, 8, 7, 4, 3, 7, 11, 12, 13, 14, 15, 16, 17, 19, 25, 26, 31, 34, 35, 36, 31, 30, 29, 27, 25, 23, 18, 15, 14, 7, 6, 0, 11, 13, 14, 15, 16, 17, 19, 27, 28, 30, 31, 35, 36, 29, 28, 9, 7, 14, 15, 16, 17, 18, 21, 24, 26, 29, 30, 39, 16, 17, 19, 23, 24, 29, 32, 35, 16, 17, 20, 23, 24, 25, 27, 35, 0
//            // challenge solution: 0,1,4,5,6,7,11,13,14,15,16,17,18,21,22,23,25,27,29,30,31,32,33,34,35,36,37,39,37,36,33,32,31,30,29,27,26,25,23,22,21,19,17,16,14,13,11,9,8,7,6,5,4,2,1,0,1,4,5,6,7,9,10,11,13,14,15,16,17,18,22,23,24,25,27,29,30,31,36,37,39,37,36,33,32,31,30,29,28,27,26,25,23,22,18,17,16,14,13,11,8,7,6,5,4,2,0,1,4,5,6,7,11,12,13,14,15,16,17,18,19,20,22,23,25,29,30,33,35,36,39,37,36,35,34,33,32,31,30,29,27,26,25,24,23,22,20,16,15,14,13,12,11,8,7,5,4,3,1,0,1,4,5,6,7,11,13,14,15,16,17,18,26,27,28,31,32,36,37,36,33,32,31,30,29,28,27,25,22,18,14,13,11,9,7,6,4,1,4,5,6,7,10,11,13,14,15,16,17,22,28,30,31,34,36,38,39,36,35,32,31,30,29,27,25,22,18,16,14,13,11,8,7,6,2,0,6,7,9,11,12,13,14,15,16,17,18,20,21,22,23,29,32,35,31,30,29,28,27,25,22,21,18,17,15,13,11,10,9,8,7,4,3,7,11,12,13,14,15,16,17,19,25,26,31,34,35,36,31,30,29,27,25,23,18,15,14,7,6,0,11,13,14,15,16,17,19,27,28,30,31,35,36,29,28,9,7,14,15,16,17,18,21,24,26,29,30,39,16,17,19,23,24,29,32,35,16,17,20,23,24,25,27,35,0
//            var challenge = new int[][]
//            {
//                new int[] { 17,35,27,39,18,16,21,23,27,10,30,24,30,9,37,18,7 },
//                new int[] { 33,20,4,19,22,12,29,5,0,13,4,36,31,6,18,32,30 },
//                new int[] {  },
//                new int[] {  },
//                new int[] { 18,18,36,39,10,38,34 },
//                new int[] { 2,6,22,14 },
//                new int[] { 4,26,28,11,20,9,23 },
//                new int[] { 17,12,16,6,17,14,34,35,31,5,25,0,26,12,19,6,6,0,36 },
//                new int[] {  },
//                new int[] {  },
//                new int[] {  },
//                new int[] { 14,36,6,28,29,22,5,35,4 },
//                new int[] {  },
//                new int[] { 22,27,35,34,7,10,31,9,27,14,17,14,8,4,9,27,19 },
//                new int[] { 8,36,31,21,7,6,6,8,0,30,36,29,30,21,24,18 },
//                new int[] { 26 },
//                new int[] { 34,32,39,18,32,19,24,8,35,8,24,23,29,29,25,35 },
//                new int[] { 1,4,22,9,36,18,28,23,27,20,35,5,24 },
//                new int[] {  },
//                new int[] {  },
//                new int[] {  },
//                new int[] { 32,2 },
//                new int[] { 0,4,11,5,1,32,39,29,15,25 },
//                new int[] { 25,35,21,30,8 },
//                new int[] {  },
//                new int[] { 30,23,31,23,29,15 },
//                new int[] { 12 },
//                new int[] { 21,22,4,14,6,22,22 },
//                new int[] {  },
//                new int[] { 15,9,7,30,28,28 },
//                new int[] { 11,6,34,18,27 },
//                new int[] { 19,22,11,16,18,18,17,3,11,27,28,21,21,14,7,25,37,23,0 },
//                new int[] { 24,33,1,27,11 },
//                new int[] { 19,3 },
//                new int[] {  },
//                new int[] {  },
//                new int[] { 0,11,18,1,33,37,6,35,2,39,7,27 },
//                new int[] { 34,26,39,26,35,29,9,28,7 },
//                new int[] {  },
//                new int[] { 31,23,33,31,6,27,7,14,18,22,6,17,7,28,30,2,20,23,36 }
//            };

//            //while(true)
//            //{
//            // var floors = 40;
//            var maxQueuePerLevel = 20;
//            var array = challenge;

//            // var randomLevels = GenerateLevels(floors, maxQueuePerLevel, determiner => determiner < 0.6);
//            var visited = Dinglemouse.TheLift(array, 8);

//            Display(array, maxQueuePerLevel);
//            Console.WriteLine(string.Join(", ", visited));
//            //}
//        }

//        public static int[][] GenerateLevels(int totalLevels, int maxQueuePerLevel, Func<double, bool> hasQueue)
//        {
//            var r = new Random();
//            var levels = new List<List<int>>();

//            for (var level = 0; level < totalLevels; level++)
//            {
//                var hasQueueDeterminer = r.NextDouble();
//                if (!hasQueue(hasQueueDeterminer))
//                {
//                    levels.Add(new List<int>());
//                    continue;
//                };

//                var queueSize = r.Next(maxQueuePerLevel);
//                var queue = new List<int>();
//                for (var s = 0; s < queueSize; s++)
//                {
//                    var destination = r.Next(totalLevels);
//                    if (destination != level) queue.Add(destination);
//                }

//                levels.Add(queue);
//            }

//            return levels.Select(level => level.ToArray()).ToArray();
//        }

//        public static void Display(int[][] levels, int maxQueuesPerLevel)
//        {
//            Func<int, string> tab = tabs => new String(' ', tabs * 4);
//            var jsonOutput = $"{{\n{tab(1)}levels: [\n" + string.Join(",\n", levels.Select(level => $"{tab(2)}[ { string.Join(", ", level) } ]")) + $"\n{tab(1)}]\n}}";
//            var setupArrayOutput = string.Join(",\n", levels.Select(level => $"new int[] {{ { string.Join(",", level) } }}"));

//            var csvArray = new int?[levels.Length][];
//            for (var level = 0; level < levels.Length; level++)
//            {
//                var queue = csvArray[level] = new int?[maxQueuesPerLevel];
//                for (var destination = 0; destination < levels[level].Length; destination++)
//                {
//                    queue[destination] = levels[level][destination];
//                }
//            }

//            var csvOutput = string.Join("\n", csvArray.Select(level => string.Join(",", level)));

//            Console.WriteLine(setupArrayOutput);
//            Console.WriteLine(csvOutput);
//            Console.WriteLine(jsonOutput);
//        }
//    }

//    public class Dinglemouse
//    {
//        public static int[] TheLift(int[][] queues, int capacity)
//        {
//            var floors = CreateFloors(queues);
//            var lift = new Lift(capacity);

//            var visited = lift.CollectPassengers(floors);

//            return visited;
//        }

//        private static Floor[] CreateFloors(int[][] queues)
//        {
//            return queues
//                .Select((queue, index) => new Floor
//                {
//                    Level = index,
//                    Queue = CreatePersonQueue(index, queue),
//                    CallButton = CreateCallButtons(index, queue)
//                })
//                .ToArray();
//        }

//        private static List<Person> CreatePersonQueue(int level, int[] destinationQueue)
//        {
//            return destinationQueue
//                .Select(destination => new Person
//                {
//                    Destination = destination,
//                    Direction = destination > level ? Direction.Up : Direction.Down
//                })
//                .ToList();
//        }

//        private static Dictionary<Direction, CallButton> CreateCallButtons(int level, int[] destinationQueue)
//        {
//            return new Dictionary<Direction, CallButton>()
//                {
//                    { Direction.Up, new CallButton { IsActive = destinationQueue.Any(destination => destination > level) } },
//                    { Direction.Down, new CallButton { IsActive = destinationQueue.Any(destination => destination < level) } }
//                };
//        }
//    }

//    public class Lift
//    {
//        private Direction[] _directions = new Direction[] { Direction.Up, Direction.Down };
//        private List<int> _destinations = new List<int>();
//        private List<int> _visited = new List<int>() { 0 };
//        private List<Person> _passengers = new List<Person>();
//        private int _trip = 0;
//        private Floor[] _floors;
//        private int _capacity;
//        private Direction _direction;

//        public Lift(int capacity)
//        {
//            _capacity = capacity;
//        }

//        public int[] CollectPassengers(Floor[] floors)
//        {
//            _floors = floors;

//            var existActiveCallButtons = true;  // amend to check if floors have active call buttons. Should be able to refactor to method since used elsewhere
//            while (existActiveCallButtons)
//            {
//                _direction = _directions[_trip++ % 2];
//                existActiveCallButtons = HandleTrip();
//            }

//            if (_visited.Last() != 0) _visited.Add(0);

//            return _visited.ToArray();
//        }

//        private bool HandleTrip()
//        {
//            //lift.Direction = directions[trip++ % 2];
//            _destinations = GetWaitingFloors();

//            while (_destinations.Any())
//            {
//                HandleFloorInterchange();
//            }

//            return _floors
//                .SelectMany(floor => floor.CallButton.Values)
//                .Any(button => button.IsActive);
//        }

//        private List<int> GetWaitingFloors()
//        {
//            return _floors
//                .Where(floor => floor.CallButton[_direction].IsActive)
//                .Select(floor => floor.Level)
//                .OrderBy(level => _direction == Direction.Up ? level : -level)
//                .ToList();
//        }

//        private void HandleFloorInterchange()
//        {
//            var currentFloor = _floors[_destinations.First()];
//            if (_visited.Last() != currentFloor.Level) _visited.Add(currentFloor.Level);

//            DisembarkPassengers(currentFloor);
//            EmbarkPassengers(currentFloor);
//            UpdateCallButton(currentFloor);
//        }

//        private void DisembarkPassengers(Floor currentFloor)
//            => _passengers.RemoveAll(passenger => passenger.Destination == currentFloor.Level);

//        private void EmbarkPassengers(Floor currentFloor)
//        {
//            var boarding = currentFloor.Queue
//                .Where(awaiting => awaiting.Direction == _direction)
//                .Take(_capacity - _passengers.Count);   // Limits but does not require this number

//            _passengers.AddRange(boarding);
//            currentFloor.Queue = currentFloor.Queue.Where(awaiting => !boarding.Contains(awaiting)).ToList();

//            var newDestinations = boarding.Select(passenger => passenger.Destination).ToList();
//            _destinations = UpdatePassengerDestinations(currentFloor, newDestinations);
//        }

//        private List<int> UpdatePassengerDestinations(Floor currentFloor, List<int> newDestinations)
//        {
//            return _destinations
//                .Where(destination => destination != currentFloor.Level)    // remove current floor
//                .Concat(newDestinations)                                    // add new destinations
//                .Distinct()
//                .OrderBy(level => _direction == Direction.Up ? level : -level)
//                .ToList();
//        }

//        private void UpdateCallButton(Floor currentFloor)
//            // Consider refactoring to method due to repeated functionality in setup
//            => currentFloor.CallButton[_direction].IsActive = currentFloor.Queue.Any(awaiting => awaiting.Direction == _direction);
//    }

//    public class Floor
//    {
//        public int Level { get; set; }
//        public Dictionary<Direction, CallButton> CallButton { get; set; }
//        public List<Person> Queue { get; set; }
//    }

//    public class CallButton
//    {
//        public bool IsActive { get; set; }
//    }

//    public class Person
//    {
//        public int Destination { get; set; }
//        public Direction Direction { get; set; }
//    }

//    public enum Direction
//    {
//        Up,
//        Down
//    }
//}
