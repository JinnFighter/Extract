namespace Logic.GameStateEvents
{
    public class ActionEventGameEnded : ActionEvent
    {
        public int WinnerId { get; set; }

        public override EActionEventType EventType => EActionEventType.GameEnd;
    }
}