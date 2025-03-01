using System.Collections.Generic;

namespace StaleSync.Proto
{
    public sealed class EvQueue<T>
    {
        private readonly Queue<T> _queue;

        public EvQueue()
        {
            _queue = new Queue<T>();
        }

        public int Count => _queue.Count;

        public void Clear()
        {
            _queue.Clear();
        }

        public void Enqueue(T value)
        {
            _queue.Enqueue(value);
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}