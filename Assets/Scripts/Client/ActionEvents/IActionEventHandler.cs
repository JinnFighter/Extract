using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public interface IActionEventHandler
    {
        void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent);
    }
}