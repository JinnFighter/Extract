using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic
{
    public interface IGameEventSender
    {
        void SendGameEvent(GameStateEvent gameStateEvent);
        void SendOption(ActionRequestOption option);
    }
}