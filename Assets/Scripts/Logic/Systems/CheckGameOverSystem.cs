using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class CheckGameOverSystem : BaseLogicSystem
    {
        private int _turnCount;

        protected override IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            if (_turnCount < 2)
            {
                _turnCount++;
                yield break;
            }

            modelServer.GameEntityServer.IsGameOver = true;
            modelServer.GameEntityServer.WinnerId = modelServer.GameEntityServer.CurrentPlayerId;
            yield return new ActionEventGameEnded
            {
                WinnerId = modelServer.GameEntityServer.WinnerId
            };
        }
    }
}