using Logic.ActionEvents;
using Logic.States;

namespace Client.GameStateEvents
{
    public class ActionEventGameStartedHandler : IActionEventHandler
    {
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != EActionEventType.GameStart) return;
            
            instance.StateMachine.ChangeState(EBattleStateId.Init);
        }
    }
}