using FishNet.Broadcast;

namespace Logic.GameStateEvents
{
    public struct GameStateEventActivePlayerChanged : IGameStateEvent, IBroadcast
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public EGameStateEventType EventType => EGameStateEventType.PlayerTurn;
        public int NewPlayerId;
    }
}