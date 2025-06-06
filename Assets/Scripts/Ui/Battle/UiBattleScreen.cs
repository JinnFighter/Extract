using Logic.ActionRequests;
using Logic.States;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class UiBattleScreen : BaseUiScreenWithStates<BattleScreenModel, BattleScreenView>
    {
        protected override void RegisterStates()
        {
            RegisterState<BattleScreenStateAlly>(Model.ModelAlly, View.ScreenStateViewAlly);
            RegisterState<BattleScreenStateEnemy>(Model.ModelEnemy, View.ScreenStateViewEnemy);
            RegisterState<BattleScreenStateGameOver>(Model.GameOverModel, View.ScreenStateViewGameOver);
        }

        protected override void InitCommon()
        {
            Model.BattleInstance.StateMachine.OnStateChanged += HandleStateChanged;
            View.ButtonEndTurn.onClick.AddListener(HandleButtonEndTurnClicked);
            HandleStateChanged(Model.BattleInstance.StateMachine.CurrentState,
                Model.BattleInstance.StateMachine.CurrentState);
        }

        protected override void TerminateCommon()
        {
            Model.BattleInstance.StateMachine.OnStateChanged -= HandleStateChanged;
            View.ButtonEndTurn.onClick.RemoveListener(HandleButtonEndTurnClicked);
        }

        private void HandleStateChanged(BattleState oldState, BattleState newState)
        {
            switch (newState.Id)
            {
                case EBattleStateId.GameOver:
                    StateRouter.SwitchState<BattleScreenStateGameOver>();
                    break;
                case EBattleStateId.PlayerTurn:
                    StateRouter.SwitchState<BattleScreenStateAlly>();
                    break;
                case EBattleStateId.EnemyTurn:
                    StateRouter.SwitchState<BattleScreenStateEnemy>();
                    break;
                default:
                    return;
            }
        }
        
        private void HandleButtonEndTurnClicked()
        {
            Model.BattleInstance.SendActionRequest(new ActionRequestEndTurn
            {
                CasterId = Model.UserDataService.LocalPlayer.Id
            });
        }
    }
}