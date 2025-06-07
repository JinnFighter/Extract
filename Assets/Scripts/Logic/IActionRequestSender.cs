using Logic.ActionRequests;

namespace Logic
{
    public interface IActionRequestSender
    {
        void SendActionRequest<T>(T actionRequest) where T : IActionRequest;
    }
}