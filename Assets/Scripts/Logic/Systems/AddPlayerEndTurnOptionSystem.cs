using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class AddPlayerEndTurnOptionSystem : IOptionSystem
    {
        public void Run(LogicModel model, IGameEventSender gameEventSender)
        {
            if (model.GameEntity.IsGameOver)
            {
                return;
            }
            var option = new ActionRequestOption
            {
                EntityId = model.GameEntity.CurrentPlayerId,
                EntityType = EEntityType.Player,
                IsAdd = true,
                RequestType = EActionRequestType.EndTurn
            };
            model.PlayerEntities[model.GameEntity.CurrentPlayerId].CurrentOptions.Add(EActionRequestType.EndTurn, option);

            gameEventSender.SendOption(option);
        }
    }
}