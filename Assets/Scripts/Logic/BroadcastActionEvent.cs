using FishNet.Broadcast;
using Logic.ActionEvents;

namespace Logic
{
    public struct BroadcastActionEvent : IBroadcast
    {
        public ActionEvent ActionEvent { get; set; }
    }
}