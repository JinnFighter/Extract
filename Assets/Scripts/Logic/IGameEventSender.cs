using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic
{
    public interface IGameEventSender
    {
        void SendGameEvent(ActionEvent actionEvent);
        void SendOption(ActionRequestOption option);
    }
}