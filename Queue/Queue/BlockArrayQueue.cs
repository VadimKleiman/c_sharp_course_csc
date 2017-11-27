using System.Threading;

namespace Queue
{
    public class BlockArrayQueue<T> : IArrayQueue<T>
    {
        private readonly int _size;
        private int _writeIndex;
        private int _readIndex;
        private readonly T[] _queue;
        private readonly object _myLock = new object();

        public BlockArrayQueue(int size)
        {
            _size = size + 1;
            _queue = new T[_size];
            _writeIndex = _readIndex = 0;
        }

        public void Enqueue(T value)
        {
            while(!TryEnqueue(value))
            {
                lock (_myLock)
                {
                    Monitor.Wait(_myLock);
                }
            }
        }

        public T Dequeue()
        {
            var result = default(T);
            while(!TryDequeue(ref result))
            {
                lock (_myLock)
                {
                    Monitor.Wait(_myLock);
                }
            }
            return result;
        }

        public bool TryEnqueue(T value)
        {
            lock(_myLock)
            {
                if ((_writeIndex + 1) % _size == _readIndex % _size)
                {
                    return false;
                }
                _queue[_writeIndex % _size] = value;
                ++_writeIndex;
                Monitor.PulseAll(_myLock);
                return true;
            }
        }

        public bool TryDequeue(ref T value)
        {
            lock (_myLock)
            {
                if (_readIndex % _size == _writeIndex % _size)
                {
                    return false;
                }
                value = _queue[_readIndex % _size];
                _queue[_readIndex % _size] = default(T);
                ++_readIndex;
                Monitor.PulseAll(_myLock);
                return true;
            }
        }

        public void Clear()
        {
            lock(_myLock)
            {
                for (var i = 0; i < _queue.Length; ++i)
                {
                    _queue[i] = default(T);
                }
                _writeIndex = _readIndex = 0;
            }
        }
    }
}
