using System;
using MVVM;

namespace UiService.Code.Widgets
{
    public class BaseUiScreenWithStates<TModel, TView> : BaseUiWidget<TModel, TView>, IUiScreen
        where TModel : IModel where TView : UiView
    {
        protected readonly StateRouter StateRouter = new();
        private Type _initialStateType;

        protected override void InitInner()
        {
            StateRouter.Init(UiService, Model);
            RegisterStates();
            if (_initialStateType != null) StateRouter.PushState(_initialStateType);
            InitCommon();
        }

        protected override void TerminateInner()
        {
            TerminateCommon();
            StateRouter.CloseAll();
            StateRouter.Terminate();
        }

        protected virtual void RegisterStates()
        {
        }

        protected void RegisterState<TState>(IModel stateModel, UiView stateView, bool isInitial = false)
            where TState : IUiScreenState
        {
            StateRouter.RegisterState<TState>(stateModel, stateView);
            stateView.gameObject.SetActive(false);
            if (isInitial) _initialStateType = typeof(TState);
        }

        protected virtual void InitCommon()
        {
        }

        protected virtual void TerminateCommon()
        {
        }
    }
}