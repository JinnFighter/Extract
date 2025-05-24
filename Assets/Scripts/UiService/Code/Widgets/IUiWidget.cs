using MVVM;

namespace UiService.Code.Widgets
{
    public interface IUiWidget : IController
    {
        bool IsEnabled { get; }
        void Setup(IModel model, IUiView uiView, IUiService uiService);
        void Enable();
        void Disable();
    }
}