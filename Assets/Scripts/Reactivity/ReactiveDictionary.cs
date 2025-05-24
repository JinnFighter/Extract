using System;
using System.Collections;
using System.Collections.Generic;

namespace Reactivity
{
    public class ReactiveDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReactiveDictionary<TKey, TValue>
    {
        public delegate void ItemEventHandler(object sender, GenericPairEventArgs<TKey, TValue> itemArg);

        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly Dictionary<TKey, TValue> _lateAdditionalDictionary;
        private readonly List<TKey> _lateRemoveList;

        private readonly GenericPairEventArgs<TKey, TValue> _onAddItemArgs;
        private readonly GenericEventArg<IDictionary<TKey, TValue>> _onAddRangeArgs;
        private readonly GenericPairEventArgs<TKey, TValue> _onChangeItemArgs;
        private readonly GenericEventArg<IDictionary<TKey, TValue>> _onClearArgs;
        private readonly GenericPairEventArgs<TKey, TValue> _onRemoveArgs;

        private Dictionary<TKey, List<ItemEventHandler>> _onAddItemHandlers;
        private Dictionary<TKey, List<ItemEventHandler>> _onChangeItemHandlers;
        private Dictionary<TKey, List<ItemEventHandler>> _onRemoveItemHandlers;

        public ReactiveDictionary() : this(null)
        {
        }

        public ReactiveDictionary(object sender)
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _lateAdditionalDictionary = new Dictionary<TKey, TValue>();
            _lateRemoveList = new List<TKey>();

            _onAddItemArgs = new GenericPairEventArgs<TKey, TValue>();
            _onAddRangeArgs = new GenericEventArg<IDictionary<TKey, TValue>>();
            _onClearArgs = new GenericEventArg<IDictionary<TKey, TValue>>();
            _onChangeItemArgs = new GenericPairEventArgs<TKey, TValue>();
            _onRemoveArgs = new GenericPairEventArgs<TKey, TValue>();

            _sender = sender ?? this;
        }

