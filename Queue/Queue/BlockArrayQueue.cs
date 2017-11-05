using System;
using System.Threading;

namespace Queue
{
    public class BlockArrayQueue<T> : IArrayQueue<T>
    {
        private readonly int _size;
        private int _writeIndex;
        private int _readIndex;
        private readonly T[] _queue;
        private readonly Object _myLock = new Object();

        public BlockArrayQueue(int size)
        {
            _size = size + 1;
            _queue = new T[_size];
            _writeIndex = _readIndex = 0;
        }
        public bool Enqueue(T value)
        {
            lock(_myLock)
            {
                return _Enqueue(value);
            }
        }

        public bool Dequeue(ref T value)
        {
            lock(_myLock)
            {
                return _Dequeue(ref value);
            }
        }

        public bool TryEnqueue(T value)
        {
            if (Monitor.TryEnter(_myLock))
            {
                try
                {
                    return _Enqueue(value);
                }
                finally
                {
                    Monitor.Exit(_myLock);
                }
            }
            return false;
        }

        public bool TryDequeue(ref T value)
        {
            if (Monitor.TryEnter(_myLock))
            {
                try
                {
                    return _Dequeue(ref value);
                }
                finally
                {
                    Monitor.Exit(_myLock);
                }
            }
            return false;
        }

        public void Clear()
        {
            lock(_myLock)
            {
                _writeIndex = _readIndex = 0;
            }
        }

        private bool _Enqueue(T value)
        {
            if ((_writeIndex + 1) % _size == _readIndex % _size)
            {
                return false;
            }
            _queue[_writeIndex % _size] = value;
            ++_writeIndex;
            return true;
        }

        private bool _Dequeue(ref T value)
        {
            if (_readIndex % _size == _writeIndex % _size)
            {
                return false;
            }
            value = _queue[_readIndex % _size];
            ++_readIndex;
            return true;
        }
    }
}
