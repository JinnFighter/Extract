using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public class AddPlayerEndTurnOptionSystem : IOptionSystem
    {
        public void Run(LogicModelServer modelServer, IActionEventSender actionEventSender)
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

            actionEventSender.SendOption(option);
        }
    }
}