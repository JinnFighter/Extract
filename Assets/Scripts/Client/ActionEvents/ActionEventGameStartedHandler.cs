using Client.States;
using Logic.ActionEvents;

namespace Client.ActionEvents
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