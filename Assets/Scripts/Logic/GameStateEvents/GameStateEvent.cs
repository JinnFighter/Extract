namespace Logic.GameStateEvents
{
    public abstract class GameStateEvent
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public abstract EGameStateEventType EventType { get; }
    }
}