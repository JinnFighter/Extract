using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class PriorityQueue<T>
    {
        private readonly Dictionary<int, Queue<T>> _queue = new();
        public int Priority { get; private set; } = -1;
        public int Count { get; private set; }

        public void Enqueue(T obj, int priority)
        {
            if (priority < Priority || Priority < 0)
                Priority = priority;

            if (!_queue.TryGetValue(priority, out _))
                _queue[priority] = new Queue<T>();

            _queue[priority].Enqueue(obj);
            Count++;
        }

        public T Dequeue()
        {
            if (Priority < 0) throw new Exception("Priority Queue is Empty");

            var items = _queue[Priority];
            var res = items.Dequeue();
            Count--;
            if (!items.Any())
            {
                _queue.Remove(Priority);
                Priority = _queue.Keys.Count > 0 ? _queue.Keys.Min() : -1;
            }

            return res;
        }
    }
}