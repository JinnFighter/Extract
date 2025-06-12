using FishNet.Broadcast;

namespace Logic.ActionRequests
{
    public struct BroadcastOption : IBroadcast
    {
        public ActionRequestOption Option { get; set; }
    }
}