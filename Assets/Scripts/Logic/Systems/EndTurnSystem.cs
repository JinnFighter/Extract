using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public class EndTurnSystem : BaseLogicSystem, IInitializeSystem
    {
        protected override IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            if (modelServer.GameEntityServer.IsGameOver)
            {
                yield break;
            }
                
            if (modelServer.GameEntityServer.CurrentPlayerId != rootRequest.CasterId)
            {
                yield break;
            }
                
            modelServer.GameEntityServer.CurrentPlayerIndex = modelServer.GameEntityServer.CurrentPlayerIndex + 1 >= modelServer.GameEntityServer.PlayerIds.Count
                ? 0
                : modelServer.GameEntityServer.CurrentPlayerIndex + 1;
            yield return new ActionEventPlayerChanged
            {
                NewPlayerId = modelServer.GameEntityServer.CurrentPlayerId,
                NetId = modelServer.PlayerEntities[modelServer.GameEntityServer.CurrentPlayerId].NetId
            };
        }

        public void Initialize(GameSetupInfo gameSetupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger)
        {
            actionEventLogger.LogGameEvent(new ActionEventPlayerChanged
            {
                NewPlayerId = logicModelServer.GameEntityServer.CurrentPlayerId
            });
        }
    }
}