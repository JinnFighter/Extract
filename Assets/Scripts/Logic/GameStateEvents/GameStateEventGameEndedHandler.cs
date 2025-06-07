using Logic.States;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventGameEndedHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, IGameStateEvent gameEvent)
        {
            if (gameEvent.EventType != EGameStateEventType.GameEnd) return;

            var id = (gameEvent is GameStateEventGameEnded @event ? @event : default).WinnerId;
            Debug.Log($"Game over event, winner is {id}");
            instance.Model.SetWinner(id);
            instance.StateMachine.ChangeState(EBattleStateId.GameOver);
        }
    }
}