namespace Logic.GameStateEvents
{
    public class GameStateEventGameEnded : GameStateEvent
    {
        public int WinnerId { get; set; }

        public override EGameStateEventType EventType => EGameStateEventType.GameEnd;
    }
}