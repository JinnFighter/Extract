using Logic.ActionEvents;
using Logic.States;
using UnityEngine;

namespace Client.GameStateEvents
{
    public class ActionEventGameEndedHandler : IActionEventHandler
    {
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != EActionEventType.GameEnd) return;

            var id = (gameEvent as ActionEventGameEnded).WinnerId;
            Debug.Log($"Game over event, winner is {id}");
            instance.ModelClient.SetWinner(id);
            instance.StateMachine.ChangeState(EBattleStateId.GameOver);
        }
    }
}