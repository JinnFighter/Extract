using System.Collections.Generic;
using MVVM;
using UiService.Code.LayerHelpers;
using UiService.Code.Widgets;

namespace UiService.Code
{
    public class WidgetReference
    {
        private readonly IUiService _uiService;
        private readonly UiView _view;
        private readonly ViewPool _viewPool;

        public WidgetReference(IUiService service, IModel model, ViewPool viewPool, IUiWidget widget, UiView view,
            IModel parentModel, bool isPoolable, ILayerHelper layerHelper)
        {
            _uiService = service;
            Model = model;
            ParentModel = parentModel;
            _viewPool = viewPool;
            Widget = widget;
            _view = view;
            IsPoolable = isPoolable;
            LayerHelper = layerHelper;
        }

        public IModel Model { get; }
        public IModel ParentModel { get; }
        public IUiWidget Widget { get; }
        public List<WidgetReference> Children { get; } = new();
        public bool IsPoolable { get; }
        public ILayerHelper LayerHelper { get; }

        public void Open()
        {
            Widget.Setup(Model, _view, _uiService);
            Widget.Init();
            LayerHelper.Open();
        }

        public void Close()
        {
            Widget.Terminate();
            _view.gameObject.SetActive(false);
            if (IsPoolable) _viewPool.Release(_view);

            LayerHelper.Close();
        }
    }
}