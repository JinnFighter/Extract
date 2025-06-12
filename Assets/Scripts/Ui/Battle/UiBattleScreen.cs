using Client.States;
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
            Model.BattleStateMachine.OnStateChanged += HandleStateChanged;
            HandleStateChanged(Model.BattleStateMachine.CurrentState,
                Model.BattleStateMachine.CurrentState);
        }

        protected override void TerminateCommon()
        {
            Model.BattleStateMachine.OnStateChanged -= HandleStateChanged;
        }

        protected override void RegisterChildWidgets()
        {
            RegisterChildWidget<WidgetSelectedUnit>(Model.ModelWidgetSelectedUnit, View.SelectedUnit);
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
    }
}