using Logic.ActionEvents;

namespace Client.GameStateEvents
{
    public interface IActionEventHandler
    {
        void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent);
    }
}