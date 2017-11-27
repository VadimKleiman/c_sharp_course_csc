using System.Threading;

namespace Queue
{
    public class LockFreeArrayQueue<T> : IArrayQueue<T>
    {
        private int _writeIndex;
        private int _readIndex;
        private int _maximumReadIndex;
        private readonly int _size;
        private readonly T[] _queue;
        private volatile bool _isCleaning;

        public LockFreeArrayQueue(int size)
        {
            _size = size + 1;
            _queue = new T[_size];
            _writeIndex = _readIndex = _maximumReadIndex = 0;
        }

        public void Clear()
        {
            _isCleaning = true;
            var tmp = default(T);
            while (TryDequeue(ref tmp)) {}
            _isCleaning = false;
        }

        public T Dequeue()
        {
            var result = default(T);
            while (!TryDequeue(ref result))
            {
                Thread.Yield();
            }
            return result;
        }

        public void Enqueue(T value)
        {
            while(!TryEnqueue(value))
            {
                Thread.Yield();
            }
        }

        public bool TryDequeue(ref T value)
        {
            int currentRi;
            do
            {
                currentRi = _readIndex;
                var currentMri = _maximumReadIndex;
                if (currentMri % _size == currentRi % _size)
                {
                    return false;
                }
                value = _queue[currentRi % _size];
            } while (Interlocked.CompareExchange(ref _readIndex, currentRi + 1, currentRi) != currentRi);
            return true;
        }

        public bool TryEnqueue(T value)
        {
            while (_isCleaning)
            {
                Thread.Yield();
            }
            int currentWi;
            do
            {
                int currentRi;
                currentRi = _readIndex;
                currentWi = _writeIndex;
                if ((currentWi + 1) % _size == currentRi % _size)
                {
                    return false;
                }
            } while (currentWi != Interlocked.CompareExchange(ref _writeIndex, currentWi + 1, currentWi));
            _queue[currentWi % _size] = value;
            while (Interlocked.CompareExchange(ref _maximumReadIndex, currentWi + 1, currentWi) != currentWi)
            {
                Thread.Yield();
            }
            return true;
        }
    }
}
