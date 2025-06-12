namespace Logic.ActionRequests
{
    public class ActionRequestEndTurn : ActionRequest
    {
        public override EActionRequestType ActionRequestType => EActionRequestType.EndTurn;
        public override bool IsValid(LogicModelServer logicModelServer)
        {
            return logicModelServer.GameEntityServer.CurrentPlayerId == CasterId;
        }
    }
}