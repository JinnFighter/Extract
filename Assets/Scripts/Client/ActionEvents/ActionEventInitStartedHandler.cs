using Client.States;
using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public class ActionEventInitStartedHandler : BaseActionEventHandler<ActionEventInitStarted>
    {
        public override EActionEventType RequestedType => EActionEventType.InitStart;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventInitStarted gameEvent)
        {
            instance.StateMachine.ChangeState(EBattleStateId.Init);
        }
    }
}