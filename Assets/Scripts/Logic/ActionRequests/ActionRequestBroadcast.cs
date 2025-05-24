using FishNet.Broadcast;

namespace Logic.ActionRequests
{
    public struct ActionRequestBroadcast : IBroadcast
    {
        public IActionRequest ActionRequest { get; set; }
    }
}