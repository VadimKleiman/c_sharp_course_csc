namespace Queue
{
    public interface IArrayQueue<T>
    {
        void Enqueue(T value);
        T Dequeue();
        bool TryEnqueue(T value);
        bool TryDequeue(ref T value);
        void Clear();
    }
}
