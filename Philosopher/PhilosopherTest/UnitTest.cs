using System.Collections.Generic;
using Xunit;
using Philosopher;
using System.Threading.Tasks;

namespace PhilosopherTest
{
    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            const int count = 50;
            var forks = new List<object>();
            var philosophers = new List<MyPhilosopher>();
            var tasks = new Task[count];
            for (var i = 0; i < count; ++i)
            {
                forks.Add(new object());
                philosophers.Add(new MyPhilosopher(i));
            }
            for (var i = 0; i < count; ++i)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    philosophers[i1].Run(forks);
                });
            }
            foreach (var task in tasks)
            {
                task.Start();
            }
            foreach (var task in tasks)
            {
                task.Wait();
            }
            foreach (var ph in philosophers)
            {
                Assert.True(ph.IsDone);
            }
        }
    }
}
