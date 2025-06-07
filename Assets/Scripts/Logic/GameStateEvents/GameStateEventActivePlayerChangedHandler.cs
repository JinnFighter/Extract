using Common;
using Logic.States;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventActivePlayerChangedHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, IGameStateEvent gameEvent)
        {
            if (gameEvent.EventType != EGameStateEventType.PlayerTurn) return;
            
            var id = (gameEvent is GameStateEventActivePlayerChanged @event ? @event : default).NewPlayerId;
            Debug.Log($"Player {id} is turned");
            instance.Model.SetCurrentPlayer(id);
            instance.StateMachine.ChangeState(id == AutoResolver.Resolve<UserDataService>().LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }
    }
}