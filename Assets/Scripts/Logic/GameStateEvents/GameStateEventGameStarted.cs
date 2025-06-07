using FishNet.Broadcast;

namespace Logic.GameStateEvents
{
    public struct GameStateEventGameStarted : IGameStateEvent, IBroadcast
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public EGameStateEventType EventType => EGameStateEventType.GameStart;
    }
}