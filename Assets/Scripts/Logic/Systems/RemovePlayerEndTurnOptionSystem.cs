using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class RemovePlayerEndTurnOptionSystem : IOptionSystem
    {
        #region IOptionSystem Members

        public void Run(LogicModelServer modelServer, IGameEventSender gameEventSender)
        {
            modelServer.PlayerEntities[modelServer.GameEntityServer.CurrentPlayerId].CurrentOptions.Remove(EActionRequestType.EndTurn);

            gameEventSender.SendOption(new ActionRequestOption
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