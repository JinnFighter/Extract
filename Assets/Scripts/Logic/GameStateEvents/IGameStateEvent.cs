using FishNet.Broadcast;

namespace Logic.GameStateEvents
{
    public interface IGameStateEvent
    {
        int EventId { get; set; }
        int TurnNumber { get; set; }
        EGameStateEventType EventType { get; }
    }
}
