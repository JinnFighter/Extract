using Client.States;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class UiBattleScreen : BaseUiScreenWithStates<BattleScreenModel, BattleScreenView>
    {
        protected override void RegisterStates()
        {
            RegisterState<BattleScreenStateAlly>(Model.ModelAlly, View.ScreenStateViewAlly);
            RegisterState<BattleScreenStateEnemy>(Model.ModelEnemy, View.ScreenStateViewEnemy);
            RegisterState<BattleScreenStateActionSelectTile>(Model.ModelSelectTile, View.ScreenStateViewActionSelectTile);
            RegisterState<BattleScreenStateGameOver>(Model.GameOverModel, View.ScreenStateViewGameOver);
        }

        protected override void InitCommon()
        {
            SubscriptionAggregator.ListenEvent(Model.BattleStateMachine.OnStateEntered, HandleStateChanged);
        }

        protected override void RegisterChildWidgets()
        {
            RegisterChildWidget<WidgetSelectedUnit>(Model.ModelWidgetSelectedUnit, View.SelectedUnit);
            RegisterChildWidget<WidgetSelectedUnitOptions>(Model.ModelSelectedUnitOptions, View.SelectedUnitOptions);
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
                case EBattleStateId.ActionSelectTile:
                    StateRouter.SwitchState<BattleScreenStateActionSelectTile>();
                    break;
                default:
                    return;
            }
        }
    }
}