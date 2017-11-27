using System;
using System.Collections.Generic;
using System.Threading;

namespace Philosopher
{
    public class MyPhilosopher
    {
        private readonly int _id;

        public bool IsDone { get; set; }

        public MyPhilosopher(int id)
        {
            _id = id;
        }

        public void Run(object obj)
        {
            var forks = obj as List<object>;
            if (forks is null)
            {
                return;
            }
            var firstFork = _id;
            var secondFork = (_id + 1) % forks.Count;
            if (firstFork > secondFork)
            {
                var tmp = firstFork;
                firstFork = secondFork;
                secondFork = tmp;
            }
            lock (forks[firstFork])
            {
                lock (forks[secondFork])
                {
                    Console.WriteLine($"Philosopher {_id} takes the forks {firstFork} and {secondFork}");
                    Console.WriteLine($"Philosopher {_id} eating");
                    Thread.Sleep(50);
                }
            }
            IsDone = true;
            Console.WriteLine($"Philosopher {_id} returns the forks {firstFork} and {secondFork}");
        }
    }
}
