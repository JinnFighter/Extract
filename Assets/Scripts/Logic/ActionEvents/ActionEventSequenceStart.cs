namespace Logic.ActionEvents
{
    public class ActionEventSequenceStart : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.SequenceStart;
        public EActionSequenceType SequenceType { get; set; }
    }
}