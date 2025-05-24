using System;
using Cysharp.Threading.Tasks;
using MVVM;
using UiService.Code.Widgets;
using UnityEngine;

namespace UiService
{
    public interface IUiService
    {
        UniTask Init();
        UniTask Terminate();
        TWidget Open<TWidget>(IModel model, Type viewType) where TWidget : IUiWidget;

        TWidget OpenEmbedded<TWidget>(IModel model, Type viewType, IModel parentModel, Transform transformParent)
            where TWidget : IUiWidget;

        TWidget OpenEmbedded<TWidget>(IModel model, UiView view, IModel parentModel) where TWidget : IUiWidget;
        IUiWidget OpenEmbedded(Type type, IModel model, UiView view, IModel parentModel);
        bool IsOpen(IModel model);
        TWidget GetChild<TWidget>(IModel model) where TWidget : IUiWidget;
        void Close(IModel model);
    }
}