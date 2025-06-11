using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class CheckGameOverSystem : BaseLogicSystem
    {
        private int _turnCount;

        protected override IEnumerator<GameStateEvent> RunLogicInner(ActionRequest rootRequest, LogicModel model)
        {
            if (_turnCount < 2)
            {
                _turnCount++;
                yield break;
            }

            model.GameEntity.IsGameOver = true;
            model.GameEntity.WinnerId = model.GameEntity.CurrentPlayerId;
            yield return new GameStateEventGameEnded
            {
                WinnerId = model.GameEntity.WinnerId
            };
        }
    }
}