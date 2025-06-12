using Common;
using Logic.States;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventActivePlayerChangedHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, GameStateEvent gameEvent)
        {
            if (gameEvent.EventType != EGameStateEventType.PlayerTurn) return;
            if (instance.Model.WinnerId != -1)
            {
                return;
            }

            var gameEventPlayerChanged = gameEvent as GameStateEventPlayerChanged;
            var id = gameEventPlayerChanged.NewPlayerId;
            Debug.Log($"Player {id} is turned");
            instance.Model.SetCurrentPlayer(id);
            instance.StateMachine.ChangeState(gameEventPlayerChanged.NetId == AutoResolver.Resolve<UserDataService>().LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }
    }
}