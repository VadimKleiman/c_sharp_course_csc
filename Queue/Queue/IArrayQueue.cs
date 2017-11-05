namespace Queue
{
    public interface IArrayQueue<T>
    {
        bool Enqueue(T value);
        bool Dequeue(ref T value);
        bool TryEnqueue(T value);
        bool TryDequeue(ref T value);
        void Clear();
    }
}
