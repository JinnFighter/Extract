using Client.States;
using Common;
using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public class ActionEventGameStartHandler : BaseActionEventHandler<ActionEventGameStart>
    {
        public override EActionEventType RequestedType => EActionEventType.GameStart;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventGameStart gameEvent)
        {
            instance.ModelClient.SetCurrentPlayer(gameEvent.NewPlayerId);
            instance.StateMachine.ChangeState(gameEvent.NetId == AutoResolver.Resolve<UserDataService>().LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }
    }
}