using System;
using System.Collections.Generic;
using MVVM;

namespace UiService.Code.Widgets
{
    public class StateRouter : IStateRouter
    {
        private readonly Dictionary<Type, StateData> _screenStates = new();
        private readonly Stack<StateData> _stateStack = new();
        private IModel _parentModel;
        private IUiService _uiService;

        public void Init(IUiService uiService, IModel parentModel)
        {
            _uiService = uiService;
            _parentModel = parentModel;
            _stateStack.Clear();
            _screenStates.Clear();
        }

        public void Terminate()
        {
            _stateStack.Clear();
            _screenStates.Clear();
        }

        public void SwitchState<TState>() where TState : IUiScreenState
        {
            if (_stateStack.Count > 0) ExitStateInner();

            EnterStateInner(typeof(TState));
        }

        public void PushState<TState>() where TState : IUiScreenState
        {
            if (_stateStack.Count > 0) DisableInner();

            EnterStateInner(typeof(TState));
        }

        public void Pop()
        {
            if (_stateStack.Count > 0)
            {
                ExitStateInner();

                if (_stateStack.Count > 0) EnableInner();
            }
        }

        public void RegisterState<TState>(IModel model, UiView view) where TState : IUiScreenState
        {
            _screenStates[typeof(TState)] = new StateData
            {
                Model = model,
                UiView = view
            };

            view.gameObject.SetActive(false);
        }

        public void PushState(Type type)
        {
            if (_stateStack.Count > 0) DisableInner();

            EnterStateInner(type);
        }

        public void CloseAll()
        {
            while (_stateStack.Count > 0) ExitStateInner();
        }

        private void EnterStateInner(Type type)
        {
            var nextState = _screenStates[type];
            _stateStack.Push(nextState);
            nextState.State =
                _uiService.OpenEmbedded(type, nextState.Model, nextState.UiView, _parentModel) as IUiScreenState;
            nextState.State.SetRouter(this);
        }

        private void ExitStateInner()
        {
            _uiService.Close(_stateStack.Pop().Model);
        }

        private void EnableInner()
        {
            _stateStack.Peek().State.Enable();
        }

        private void DisableInner()
        {
            _stateStack.Peek().State.Disable();
        }
    }

    internal class StateData
    {
        public IModel Model;
        public IUiScreenState State;
        public UiView UiView;
    }
}