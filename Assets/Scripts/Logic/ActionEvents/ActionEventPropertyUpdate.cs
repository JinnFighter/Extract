namespace Logic.ActionEvents
{
    public class ActionEventPropertyUpdate : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.PropertyUpdate;
        public EPropertyType PropertyType { get; set; }
        public int Value { get; set; }
    }
}