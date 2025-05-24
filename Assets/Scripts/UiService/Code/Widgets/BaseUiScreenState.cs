using System;
using MVVM;
using UnityEngine;

namespace UiService.Code.Widgets
{
    public abstract class BaseUiScreenState<TModel, TView> : BaseUiWidget<TModel, TView>, IUiScreenState
        where TModel : IModel where TView : UiView
    {
        protected IStateRouter StateRouter;

        public void SetRouter(IStateRouter stateRouter)
        {
            StateRouter = stateRouter;
        }

        protected TWidget OpenEmbedded<TWidget>(IModel model, Type viewType, Transform transformParent)
            where TWidget : IUiWidget
        {
            return UiService.OpenEmbedded<TWidget>(model, viewType, Model, transformParent);
        }

        protected TWidget OpenEmbedded<TWidget>(IModel model, UiView view) where TWidget : IUiWidget
        {
            return UiService.OpenEmbedded<TWidget>(model, view, Model);
        }
    }
}