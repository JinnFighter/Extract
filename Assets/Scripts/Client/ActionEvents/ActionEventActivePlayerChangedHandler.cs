using Client.States;
using Common;
using Logic.ActionEvents;
using UnityEngine;

namespace Client.ActionEvents
{
    public class ActionEventActivePlayerChangedHandler : IActionEventHandler
    {
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != EActionEventType.PlayerTurn) return;
            if (instance.ModelClient.WinnerId != -1)
            {
                return;
            }

            var gameEventPlayerChanged = gameEvent as ActionEventPlayerChanged;
            var id = gameEventPlayerChanged.NewPlayerId;
            Debug.Log($"Player {id} is turned");
            instance.ModelClient.SetCurrentPlayer(id);
            instance.StateMachine.ChangeState(gameEventPlayerChanged.NetId == AutoResolver.Resolve<UserDataService>().LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }
    }
}