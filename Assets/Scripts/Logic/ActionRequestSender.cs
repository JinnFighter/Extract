using Common;
using Logic.ActionRequests;
using VContainer;

namespace Logic
{
    public class ActionRequestSender : IActionRequestSender
    {
        [Inject] private NetworkService _networkService;
        
        public void SendActionRequest<T>(T actionRequest) where T : ActionRequest
        {
            _networkService.SendClientBroadcast(new ActionRequestBroadcast
            {
                ActionRequest = actionRequest
            });
        }
    }
}