using Logic.GameStateEvents;

namespace Logic
{
    public interface IGameEventSender
    {
        void SendGameEvent(GameStateEvent gameStateEvent);
    }
}