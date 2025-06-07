using FishNet.Broadcast;
using Logic.GameStateEvents;

namespace Logic
{
    public interface IGameEventSender
    {
        void SendGameEvent<T>(T gameStateEvent) where T : struct, IGameStateEvent, IBroadcast;
    }
}
