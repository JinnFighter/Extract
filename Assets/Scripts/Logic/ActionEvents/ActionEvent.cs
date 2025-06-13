namespace Logic.ActionEvents
{
    public abstract class ActionEvent
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public abstract EActionEventType EventType { get; }
        public bool IsInitEvent { get; set; }
    }
}