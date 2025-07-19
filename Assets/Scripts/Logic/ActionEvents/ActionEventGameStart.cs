namespace Logic.ActionEvents
{
    public class ActionEventGameStart : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.GameStart;
        public int NewPlayerId { get; set; }
        public int NetId { get; set; }
    }
}