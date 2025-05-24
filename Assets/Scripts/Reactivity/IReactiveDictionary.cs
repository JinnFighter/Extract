using System;
using System.Collections.Generic;

namespace Reactivity
{
    public interface IReactiveDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        TValue this[TKey key] { get; }

        int Count { get; }

        event EventHandler<GenericPairEventArgs<TKey, TValue>> OnAddItem;
        event EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> OnAddRange;
        event EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> OnClear;
        event EventHandler<GenericPairEventArgs<TKey, TValue>> OnElementChange;
        event EventHandler<GenericPairEventArgs<TKey, TValue>> OnRemoveItem;
        bool Contains(KeyValuePair<TKey, TValue> item);
        bool TryGetValue(TKey key, out TValue value);
        TValue GetSafe(TKey key);

        void SubscribeOnAddItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
        void UnsubscribeOnAddItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
        void SubscribeOnRemoveItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
        void UnsubscribeOnRemoveItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
        void SubscribeOnChangeItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
        void UnsubscribeOnChangeItem(TKey key, ReactiveDictionary<TKey, TValue>.ItemEventHandler handler);
    }
}