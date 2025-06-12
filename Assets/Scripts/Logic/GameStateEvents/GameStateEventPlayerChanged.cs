namespace Logic.GameStateEvents
{
    public class GameStateEventPlayerChanged : GameStateEvent
    {
        public override EGameStateEventType EventType => EGameStateEventType.PlayerTurn;
        public int NewPlayerId { get; set; }
        public int NetId { get; set; }
    }
}