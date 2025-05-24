using System;
using UiService.Code.Widgets;

namespace UiService.Code
{
    public interface IViewPool
    {
        void Init();
        void Terminate();
        T TakeItem<T>() where T : UiView;
        UiView TakeItem(Type type);
        void Release(UiView item);
    }
}