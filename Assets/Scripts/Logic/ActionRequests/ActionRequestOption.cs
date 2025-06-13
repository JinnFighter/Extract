using Logic.ActionEvents;

namespace Logic.ActionRequests
{
    public class ActionRequestOption
    {
        public EActionRequestType RequestType { get; set; }
        public int EntityId { get; set; }
        public EEntityType EntityType { get; set; }
        public string NameId { get; set; }
        public bool IsAdd { get; set; }
    }
}