namespace Logic.GameStateEvents
{
    public class GameStateEventPlayerChanged : GameStateEvent
    {
        public override EGameStateEventType GetEventType() => EGameStateEventType.PlayerTurn;
        public int NewPlayerId { get; set; }
    }
}