namespace Logic.GameStateEvents
{
    public class GameStateEventGameStarted : GameStateEvent
    {
        public override EGameStateEventType EventType => EGameStateEventType.GameStart;
    }
}