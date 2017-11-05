using System;
using System.Collections.Generic;
using System.Threading;

namespace Philosopher
{
    sealed class Fork
    {
        public bool IsUsing { get; set; }
    }

    sealed class Philosopher
    {
        private readonly int _id;

        public Philosopher(int id)
        {
            _id = id;
        }

        public void Run(Object obj)
        {
            var fork = obj as List<Fork>;
            int firstFork = _id;
            int secondFork = (_id + 1) % fork.Count;
            while (true)
            {
                Thread.Sleep(_id * 500);
                Monitor.Enter(fork);
                try
                {
                    if (!fork[firstFork].IsUsing && !fork[secondFork].IsUsing)
                    {
                        Console.WriteLine("Philosopher {0} takes the forks {1} and {2}", _id, firstFork, secondFork);
                        fork[firstFork].IsUsing = fork[secondFork].IsUsing = true;
                    }
                }
                finally
                {
                    Monitor.Exit(fork);
                }
                Console.WriteLine("Philosopher {0} eating", _id);
                Thread.Sleep(300);
                Monitor.Enter(fork);
                try
                {
                    Console.WriteLine("Philosopher {0} returns the forks {1} and {2}", _id, firstFork, secondFork);
                    fork[firstFork].IsUsing = fork[secondFork].IsUsing = false;
                }
                finally
                {
                    Monitor.Exit(fork);
                }
            }
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            const int count = 10;
            var forks = new List<Fork>();
            var philosophers = new List<Philosopher>();
            var threads = new Thread[count];
            for (int i = 0; i < count; ++i)
            {
                forks.Add(new Fork());
                philosophers.Add(new Philosopher(i));
            }
            for (int i = 0; i < count; ++i)
            {
                threads[i] = new Thread(philosophers[i].Run);
                threads[i].Start(forks);
            }
            for (int i = 0; i < count; ++i)
            {
                threads[i].Join();
            }
        }
    }
}
