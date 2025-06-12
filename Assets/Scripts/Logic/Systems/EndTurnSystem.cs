using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class EndTurnSystem : BaseLogicSystem, IInitializeSystem
    {
        protected override IEnumerator<GameStateEvent> RunLogicInner(ActionRequest rootRequest, LogicModel model)
        {
            if (model.GameEntity.IsGameOver)
            {
                yield break;
            }
                
            if (model.GameEntity.CurrentPlayerId != rootRequest.CasterId)
            {
                yield break;
            }
                
            model.GameEntity.CurrentPlayerIndex = model.GameEntity.CurrentPlayerIndex + 1 >= model.GameEntity.PlayerIds.Count
                ? 0
                : model.GameEntity.CurrentPlayerIndex + 1;
            yield return new GameStateEventPlayerChanged
            {
                NewPlayerId = model.GameEntity.CurrentPlayerId,
                NetId = model.PlayerEntities[model.GameEntity.CurrentPlayerId].NetId
            };
        }

        public void Initialize(GameSetupInfo gameSetupInfo, LogicModel logicModel, GameEventLogger gameEventLogger)
        {
            gameEventLogger.LogGameEvent(new GameStateEventPlayerChanged
            {
                NewPlayerId = logicModel.GameEntity.CurrentPlayerId
            });
        }
    }
}