namespace Logic.ActionEvents
{
    public class ActionEventPlayerChanged : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.PlayerTurn;
        public int NewPlayerId { get; set; }
        public int NetId { get; set; }
    }
}