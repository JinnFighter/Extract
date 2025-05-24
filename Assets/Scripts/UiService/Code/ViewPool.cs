using System;
using System.Collections.Generic;
using System.Linq;
using UiService.Code.Widgets;
using UnityEngine;

namespace UiService.Code
{
    public class ViewPool : MonoBehaviour, IViewPool
    {
        [SerializeField] private Transform _poolStash;
        [SerializeField] private UiViewContainer _viewContainer;
        private readonly Dictionary<Type, Queue<UiView>> _pooledObjects = new();

        public void Init()
        {
            _viewContainer.Init();
        }

        public void Terminate()
        {
            foreach (var view in _pooledObjects.Keys.SelectMany(type => _pooledObjects[type]))
                Destroy(view.gameObject);

            _pooledObjects.Clear();
        }

        public T TakeItem<T>() where T : UiView
        {
            T result;
            var type = typeof(T);
            if (_pooledObjects.ContainsKey(type) && _pooledObjects[type].Count > 0)
            {
                result = (T)_pooledObjects[type].Dequeue();
                result.transform.SetParent(null);
            }
            else
            {
                result = Instantiate(_viewContainer.GetView<T>());
            }

            return result;
        }

        public UiView TakeItem(Type type)
        {
            UiView result;
            if (_pooledObjects.ContainsKey(type) && _pooledObjects[type].Count > 0)
                result = _pooledObjects[type].Dequeue();
            else
                result = Instantiate(_viewContainer.GetView(type) as UiView);

            return result;
        }

        public void Release(UiView item)
        {
            var type = item.GetType();
            if (!_pooledObjects.ContainsKey(type)) _pooledObjects[type] = new Queue<UiView>();

            _pooledObjects[type].Enqueue(item);
            item.transform.SetParent(_poolStash, false);
        }
    }
}