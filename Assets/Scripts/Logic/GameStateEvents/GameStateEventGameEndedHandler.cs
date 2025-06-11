using Logic.States;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventGameEndedHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, GameStateEvent gameEvent)
        {
            if (gameEvent.GetEventType() != EGameStateEventType.GameEnd) return;

            var id = (gameEvent as GameStateEventGameEnded).WinnerId;
            Debug.Log($"Game over event, winner is {id}");
            instance.Model.SetWinner(id);
            instance.StateMachine.ChangeState(EBattleStateId.GameOver);
        }
    }
}