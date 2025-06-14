using System;
using System.Collections.Generic;
using Client.Actions.ActionBuilders;
using Logic.ActionRequests;

namespace Client.Actions
{
    public class ActionRequestBuilderSystem : IActionRequestBuilderSystem
    {
        private readonly Dictionary<EActionRequestType, IActionRequestBuilder> _actionRequestBuilders = new()
        {
            { EActionRequestType.EndTurn , new ActionRequestBuilderEndTurn() },
            { EActionRequestType.Move , new ActionRequestBuilderMove() }
        };
        private IActionRequestBuilder _currentBuilder;
        public event Action OnReset;
        public event Action<ActionRequest> OnBuildComplete;
        public void Reset()
        {
            _currentBuilder = null;
            OnReset?.Invoke();
        }

        public IActionRequestBuilder StartBuild(EActionRequestType eActionRequestType)
        {
            _currentBuilder = _actionRequestBuilders[eActionRequestType];
            return _currentBuilder;
        }

        public ActionRequest Build()
        {
            if (_currentBuilder == null)
            {
                return null;
            }
            
            var action = _currentBuilder.BuildAction();
            OnBuildComplete?.Invoke(action);
            return action;
        }
    }
}