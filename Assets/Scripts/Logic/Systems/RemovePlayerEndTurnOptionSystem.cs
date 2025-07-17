using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public class RemovePlayerEndTurnOptionSystem : IOptionSystem
    {
        #region IOptionSystem Members

        public void Run(LogicModelServer modelServer, IActionEventSender actionEventSender)
        {
            modelServer.PlayerEntities[modelServer.GameEntityServer.CurrentPlayerId].CurrentOptions.Remove(EActionRequestType.EndTurn);

            actionEventSender.SendOption(new ActionRequestOption
            {
                EntityId = modelServer.GameEntityServer.CurrentPlayerId,
                EntityType = EEntityType.Player,
                IsAdd = false,
                RequestType = EActionRequestType.EndTurn
            });
        }

        #endregion
    }
}