using MVVM;

namespace UiService.Code.Widgets
{
    public interface IStateRouter
    {
        void Init(IUiService uiService, IModel parentModel);
        void Terminate();
        void SwitchState<TState>() where TState : IUiScreenState;
        void PushState<TState>() where TState : IUiScreenState;
        void Pop();
        void RegisterState<TState>(IModel model, UiView view) where TState : IUiScreenState;
    }
}