using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
    public class ReactiveStack<T> : IReactiveStack<T>
    {
        private readonly GenericEventArg<IEnumerable<T>> _onClearArg;
        private readonly GenericEventArg<T> _onPopArg;
        private readonly GenericEventArg<T> _onPushArg;
        private readonly object _sender;
        private readonly Stack<T> _stack;
        
        public ReactiveStack() : this(0, null)
        {
        }

        public ReactiveStack(object sender) : this(0, sender)
        {
        }

        public ReactiveStack(int count) : this(count, null)
        {
        }

        public ReactiveStack(int count, object sender)
        {
            _stack = new Stack<T>(count);
            _onPushArg = new GenericEventArg<T>();
            _onClearArg = new GenericEventArg<IEnumerable<T>>();
            _onPopArg = new GenericEventArg<T>();
            _sender = sender;
        }

        public event EventHandler<GenericEventArg<T>> OnPushItem;
        public event EventHandler<GenericEventArg<IEnumerable<T>>> OnClear;
        public event EventHandler<GenericEventArg<T>> OnPopItem;
        public T Peek()
        {
            return _stack.Peek();
        }

        int IReactiveStack<T>.Count => _stack.Count;

        public void Push(T item)
        {
            _stack.Push(item);
            FireOnPush(item);
        }

        public T Pop()
        {
            var item = _stack.Pop();
            FireOnPop(item);
            return item;
        }

        public void Clear()
        {
            var items = _stack.ToList();
            _stack.Clear();
            FireOnClear(items);
        }

        private void FireOnPush(T item)
        {
            if (OnPushItem == null) return;
            
            _onPushArg.Value = item;
            OnPushItem(_sender, _onPushArg);
        }

        private void FireOnPop(T item)
        {
            if (OnPopItem == null)
                return;

            _onPopArg.Value = item;
            OnPopItem(_sender, _onPopArg);
        }

        private void FireOnClear(IEnumerable<T> items)
        {
            if (OnClear == null)
            {
                return;
            }

            _onClearArg.Value = items;
            OnClear(_sender, _onClearArg);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}