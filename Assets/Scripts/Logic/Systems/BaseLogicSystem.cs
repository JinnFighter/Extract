using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public abstract class BaseLogicSystem : ILogicSystem
    {
        private readonly List<GameStateEvent> _collectedEvents = new();
        public IEnumerator<List<GameStateEvent>> RunLogic(ActionRequest rootRequest, LogicModel model)
        {
            _collectedEvents.Clear();
            var innerLogic = RunLogicInner(rootRequest, model);
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

        protected abstract IEnumerator<GameStateEvent> RunLogicInner(ActionRequest rootRequest, LogicModel model);
    }
}