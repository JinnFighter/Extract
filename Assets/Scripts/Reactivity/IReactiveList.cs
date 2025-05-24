using System;
using System.Collections.Generic;

namespace Reactivity
{
    public interface IReactiveList<T> : IEnumerable<T>
    {
        int Count { get; }
        T this[int index] { get; }

        int IndexOf(T item);

        event EventHandler<GenericPairEventArgs<int, T>> OnAddItem;
        event EventHandler<GenericEventArg<IEnumerable<T>>> OnAddRange;
        event EventHandler<GenericEventArg<IEnumerable<T>>> OnClear;
        event EventHandler<GenericPairEventArgs<int, T>> OnElementChange;
        event EventHandler<GenericPairEventArgs<int, T>> OnRemoveItem;
        event EventHandler<ReactiveListSortingArgs<T>> OnSort;
    }
}