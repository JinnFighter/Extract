namespace Logic.ActionRequests
{
    public class ActionRequestEndTurn : ActionRequest
    {
        public override EActionRequestType ActionRequestType => EActionRequestType.EndTurn;
        public override bool IsValid(LogicModel logicModel)
        {
            return logicModel.GameEntity.CurrentPlayerId == CasterId;
        }
    }
}