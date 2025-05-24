using System;
using System.Collections.Generic;
using UiService.Code.Widgets;
using UnityEngine;

namespace UiService
{
    [CreateAssetMenu(fileName = "UiViewContainer", menuName = "Containers/UiViewContainer")]
    public class UiViewContainer : ScriptableObject
    {
        [SerializeField] private List<UiView> uiViews;

        private readonly Dictionary<Type, IUiView> _views = new();

        public void Init()
        {
            foreach (var uiView in uiViews)
            {
                Register(uiView);
            }
        }

        public T GetView<T>() where T : IUiView
        {
            return (T)_views[typeof(T)];
        }

        public IUiView GetView(Type type)
        {
            return _views[type];
        }

        private void Register<TView>(TView view) where TView : UiView
        {
            _views[view.GetType()] = view;
        }
    }
}