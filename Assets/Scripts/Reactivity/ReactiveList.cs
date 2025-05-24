using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
    public class ReactiveList<T> : IList<T>, IReactiveList<T>
    {
        private readonly List<T> _list;
        private readonly GenericPairEventArgs<int, T> _onAddItemArgs;

        private readonly GenericEventArg<IEnumerable<T>> _onAddRangeArgs;
        private readonly GenericEventArg<IEnumerable<T>> _onClearArgs;
        private readonly GenericPairEventArgs<int, T> _onElementChangeArgs;
        private readonly GenericPairEventArgs<int, T> _onRemoveArgs;
        private readonly NotifierListRemoveRangeArgs<T> _onRemoveRangeArgs;
        private readonly ReactiveListSortingArgs<T> _onSortingArgs;
        private readonly object _sender;

        public ReactiveList() : this(0, null)
        {
        }

        public ReactiveList(object sender) : this(0, sender)
        {
        }

        public ReactiveList(int count) : this(count, null)
        {
        }

        public ReactiveList(int count, object sender)
        {
            _list = new List<T>(count);

            _onAddRangeArgs = new GenericEventArg<IEnumerable<T>>();
            _onClearArgs = new GenericEventArg<IEnumerable<T>>();
            _onAddItemArgs = new GenericPairEventArgs<int, T>();
            _onElementChangeArgs = new GenericPairEventArgs<int, T>();
            _onRemoveArgs = new GenericPairEventArgs<int, T>();
            _onRemoveRangeArgs = new NotifierListRemoveRangeArgs<T>();
            _onSortingArgs = new ReactiveListSortingArgs<T>();

            _sender = sender ?? this;
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                FireOnElementChange(value, index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Add(T item)
        {
            _list.Add(item);
            FireOnAddItem(item, _list.Count - 1);
        }


        public void Clear()
        {
            if (OnClear != null)
            {
                var clearedElements = new List<T>(_list.Count);
                clearedElements.AddRange(_list);
                _list.Clear();
                FireOnClear(clearedElements);
            }
            else
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var containsItem = _list.Contains(item);
            if (containsItem)
            {
                var index = _list.IndexOf(item);
                _list.Remove(item);
                FireOnRemoveItem(item, index);
            }

            return containsItem;
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            FireOnAddItem(item, index);
        }

        public void RemoveAt(int index)
        {
            var removedItem = _list[index];
            _list.RemoveAt(index);
            FireOnRemoveItem(removedItem, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event EventHandler<GenericPairEventArgs<int, T>> OnAddItem;
        public event EventHandler<GenericEventArg<IEnumerable<T>>> OnAddRange;
        public event EventHandler<GenericEventArg<IEnumerable<T>>> OnClear;
        public event EventHandler<GenericPairEventArgs<int, T>> OnElementChange;
        public event EventHandler<GenericPairEventArgs<int, T>> OnRemoveItem;
        public event EventHandler<ReactiveListSortingArgs<T>> OnSort;
        public event EventHandler<NotifierListRemoveRangeArgs<T>> OnRemoveRange;

        public void AddRange(List<T> collection)
        {
            _list.AddRange(collection);
            FireOnAddRange(collection);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            var enumerable = collection.ToList();
            _list.AddRange(enumerable);
            FireOnAddRange(enumerable);
        }


        public void AddRange(ReactiveList<T> collection)
        {
            _list.AddRange(collection);
            FireOnAddRange(collection._list);
        }

        public void RemoveRange(int index, int count)
        {
            var res = _list.GetRange(index, count);
            _list.RemoveRange(index, count);
            FireOnRemoveRange(index, count, res);
        }

        public void Sort()
        {
            var tmpList = _list.ToList();
            _list.Sort();
            FireOnSort(tmpList);
        }

        public void Sort(IComparer<T> comparer)
        {
            var tmpList = _list.ToList();
            _list.Sort(comparer);
            FireOnSort(tmpList);
        }

        public void Sort(Comparison<T> comparer)
        {
            var tmpList = _list.ToList();
            _list.Sort(comparer);
            FireOnSort(tmpList);
        }

        private void FireOnAddItem(T item, int index)
        {
            if (OnAddItem == null) return;
            
            _onAddItemArgs.Key = index;
            _onAddItemArgs.Value = item;
            OnAddItem(_sender, _onAddItemArgs);
        }

        private void FireOnRemoveItem(T item, int index)
        {
            if (OnRemoveItem == null) return;
            
            _onRemoveArgs.Key = index;
            _onRemoveArgs.Value = item;
            OnRemoveItem(_sender, _onRemoveArgs);
        }

        private void FireOnAddRange(IEnumerable<T> items)
        {
            if (OnAddRange == null) return;
            
            _onAddRangeArgs.Value = items;
            OnAddRange(_sender, _onAddRangeArgs);
        }

        private void FireOnRemoveRange(int index, int count, IEnumerable<T> items)
        {
            if (OnRemoveRange == null) return;
            
            _onRemoveRangeArgs.Index = index;
            _onRemoveRangeArgs.Count = count;
            _onRemoveRangeArgs.removedValues = items;
            OnRemoveRange(_sender, _onRemoveRangeArgs);
        }

        private void FireOnElementChange(T item, int index)
        {
            if (OnElementChange != null)
            {
                _onElementChangeArgs.Key = index;
                _onElementChangeArgs.Value = item;
                OnElementChange(_sender, _onElementChangeArgs);
            }
        }

        private void FireOnClear(IEnumerable<T> items)
        {
            if (OnClear == null) return;
            
            _onClearArgs.Value = items;
            OnClear(_sender, _onClearArgs);
        }

        private void FireOnSort(List<T> beforeSortList)
        {
            if (OnSort == null) return;

            for (var oldIndex = 0; oldIndex < beforeSortList.Count; oldIndex++)
            {
                var oldValue = beforeSortList[oldIndex];
                var newIndex = _list.IndexOf(oldValue);

                if (newIndex == oldIndex) continue;
                
                _onSortingArgs.OldIndex = oldIndex;
                _onSortingArgs.NewIndex = newIndex;
                _onSortingArgs.Value = oldValue;
                OnSort(this, _onSortingArgs);
            }
        }
    }
}