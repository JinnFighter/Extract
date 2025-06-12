using FishNet.Broadcast;

namespace Logic.ActionRequests
{
    public struct OptionBroadcast : IBroadcast
    {
        public ActionRequestOption Option { get; set; }
    }
}