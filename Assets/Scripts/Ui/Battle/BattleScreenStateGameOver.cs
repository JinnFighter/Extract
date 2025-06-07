using UiService.Code.Widgets;
using UnityEngine;

namespace Ui.Battle
{
    public class BattleScreenStateGameOver : BaseUiScreenState<BattleScreenStateGameOverModel, BattleScreenStateViewGameOver>
    {
        protected override void InitInner()
        {
            View.TextWinnerId.text = $"Game over! Winner is {Model.BattleInstance.Model.CurrentPlayerId}";
            View.ButtonQuitGame.onClick.AddListener(HandleButtonQuitGameClicked);
        }

        protected override void TerminateInner()
        {
            View.ButtonQuitGame.onClick.RemoveListener(HandleButtonQuitGameClicked);
        }
        
        private void HandleButtonQuitGameClicked()
        {
            Application.Quit();
        }
    }
}