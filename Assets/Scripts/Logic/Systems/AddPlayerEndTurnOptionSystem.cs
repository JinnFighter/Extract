using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class AddPlayerEndTurnOptionSystem : IOptionSystem
    {
        public void Run(LogicModelServer modelServer, IGameEventSender gameEventSender)
        {
            if (modelServer.GameEntityServer.IsGameOver)
            {
                return;
            }
            var option = new ActionRequestOption
            {
                EntityId = modelServer.GameEntityServer.CurrentPlayerId,
                EntityType = EEntityType.Player,
                IsAdd = true,
                RequestType = EActionRequestType.EndTurn
            };
            modelServer.PlayerEntities[modelServer.GameEntityServer.CurrentPlayerId].CurrentOptions.Add(EActionRequestType.EndTurn, option);

            gameEventSender.SendOption(option);
        }
    }
}