using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic
{
    public interface IActionEventSender
    {
        void SendActionEvent(ActionEvent actionEvent);
        void SendOption(ActionRequestOption option);
    }
}