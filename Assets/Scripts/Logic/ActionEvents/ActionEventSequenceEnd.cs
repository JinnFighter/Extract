namespace Logic.ActionEvents
{
    public class ActionEventSequenceEnd : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.SequenceEnd;
        public EActionSequenceType SequenceType { get; set; }
    }
}