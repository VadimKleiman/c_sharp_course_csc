using System;
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
            T tmp = default(T);
            while (Dequeue(ref tmp)) {}
            _isCleaning = false;
        }

        public bool Dequeue(ref T value)
        {
            int currentMRI;
            int currentRI;
            do
            {
                currentRI = _readIndex;
                currentMRI = _maximumReadIndex;
                if (currentMRI % _size == currentRI % _size)
                {
                    return false;
                }
                value = _queue[currentRI % _size];
                if (Interlocked.CompareExchange(ref _readIndex, currentRI + 1, currentRI) == currentRI)
                {
                    return true;
                }
            } while (true);
        }

        public bool Enqueue(T value)
        {
            while (_isCleaning)
            {
                Thread.Yield();
            }
            int currentRI;
            int currentWI;
            do
            {
                currentRI = _readIndex;
                currentWI = _writeIndex;
                if ((currentWI + 1) % _size == currentRI % _size)
                {
                    return false;
                }
            } while (currentWI != Interlocked.CompareExchange(ref _writeIndex, currentWI + 1, currentWI));
            _queue[currentWI % _size] = value;
            while (Interlocked.CompareExchange(ref _maximumReadIndex, currentWI + 1, currentWI) != currentWI)
            {
                Thread.Yield();
            }
            return true;
        }

        public bool TryDequeue(ref T value)
        {
            throw new NotImplementedException();
        }

        public bool TryEnqueue(T value)
        {
            throw new NotImplementedException();
        }
    }
}
