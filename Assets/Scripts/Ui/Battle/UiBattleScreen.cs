using Logic.ActionRequests;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class UiBattleScreen : BaseUiScreenWithStates<BattleScreenModel, BattleScreenView>
    {
        protected override void RegisterStates()
        {
            RegisterState<BattleScreenStateAlly>(Model.ModelAlly, View.ScreenStateViewAlly);
            RegisterState<BattleScreenStateEnemy>(Model.ModelEnemy, View.ScreenStateViewEnemy);
        }

        protected override void InitCommon()
        {
            Model.BattleInstance.OnGameStarted += HandleGameStarted;
            View.ButtonEndTurn.onClick.AddListener(HandleButtonEndTurnClicked);
        }

        protected override void TerminateCommon()
        {
            Model.BattleInstance.OnGameStarted -= HandleGameStarted;
            View.ButtonEndTurn.onClick.RemoveListener(HandleButtonEndTurnClicked);
        }
        
        private void HandleGameStarted(int id)
        {
            if (Model.UserDataService.LocalPlayer.Id == id)
                StateRouter.SwitchState<BattleScreenStateAlly>();
            else
                StateRouter.SwitchState<BattleScreenStateEnemy>();
        }
        
        private void HandleButtonEndTurnClicked()
        {
            Model.BattleInstance.SendActionRequest(new ActionRequestEndTurn
            {
                CasterId = Model.UserDataService.LocalPlayer.Id,
            });
        }
    }
}