using Client.States;
using Common;
using Logic.ActionEvents;
using UnityEngine;

namespace Client.ActionEvents
{
    public class ActionEventActivePlayerChangedHandler : BaseActionEventHandler<ActionEventPlayerChanged>
    {
        public override EActionEventType RequestedType => EActionEventType.PlayerTurn;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventPlayerChanged gameEvent)
        {
            if (instance.ModelClient.WinnerId != -1)
            {
                return;
            }
            
            var id = gameEvent.NewPlayerId;
            Debug.Log($"Player {id} is turned");
            instance.ModelClient.SetCurrentPlayer(id);
            instance.StateMachine.ChangeState(gameEvent.NetId == AutoResolver.Resolve<UserDataService>().LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }
    }
}