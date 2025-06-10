namespace Logic.GameStateEvents
{
    public class GameStateEventGameStarted : GameStateEvent
    {
        public override EGameStateEventType GetEventType()
        {
            return EGameStateEventType.GameStart;
        }
    }
}