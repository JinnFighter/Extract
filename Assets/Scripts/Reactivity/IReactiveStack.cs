using System;
using System.Collections.Generic;

namespace Reactivity
{
    public interface IReactiveStack<T> : IEnumerable<T>
    {
        int Count { get; }

        event EventHandler<GenericEventArg<T>> OnPushItem;
        event EventHandler<GenericEventArg<IEnumerable<T>>> OnClear;
        event EventHandler<GenericEventArg<T>> OnPopItem;
        T Peek();
    }
}