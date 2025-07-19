using Client.States;
using Logic.ActionEvents;
using UnityEngine;

namespace Client.ActionEvents
{
    public class ActionEventGameEndedHandler : BaseActionEventHandler<ActionEventGameEnded>
    {
        public override EActionEventType RequestedType => EActionEventType.GameEnd;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventGameEnded gameEvent)
        {
            var id = gameEvent.WinnerId;
            Debug.Log($"Game over event, winner is {id}");
            instance.ModelClient.SetWinner(id);
            instance.StateMachine.ChangeState(EBattleStateId.GameOver);
        }
    }
}