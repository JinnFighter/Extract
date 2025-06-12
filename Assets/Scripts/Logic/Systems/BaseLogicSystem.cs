using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public abstract class BaseLogicSystem : ILogicSystem
    {
        private readonly List<ActionEvent> _collectedEvents = new();
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

        protected abstract IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer);
    }
}