namespace Logic.GameStateEvents
{
    public interface IGameStateEvent
    {
        int EventId { get; set; }
        int TurnNumber { get; set; }
    }
}