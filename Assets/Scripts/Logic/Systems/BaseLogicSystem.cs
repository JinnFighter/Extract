using System;
using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public abstract class BaseLogicSystem : ILogicSystem
    {
        private readonly List<ActionEvent> _collectedEvents = new();
        public bool IsCancelled { get; private set; }
        public event Action<ILogicSystem> OnPrepareLogicRunLaunched;
        public event Action<ILogicSystem> OnCancelLogicRunLaunched;
        public event Action<ILogicSystem> OnFinishLogicRunLaunched;

        public IEnumerator<List<ActionEvent>> RunPrepareLogic(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            IsCancelled = false;
            OnPrepareLogicRunLaunched?.Invoke(this);
            _collectedEvents.Clear();
            var innerLogic = RunPrepareLogicInner(rootRequest, modelServer);
            while (innerLogic.MoveNext())
            {
                var gameStateEvent = innerLogic.Current;
                if (gameStateEvent == null)
                {
                    continue;
                }
                _collectedEvents.Add(gameStateEvent);
            }

            yield return _collectedEvents;
        }

        public IEnumerator<List<ActionEvent>> RunLogic(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            _collectedEvents.Clear();
            var innerLogic = RunLogicInner(rootRequest, modelServer);
            while (innerLogic.MoveNext())
            {
                var gameStateEvent = innerLogic.Current;
                if (gameStateEvent == null)
                {
                    continue;
                }
                _collectedEvents.Add(gameStateEvent);
            }

            yield return _collectedEvents;
        }

        public IEnumerator<List<ActionEvent>> RunFinishLogic(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            OnFinishLogicRunLaunched?.Invoke(this);
            _collectedEvents.Clear();
            var innerLogic = RunFinishLogicInner(rootRequest, modelServer);
            while (innerLogic.MoveNext())
            {
                var gameStateEvent = innerLogic.Current;
                if (gameStateEvent == null)
                {
                    continue;
                }
                _collectedEvents.Add(gameStateEvent);
            }

            yield return _collectedEvents;
        }

        public IEnumerator<List<ActionEvent>> RunCancelLogic(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            IsCancelled = true;
            OnCancelLogicRunLaunched?.Invoke(this);
            _collectedEvents.Clear();
            var innerLogic = RunCancelLogicInner(rootRequest, modelServer);
            while (innerLogic.MoveNext())
            {
                var gameStateEvent = innerLogic.Current;
                if (gameStateEvent == null)
                {
                    continue;
                }
                _collectedEvents.Add(gameStateEvent);
            }

            yield return _collectedEvents;
        }

        protected abstract IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer);

        protected virtual IEnumerator<ActionEvent> RunPrepareLogicInner(ActionRequest rootRequest,
            LogicModelServer modelServer)
        {
            yield break;
        }
        
        protected virtual IEnumerator<ActionEvent> RunCancelLogicInner(ActionRequest rootRequest,
            LogicModelServer modelServer)
        {
            yield break;
        }
        
        protected virtual IEnumerator<ActionEvent> RunFinishLogicInner(ActionRequest rootRequest,
            LogicModelServer modelServer)
        {
            yield break;
        }
    }
}