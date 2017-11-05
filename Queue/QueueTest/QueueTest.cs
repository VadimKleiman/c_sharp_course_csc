using Xunit;
using Queue;
using System.Threading;

namespace QueueTest
{
    public class QTest
    {
        private const int threadCount = 50;
        private const int iter = 1000;
        
        private IArrayQueue<int> q;

        private void Enqueue()
        {
            for (int i = 0; i < 10; i++)
            {
                Assert.True(q.Enqueue(i));
            }
            Assert.False(q.Enqueue(42));
        }

        private void Dequeue()
        {
            int check = 0;
            for (int i = 0; i < 10; i++)
            {
                Assert.True(q.Dequeue(ref check));
                Assert.Equal(check, i);
            }
        }

        private void EnqueueMultiThread()
        {
            var threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() => {
                    for (int j = 0; j < iter; j++)
                    {
                        q.Enqueue(j);
                    }
                });
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            int []check = new int[iter];
            int tmp = 0;
            while (q.Dequeue(ref tmp))
            {
                ++check[tmp];
            }
            for (int i = 0; i < iter; i++)
            {
                Assert.Equal(check[i], threadCount);
            }
        }

        private void DequeueMultiThread()
        {
            var threads = new Thread[threadCount];
            int check = 0;
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() => {
                    for (int j = 0; j < iter; j++)
                    {
                        q.Enqueue(j);
                    }
                    int ignore = 0;
                    while (q.Dequeue(ref ignore))
                    {
                        Interlocked.Increment(ref check);
                    }
                });
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            for (int i = 0; i < iter; i++)
            {
                Assert.Equal(check, threadCount * iter);
            }
        }

        [Fact]
        public void LockFreeEnqueueTest()
        {
            q = new LockFreeArrayQueue<int>(10);
            Enqueue();
        }

        [Fact]
        public void BlockEnqueueTest()
        {
            q = new BlockArrayQueue<int>(10);
            Enqueue();
        }

        [Fact]
        public void LockFreeDequeueTest()
        {
            q = new LockFreeArrayQueue<int>(10);
            Enqueue();
            Dequeue();
        }

        [Fact]
        public void BlockDequeueTest()
        {
            q = new BlockArrayQueue<int>(10);
            Enqueue();
            Dequeue();
        }

        [Fact]
        public void LockFreeClearTest()
        {
            q = new LockFreeArrayQueue<int>(10);
            Enqueue();
            q.Clear();
            int check = 0;
            Assert.False(q.Dequeue(ref check));
        }

        [Fact]
        public void BlockClearTest()
        {
            q = new BlockArrayQueue<int>(10);
            Enqueue();
            q.Clear();
            int check = 0;
            Assert.False(q.Dequeue(ref check));
        }

        [Fact]
        public void LockFreeEnqueueMultiThreadTest()
        {
            q = new LockFreeArrayQueue<int>(threadCount * iter);
            EnqueueMultiThread();
        }

        [Fact]
        public void BlockEnqueueMultiThreadTest()
        {
            q = new BlockArrayQueue<int>(threadCount * iter);
            EnqueueMultiThread();
        }

        [Fact]
        public void LockFreeDequeueMultiThreadTest()
        {
            q = new LockFreeArrayQueue<int>(threadCount * iter);
            DequeueMultiThread();
        }

        [Fact]
        public void BlockDequeueMultiThreadTest()
        {
            q = new BlockArrayQueue<int>(threadCount * iter);
            DequeueMultiThread();
        }
    }
}
