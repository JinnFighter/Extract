namespace Logic.ActionRequests
{
    public abstract class ActionRequest
    {
        public int CasterId { get; set; }
        public abstract EActionRequestType ActionRequestType { get; }
    }
}