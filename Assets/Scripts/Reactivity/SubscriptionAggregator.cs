using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Reactivity
{
    public class SubscriptionAggregator
    {
        private readonly Dictionary<object, List<(Delegate OriginalHandler, Action UnsubscribeHandler)>>
            _subscriptions = new();

        private void CheckParameters<T, T1>(T caller, T1 handler)
        {
            if (caller == null)
                throw new Exception("Attempted to pass null object for event caller");
            if (handler == null)
                throw new Exception("Attempted to pass null handler for event");
        }

        private void AddSubscription(object notifyContainer, Delegate handler, Action unsubscribeAction)
        {
            if (!_subscriptions.ContainsKey(notifyContainer))
                _subscriptions[notifyContainer] = new List<(Delegate, Action)>();

            _subscriptions[notifyContainer].Add((handler, unsubscribeAction));
        }

        private void RemoveSubscription(object caller, Delegate handler)
        {
            if (!_subscriptions.TryGetValue(caller, out var propertySubscriptions)) return;
            for (var i = propertySubscriptions.Count - 1; i > -1; i--)
            {
                var currentSubscription = propertySubscriptions[i];

                if (currentSubscription.OriginalHandler != handler) continue;

                currentSubscription.UnsubscribeHandler();
                propertySubscriptions.RemoveAt(i);
            }
        }
        
        public void ListenEvent(UnityEvent caller, UnityAction handler, bool invokeImmediately = false)
        {
            CheckParameters(caller, handler);

            caller.AddListener(handler);
            AddSubscription(caller, handler, () => caller.RemoveListener(handler));
            if (invokeImmediately) handler.Invoke();
        }

        public void ListenEvent<TPropertyType>(UnityEvent<TPropertyType> caller, UnityAction<TPropertyType> handler,
            TPropertyType defaultValue = default, bool invokeImmediately = false)
        {
            CheckParameters(caller, handler);

            caller.AddListener(handler);
            AddSubscription(caller, handler, () => caller.RemoveListener(handler));
            if (invokeImmediately) handler.Invoke(defaultValue);
        }

        public void ListenEvent<TPropertyType, TPropertyType2>(UnityEvent<TPropertyType, TPropertyType2> caller,
            UnityAction<TPropertyType, TPropertyType2> handler, TPropertyType defaultValue = default,
            TPropertyType2 defaultValue2 = default, bool invokeImmediately = false)
        {
            CheckParameters(caller, handler);

            caller.AddListener(handler);
            AddSubscription(caller, handler, () => caller.RemoveListener(handler));
            if (invokeImmediately) handler.Invoke(defaultValue, defaultValue2);
        }


        public void ListenEvent<TPropertyType>(IReactiveProperty<TPropertyType> caller,
            EventHandler<GenericEventArg<TPropertyType>> handler,
            bool invokeImmediately = false)
        {
            CheckParameters(caller, handler);

            caller.OnValueChanged += handler;
            AddSubscription(caller, handler, () => caller.OnValueChanged -= handler);
            if (invokeImmediately) handler.Invoke(caller, new GenericEventArg<TPropertyType>(caller.Value));
        }


        public void UnlistenEvent(UnityEvent caller, UnityAction handler)
        {
            RemoveSubscription(caller, handler);
        }

        public void ListenListAddRange<TType>
        (
            ReactiveList<TType> list,
            EventHandler<GenericEventArg<IEnumerable<TType>>> handler,
            IEnumerable<TType> defaultArgs = null
        )
        {
            list.OnAddRange += handler;
            Action unsubscriptionDelegate = () => list.OnAddRange -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
            if (defaultArgs != null) handler(list, new GenericEventArg<IEnumerable<TType>>(defaultArgs));
        }

        public void ListenListAddRange<TType>
        (
            IReactiveList<TType> list,
            EventHandler<GenericEventArg<IEnumerable<TType>>> handler,
            IEnumerable<TType> defaultArgs = null
        )
        {
            list.OnAddRange += handler;
            Action unsubscriptionDelegate = () => list.OnAddRange -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
            if (defaultArgs != null) handler(list, new GenericEventArg<IEnumerable<TType>>(defaultArgs));
        }

        public void ListenListAddElement<TType>
        (
            IReactiveList<TType> list,
            EventHandler<GenericPairEventArgs<int, TType>> handler,
            bool invokeAfterSubscribe = false
        )
        {
            ListenListAddElement((ReactiveList<TType>)list, handler, invokeAfterSubscribe);
        }

        public void ListenStackPush<TType>(IReactiveStack<TType> stack, EventHandler<GenericEventArg<TType>> handler,
            bool invokeAfterSubscribe = false)
        {
            stack.OnPushItem += handler;
            Action unsubscriptionDelegate = () => stack.OnPushItem -= handler;
            AddSubscription(stack, handler, unsubscriptionDelegate);
            if (invokeAfterSubscribe) handler(stack, new GenericEventArg<TType>(stack.Peek()));
        }
        
        public void ListenStackPop<TType>(IReactiveStack<TType> stack, EventHandler<GenericEventArg<TType>> handler)
        {
            stack.OnPopItem += handler;
            Action unsubscriptionDelegate = () => stack.OnPopItem -= handler;
            AddSubscription(stack, handler, unsubscriptionDelegate);
        }
        
        public void ListenStackClear<TType>(IReactiveStack<TType> stack, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
        {
            stack.OnClear += handler;
            Action unsubscriptionDelegate = () => stack.OnClear -= handler;
            AddSubscription(stack, handler, unsubscriptionDelegate);
        }

        public void ListenListAddElement<TType>
        (
            ReactiveList<TType> list,
            EventHandler<GenericPairEventArgs<int, TType>> handler,
            bool invokeAfterSubscribe = false
        )
        {
            list.OnAddItem += handler;
            Action unsubscriptionDelegate = () => list.OnAddItem -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);

            if (invokeAfterSubscribe)
            {
                var args = new GenericPairEventArgs<int, TType>();

                for (var i = 0; i < list.Count; i++)
                {
                    args.Key = i;
                    args.Value = list[i];
                    handler(list, args);
                }
            }
        }

        public void ListenListElementChange<TType>
            (ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            list.OnElementChange += handler;
            Action unsubscriptionDelegate = () => list.OnElementChange -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void ListenListElementChange<TType>
            (IReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            ListenListElementChange(list as ReactiveList<TType>, handler);
        }

        public void ListenListRemoveItem<TType>
            (ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            list.OnRemoveItem += handler;
            Action unsubscriptionDelegate = () => list.OnRemoveItem -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void ListenListRemoveItem<TType>
            (IReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            list.OnRemoveItem += handler;
            Action unsubscriptionDelegate = () => list.OnRemoveItem -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void ListenListRemoveRange<TType>
            (ReactiveList<TType> list, EventHandler<NotifierListRemoveRangeArgs<TType>> handler)
        {
            list.OnRemoveRange += handler;
            Action unsubscriptionDelegate = () => list.OnRemoveRange -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void UnlistenListAddRange<TType>
            (ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void UnlistenListAddElement<TType>
            (ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void UnlistenListElementChange<TType>
            (ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void UnlistenListRemoveItem<TType>
            (ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void UnlistenListRemoveRange<TType>
            (ReactiveList<TType> list, EventHandler<NotifierListRemoveRangeArgs<TType>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void UnlistenEvent<TType>
        (
            UnityEvent<TType> eventObject,
            UnityAction<TType> handler
        )
        {
            RemoveSubscription(eventObject, handler);
        }

        public void UnlistenEvent<TType, TType2>
        (
            UnityEvent<TType, TType2> eventObject,
            UnityAction<TType, TType2> handler
        )
        {
            RemoveSubscription(eventObject, handler);
        }

        public void UnlistenEvent<TType, TType2, TType3>
        (
            UnityEvent<TType, TType2, TType3> eventObject,
            UnityAction<TType, TType2, TType3> handler
        )
        {
            RemoveSubscription(eventObject, handler);
        }


        public void ListenDictionaryAddItem<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
            bool invokeAfterSubscribe = false
        )
        {
            dictionary.OnAddItem += handler;
            Action unsubscriptionDelegate = () => dictionary.OnAddItem -= handler;
            AddSubscription(dictionary, handler, unsubscriptionDelegate);

            if (invokeAfterSubscribe)
            {
                var args = new GenericPairEventArgs<TKey, TValue>();

                foreach (var value in dictionary)
                {
                    args.Key = value.Key;
                    args.Value = value.Value;
                    handler(dictionary, args);
                }
            }
        }

        public void ListenDictionaryRemoveItem<TKey, TValue>
            (ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
        {
            dictionary.OnRemoveItem += handler;
            Action unsubscriptionDelegate = () => dictionary.OnRemoveItem -= handler;
            AddSubscription(dictionary, handler, unsubscriptionDelegate);
        }

        public void ListenDictionaryElementChange<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
            bool invokeAfterSubscribe = false
        )
        {
            dictionary.OnElementChange += handler;
            Action unsubscriptionDelegate = () => dictionary.OnElementChange -= handler;
            AddSubscription(dictionary, handler, unsubscriptionDelegate);

            if (invokeAfterSubscribe)
            {
                var args = new GenericPairEventArgs<TKey, TValue>();

                foreach (var value in dictionary)
                {
                    args.Key = value.Key;
                    args.Value = value.Value;
                    handler(dictionary, args);
                }
            }
        }

        public void ListenDictionaryAddRange<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler,
            IDictionary<TKey, TValue> defaultValue = null
        )
        {
            dictionary.OnAddRange += handler;
            Action unsubscriptionDelegate = () => dictionary.OnAddRange -= handler;
            AddSubscription(dictionary, handler, unsubscriptionDelegate);
            if (defaultValue != null) handler(dictionary, new GenericEventArg<IDictionary<TKey, TValue>>(defaultValue));
        }

        public void ListenDictionaryClear<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
        )
        {
            dictionary.OnClear += handler;
            Action unsubscriptionDelegate = () => dictionary.OnClear -= handler;
            AddSubscription(dictionary, handler, unsubscriptionDelegate);
        }

        public void UnlistenDictionaryAddItem<TKey, TValue>
            (ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
        {
            RemoveSubscription(dictionary, handler);
        }

        public void UnlistenDictionaryRemoveItem<TKey, TValue>
            (ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
        {
            RemoveSubscription(dictionary, handler);
        }

        public void UnlistenDictionaryElementChange<TKey, TValue>
            (ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
        {
            RemoveSubscription(dictionary, handler);
        }

        public void UnlistenDictionaryAddRange<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
        )
        {
            RemoveSubscription(dictionary, handler);
        }

        public void UnlistenDictionaryClear<TKey, TValue>
        (
            ReactiveDictionary<TKey, TValue> dictionary,
            EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
        )
        {
            RemoveSubscription(dictionary, handler);
        }

        public void ListenListClear<TType>
            (ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
        {
            list.OnClear += handler;
            Action unsubscriptionDelegate = () => list.OnClear -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void ListenListClear<TType>
            (IReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
        {
            list.OnClear += handler;
            Action unsubscriptionDelegate = () => list.OnClear -= handler;
            AddSubscription(list, handler, unsubscriptionDelegate);
        }

        public void ListenListSort<TType>
            (ReactiveList<TType> list, EventHandler<ReactiveListSortingArgs<TType>> handler)
        {
            list.OnSort += handler;
            Action unsubscribeDelegate = () => list.OnSort -= handler;
            AddSubscription(list, handler, unsubscribeDelegate);
        }

        public void ListenInputActionPerformed(InputAction inputAction, Action<InputAction.CallbackContext> handler)
        {
            inputAction.performed += handler;
            Action unsubscriptionDelegate = () => inputAction.performed -= handler;
            AddSubscription(inputAction, handler, unsubscriptionDelegate);
        }

        public void UnlistenListClear<TType>
            (ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
        {
            RemoveSubscription(list, handler);
        }

        public void Unsubscribe()
        {
            foreach (var subs in _subscriptions.SelectMany(kvp => kvp.Value)) subs.UnsubscribeHandler.Invoke();

            _subscriptions.Clear();
        }
    }
}