        private object _sender { get; }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return _dictionary[key];
                }
                catch (Exception exception)
                {
                    throw new Exception($"Fail get value from {GetType().Name}. Key: {key}",
                        exception);
                }
            }
            set
            {
                if (_dictionary.ContainsKey(key))
                {
                    var currentValue = _dictionary[key];
                    if (!currentValue.Equals(value))
                    {
                        _dictionary[key] = value;
                        FireOnChangeItem(key, value);
                    }
                }
                else
                {
                    _dictionary[key] = value;
                    FireOnAddItem(key, value);
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
            FireOnAddItem(item.Key, item.Value);
        }

        public void Clear()
        {
            if (OnClear == null)
            {
                _dictionary.Clear();
            }
            else
            {
                var oldDictionary = new Dictionary<TKey, TValue>(_dictionary);
                _dictionary.Clear();
                FireOnClear(oldDictionary);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var containsKey = _dictionary.ContainsKey(item.Key);
            if (containsKey)
            {
                var removedValue = _dictionary[item.Key];
                _dictionary.Remove(item.Key);
                FireOnRemoveItem(item.Key, removedValue);
            }

            return containsKey;
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            FireOnAddItem(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            var containsKey = _dictionary.ContainsKey(key);
            if (containsKey)
            {
                var removedValue = _dictionary[key];
                _dictionary.Remove(key);
                FireOnRemoveItem(key, removedValue);
            }

            return containsKey;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            try
            {
                return _dictionary.TryGetValue(key, out value);
            }
            catch (Exception exception)
            {
                var keyInfo = key == null ? "NULL" : key.ToString();
                throw new Exception($"{GetType().Name} TryGetValue exception. Key: {keyInfo}",
                    exception);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void SubscribeOnRemoveItem(TKey key, ItemEventHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            if (_onRemoveItemHandlers == null) _onRemoveItemHandlers = new Dictionary<TKey, List<ItemEventHandler>>();

            if (!_onRemoveItemHandlers.ContainsKey(key)) _onRemoveItemHandlers.Add(key, new List<ItemEventHandler>());

            if (!_onRemoveItemHandlers[key].Contains(handler)) _onRemoveItemHandlers[key].Add(handler);
        }

        public void UnsubscribeOnRemoveItem(TKey key, ItemEventHandler handler)
        {
            if (_onRemoveItemHandlers != null && _onRemoveItemHandlers.ContainsKey(key))
                _onRemoveItemHandlers[key].Remove(handler);
        }

        public void SubscribeOnChangeItem(TKey key, ItemEventHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            if (_onChangeItemHandlers == null) _onChangeItemHandlers = new Dictionary<TKey, List<ItemEventHandler>>();

            if (!_onChangeItemHandlers.ContainsKey(key)) _onChangeItemHandlers.Add(key, new List<ItemEventHandler>());

            if (!_onChangeItemHandlers[key].Contains(handler)) _onChangeItemHandlers[key].Add(handler);
        }

        public void UnsubscribeOnChangeItem(TKey key, ItemEventHandler handler)
        {
            if (_onChangeItemHandlers != null && _onChangeItemHandlers.ContainsKey(key))
                _onChangeItemHandlers[key].Remove(handler);
        }

        public void SubscribeOnAddItem(TKey key, ItemEventHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            if (_onAddItemHandlers == null) _onAddItemHandlers = new Dictionary<TKey, List<ItemEventHandler>>();

            if (!_onAddItemHandlers.ContainsKey(key)) _onAddItemHandlers.Add(key, new List<ItemEventHandler>());

            if (!_onAddItemHandlers[key].Contains(handler)) _onAddItemHandlers[key].Add(handler);
        }

        public void UnsubscribeOnAddItem(TKey key, ItemEventHandler handler)
        {
            if (_onAddItemHandlers != null && _onAddItemHandlers.ContainsKey(key))
                _onAddItemHandlers[key].Remove(handler);
        }

        public event EventHandler<GenericPairEventArgs<TKey, TValue>> OnAddItem;
        public event EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> OnAddRange;
        public event EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> OnClear;
        public event EventHandler<GenericPairEventArgs<TKey, TValue>> OnElementChange;
        public event EventHandler<GenericPairEventArgs<TKey, TValue>> OnRemoveItem;

        /// <summary>
        ///     Если ключ не найден - вернет null вместо выброса исключения
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue GetSafe(TKey key)
        {
            TValue value;
            TryGetValue(key, out value);
            return value;
        }

        public void AddRange(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var value in dictionary) _dictionary.Add(value.Key, value.Value);

            if (OnAddRange == null) return;

            _onAddRangeArgs.Value = dictionary;
            OnAddRange(_sender, _onAddRangeArgs);
        }

        public void AddLate(TKey key, TValue value)
        {
            _lateAdditionalDictionary.Add(key, value);
        }

        public void RemoveLate(TKey key)
        {
            _lateRemoveList.Add(key);
        }

        public void Apply()
        {
            foreach (var value in _lateAdditionalDictionary) Add(value);

            foreach (var value in _lateRemoveList) Remove(value);

            _lateAdditionalDictionary.Clear();
            _lateRemoveList.Clear();
        }

        protected void FireOnChangeItem(TKey key, TValue value)
        {
            _onChangeItemArgs.Key = key;
            _onChangeItemArgs.Value = value;

            if (OnElementChange != null) OnElementChange(_sender, _onChangeItemArgs);

            if (_onChangeItemHandlers != null && _onChangeItemHandlers.ContainsKey(key))
                foreach (var handler in _onChangeItemHandlers[key])
                    handler(_sender, _onChangeItemArgs);
        }

        private void FireOnAddItem(TKey key, TValue value)
        {
            _onAddItemArgs.Key = key;
            _onAddItemArgs.Value = value;

            OnAddItem?.Invoke(_sender, _onAddItemArgs);

            if (_onAddItemHandlers == null || !_onAddItemHandlers.ContainsKey(key)) return;
            foreach (var handler in _onAddItemHandlers[key])
                handler(_sender, _onAddItemArgs);
        }

        private void FireOnRemoveItem(TKey key, TValue value)
        {
            _onRemoveArgs.Key = key;
            _onRemoveArgs.Value = value;

            OnRemoveItem?.Invoke(_sender, _onRemoveArgs);

            if (_onRemoveItemHandlers == null || !_onRemoveItemHandlers.ContainsKey(key)) return;

            foreach (var handler in _onRemoveItemHandlers[key])
                handler(_sender, _onRemoveArgs);
        }

        private void FireOnClear(IDictionary<TKey, TValue> elements)
        {
            if (OnClear == null) return;
            _onClearArgs.Value = elements;
            OnClear(_sender, _onClearArgs);
        }
    }
}