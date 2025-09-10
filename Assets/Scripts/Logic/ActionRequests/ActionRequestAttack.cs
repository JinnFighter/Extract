namespace Logic.ActionRequests
{
    public class ActionRequestAttack : ActionRequest
    {
        public int TargetId { get; set; }
        public override EActionRequestType ActionRequestType => EActionRequestType.Attack;
        public override bool IsValid(LogicModelServer logicModelServer)
        {
            if (!logicModelServer.UnitEntities.TryGetValue(CasterId, out _)) return false;
            if (!logicModelServer.UnitEntities.TryGetValue(TargetId, out _)) return false;
            return true;
        }
    }
}