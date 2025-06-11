using FishNet.Broadcast;

namespace Logic.ActionRequests
{
    public struct ActionRequestBroadcast : IBroadcast
    {
        public ActionRequest ActionRequest { get; set; }
    }
}