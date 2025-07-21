using Common;
using FishNet;
using Logic.ActionEvents;
using Logic.ActionRequests;
using VContainer;

namespace Logic
{
    public class ActionEventSender : IActionEventSender
    {
        [Inject] private NetworkService _networkService;
        
        public void SendActionEvent(ActionEvent actionEvent)
        {
            if (!InstanceFinder.IsServerStarted) return;

            _networkService.SendServerBroadcast(new BroadcastActionEvent
            {
                ActionEvent = actionEvent
            });
        }

        public void SendOption(ActionRequestOption option)
        {
            if (!InstanceFinder.IsServerStarted) return;

            _networkService.SendServerBroadcast(new BroadcastOption
            {
                Option = option
            });
        }
    }
}