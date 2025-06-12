using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class RemovePlayerEndTurnOptionSystem : IOptionSystem
    {
        #region IOptionSystem Members

        public void Run(LogicModel model, IGameEventSender gameEventSender)
        {
            model.PlayerEntities[model.GameEntity.CurrentPlayerId].CurrentOptions.Remove(EActionRequestType.EndTurn);

            gameEventSender.SendOption(new ActionRequestOption
            {
                EntityId = model.GameEntity.CurrentPlayerId,
                EntityType = EEntityType.Player,
                IsAdd = false,
                RequestType = EActionRequestType.EndTurn
            });
        }

        #endregion
    }
}