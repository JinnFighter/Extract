namespace Logic.GameStateEvents
{
    public abstract class GameStateEvent : IGameStateEvent
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public virtual EGameStateEventType GetEventType() => EGameStateEventType.None;
    }
}