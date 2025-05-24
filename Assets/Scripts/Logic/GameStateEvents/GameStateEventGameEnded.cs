using FishNet.Broadcast;

namespace Logic.GameStateEvents
{
    public struct GameStateEventGameEnded : IGameStateEvent, IBroadcast
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public int WinnerId { get; }
    }
}