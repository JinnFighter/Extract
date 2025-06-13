using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic
{
    public interface IGameEventSender
    {
        void SendGameEvent(ActionEvent actionEvent);
        void SendOption(ActionRequestOption option);
    }
}