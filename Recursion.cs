using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges
{
    public class Recursion
    {
        public static void Run()
        {
            var tasks = new string[]
            {
                "get some milk",
                "milk the cow",
                "get some hay",
                "find the key",
                "darn his socks",
                "get some wool",
                "chase the mice"
            };

            GetMilk(tasks);
        }

        private static void GetMilk(IEnumerable<string> tasks, int task = 1)
        {
            if (!tasks.Any()) { Console.WriteLine(); return; }

            Console.WriteLine($"{task}: We need to {tasks.First()}");

            GetMilk(tasks.Skip(1), task + 1);

            Console.WriteLine($"{task}: Now we can {tasks.First()}");
        }
    }
}
