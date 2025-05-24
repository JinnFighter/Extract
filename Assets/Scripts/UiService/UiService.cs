using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MVVM;
using UiService.Code;
using UiService.Code.LayerHelpers;
using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UiService
{
    public class UiService : MonoBehaviour, IUiService
    {
        [SerializeField] private Canvas _layerScreenCanvas;
        [SerializeField] private Canvas _layerDialogCanvas;
        [SerializeField] private Image _dialogBgFader;
        [SerializeField] private ViewPool _viewPool;

        private readonly Dictionary<IModel, WidgetReference> _widgets = new();
        private DialogLayerHelper _dialogLayerHelper;
        private ScreenLayerHelper _screenLayerHelper;
        private WidgetLayerHelper _widgetLayerHelper;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _screenLayerHelper = new ScreenLayerHelper();
            _dialogLayerHelper = new DialogLayerHelper(_dialogBgFader);
            _widgetLayerHelper = new WidgetLayerHelper();
        }

        public UniTask Init()
        {
            _viewPool.Init();
            _screenLayerHelper.Init();
            _dialogLayerHelper.Init();
            _widgetLayerHelper.Init();
            return UniTask.CompletedTask;
        }

        public UniTask Terminate()
        {
            _dialogLayerHelper.Terminate();
            _screenLayerHelper.Terminate();
            _widgetLayerHelper.Terminate();
            _viewPool.Terminate();
            return UniTask.CompletedTask;
        }

        public TWidget Open<TWidget>(IModel model, Type viewType) where TWidget : IUiWidget
        {
            var reference = OpenWidgetInner<TWidget>(model, viewType);
            return (TWidget)reference.Widget;
        }

        public TWidget OpenEmbedded<TWidget>(IModel model, Type viewType, IModel parentModel, Transform transformParent)
            where TWidget : IUiWidget
        {
            var reference = OpenEmbeddedWidgetInner<TWidget>(model, viewType, parentModel, transformParent);
            return (TWidget)reference.Widget;
        }

        public TWidget OpenEmbedded<TWidget>(IModel model, UiView view, IModel parentModel) where TWidget : IUiWidget
        {
            var reference = OpenEmbeddedWidgetInner<TWidget>(model, parentModel, view);
            return (TWidget)reference.Widget;
        }

        public IUiWidget OpenEmbedded(Type type, IModel model, UiView view, IModel parentModel)
        {
            var reference = OpenEmbeddedWidgetInner(type, model, parentModel, view);
            return reference.Widget;
        }

        public bool IsOpen(IModel model)
        {
            return _widgets.TryGetValue(model, out _);
        }

        public TWidget GetChild<TWidget>(IModel model) where TWidget : IUiWidget
        {
            var widget = (TWidget)_widgets[model].Widget;
            return widget;
        }

        public void Close(IModel model)
        {
            CloseWidgetInner(model);
        }

        private WidgetReference OpenWidgetInner<TWidget>(IModel model, Type viewType) where TWidget : IUiWidget
        {
            if (_widgets.ContainsKey(model))
                throw new Exception("Widget already open");

            var widget = Activator.CreateInstance<TWidget>();
            var layer = widget switch
            {
                IUiScreen => _layerScreenCanvas.transform,
                IUiDialog => _layerDialogCanvas.transform,
                _ => throw new Exception("Tried to Create embedded widget as not embedded")
            };
            ILayerHelper layerHelper = widget switch
            {
                IUiScreen => _screenLayerHelper,
                IUiDialog => _dialogLayerHelper,
                _ => throw new Exception("Unknown layer type")
            };
            var view = CreatePoolableView(viewType, layer);


            return CreateReference(model, widget, view, null, true, layerHelper);
        }

        private WidgetReference OpenEmbeddedWidgetInner<TWidget>(IModel model, Type viewType, IModel parentModel,
            Transform parent)
            where TWidget : IUiWidget
        {
            if (_widgets.ContainsKey(model))
                throw new Exception("Embedded widget already open");

            var widget = Activator.CreateInstance<TWidget>();
            var view = CreatePoolableView(viewType, parent);

            return CreateReference(model, widget, view, parentModel, true, _widgetLayerHelper);
        }

        private WidgetReference OpenEmbeddedWidgetInner<TWidget>(IModel model, IModel parentModel, UiView view)
            where TWidget : IUiWidget
        {
            if (_widgets.ContainsKey(model))
                throw new Exception("Embedded widget already open");

            var widget = Activator.CreateInstance<TWidget>();
            view.gameObject.SetActive(true);

            return CreateReference(model, widget, view, parentModel, false, _widgetLayerHelper);
        }

        private WidgetReference OpenEmbeddedWidgetInner(Type type, IModel model, IModel parentModel, UiView view)
        {
            if (_widgets.ContainsKey(model))
                throw new Exception("Embedded widget already open");

            var widget = Activator.CreateInstance(type) as IUiWidget;
            view.gameObject.SetActive(true);

            return CreateReference(model, widget, view, parentModel, false, _widgetLayerHelper);
        }

        private UiView CreatePoolableView(Type viewType, Transform parent)
        {
            var view = _viewPool.TakeItem(viewType);
            view.transform.SetParent(parent, false);
            view.gameObject.SetActive(true);
            return view;
        }

        private WidgetReference CreateReference<TWidget>(IModel model, TWidget widget, UiView view, IModel parentModel,
            bool isPoolable, ILayerHelper layerHelper) where TWidget : IUiWidget
        {
            var widgetReference = new WidgetReference(this, model, _viewPool, widget, view, parentModel, isPoolable,
                layerHelper);
            _widgets[model] = widgetReference;

            if (parentModel != null) _widgets[parentModel].Children.Add(widgetReference);
            widgetReference.Open();

            return widgetReference;
        }

        private void CloseWidgetInner(IModel model)
        {
            if (!_widgets.TryGetValue(model, out var widgetReference))
                return;

            var childrenQueue = new Queue<WidgetReference>(widgetReference.Children);

            while (childrenQueue.Count > 0) CloseWidgetInner(childrenQueue.Dequeue().Model);

            widgetReference.Close();
            _widgets.Remove(model);
            if (widgetReference.ParentModel != null)
                _widgets[widgetReference.ParentModel].Children.Remove(widgetReference);
        }
    }
}