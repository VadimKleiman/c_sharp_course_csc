using Xunit;
using Queue;
using System.Threading;

namespace QueueTest
{
    public class QTest
    {
        private const int ThreadCount = 50;
        private const int Iter = 1000;
        
        private IArrayQueue<int> _queue;

        private void Enqueue()
        {
            for (var i = 0; i < 10; i++)
            {
                Assert.True(_queue.TryEnqueue(i));
            }
            Assert.False(_queue.TryEnqueue(42));
        }

        private void Dequeue()
        {
            var check = 0;
            for (var i = 0; i < 10; i++)
            {
                Assert.True(_queue.TryDequeue(ref check));
                Assert.Equal(check, i);
            }
        }

        private void TryEnqueueMultiThread()
        {
            var threads = new Thread[ThreadCount];
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i] = new Thread(() => {
                    for (var j = 0; j < Iter; j++)
                    {
                        _queue.TryEnqueue(j);
                    }
                });
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Start();
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Join();
            }
            var check = new int[Iter];
            var tmp = 0;
            while (_queue.TryDequeue(ref tmp))
            {
                ++check[tmp];
            }
            for (var i = 0; i < Iter; i++)
            {
                Assert.Equal(check[i], ThreadCount);
            }
        }

        private void TryDequeueMultiThread()
        {
            var threads = new Thread[ThreadCount];
            var check = 0;
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i] = new Thread(() => {
                    for (var j = 0; j < Iter; j++)
                    {
                        _queue.TryEnqueue(j);
                    }
                    var ignore = 0;
                    while (_queue.TryDequeue(ref ignore))
                    {
                        Interlocked.Increment(ref check);
                    }
                });
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Start();
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Join();
            }
            Assert.Equal(check, ThreadCount * Iter);
        }
        
        private void EnqueueDegueueMultiThread()
        {
            var threads = new Thread[ThreadCount];
            var check = 0;
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i] = new Thread(() => {
                    for (var j = 0; j < Iter; j++)
                    {
                        _queue.Enqueue(j);
                    }
                    for (var j = 0; j < Iter; j++) 
                    {
                    _queue.Dequeue();
                    Interlocked.Increment(ref check);
                    }
                });
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Start();
            }
            for (var i = 0; i < ThreadCount; i++)
            {
                threads[i].Join();
            }
            Assert.Equal(check, ThreadCount * Iter);
        }
     

        [Fact]
        public void LockFreeEnqueueTest()
        {
            _queue = new LockFreeArrayQueue<int>(10);
            Enqueue();
        }

        [Fact]
        public void BlockEnqueueTest()
        {
            _queue = new BlockArrayQueue<int>(10);
            Enqueue();
        }

        [Fact]
        public void LockFreeDequeueTest()
        {
            _queue = new LockFreeArrayQueue<int>(10);
            Enqueue();
            Dequeue();
        }

        [Fact]
        public void BlockDequeueTest()
        {
            _queue = new BlockArrayQueue<int>(10);
            Enqueue();
            Dequeue();
        }

        [Fact]
        public void LockFreeClearTest()
        {
            _queue = new LockFreeArrayQueue<int>(10);
            Enqueue();
            _queue.Clear();
            var check = 0;
            Assert.False(_queue.TryDequeue(ref check));
        }

        [Fact]
        public void BlockClearTest()
        {
            _queue = new BlockArrayQueue<int>(10);
            Enqueue();
            _queue.Clear();
            var check = 0;
            Assert.False(_queue.TryDequeue(ref check));
        }

        [Fact]
        public void LockFreeEnqueueMultiThreadTest()
        {
            _queue = new LockFreeArrayQueue<int>(ThreadCount * Iter);
            TryEnqueueMultiThread();
        }

        [Fact]
        public void BlockEnqueueMultiThreadTest()
        {
            _queue = new BlockArrayQueue<int>(ThreadCount * Iter);
            TryEnqueueMultiThread();
        }

        [Fact]
        public void LockFreeDequeueMultiThreadTest()
        {
            _queue = new LockFreeArrayQueue<int>(ThreadCount * Iter);
            TryDequeueMultiThread();
        }

        [Fact]
        public void BlockDequeueMultiThreadTest()
        {
            _queue = new BlockArrayQueue<int>(ThreadCount * Iter);
            TryDequeueMultiThread();
        }

        [Fact]
        public void BlockTryEnqueueDequeueMultiThreadTest()
        {
            _queue = new BlockArrayQueue<int>(ThreadCount * Iter);
            EnqueueDegueueMultiThread();
        }

        [Fact]
        public void LockFreeTryEnqueueDequeueMultiThreadTest()
        {
            _queue = new LockFreeArrayQueue<int>(ThreadCount * Iter);
            EnqueueDegueueMultiThread();
        }
    }
}
