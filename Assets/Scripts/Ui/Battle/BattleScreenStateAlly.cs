using Logic.ActionRequests;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class BattleScreenStateAlly : BaseUiScreenState<BattleScreenStateModelAlly, BattleScreenStateViewAlly>
    {
        protected override void InitInner()
        {
            var playerEntityModel =
                Model.BattleInstanceModel.PlayerEntityModels[Model.BattleInstanceModel.CurrentPlayerId];
            playerEntityModel.OnOptionAdded += HandleOptionAdded;
            playerEntityModel.OnOptionRemoved += HandleOptionRemoved;
            if (playerEntityModel.ActionRequestOptions.TryGetValue(EActionRequestType.EndTurn , out _))
            {
                AddOption();
            }
            else
            {
                RemoveOption();
            }
        }

        protected override void TerminateInner()
        {
            RemoveOption();
            var playerEntityModel =
                Model.BattleInstanceModel.PlayerEntityModels[Model.BattleInstanceModel.CurrentPlayerId];
            playerEntityModel.OnOptionAdded -= HandleOptionAdded;
            playerEntityModel.OnOptionRemoved -= HandleOptionRemoved;
        }


        private void HandleOptionAdded(ActionRequestOption obj)
        {
            if (obj.RequestType != EActionRequestType.EndTurn)
            {
                return;
            }
            
            AddOption();
        }
        
        private void HandleOptionRemoved(ActionRequestOption obj)
        {
            if (obj.RequestType != EActionRequestType.EndTurn)
            {
                return;
            }
            
            RemoveOption();
        }

        private void AddOption()
        {
            View.ButtonEndTurn.gameObject.SetActive(true);
            View.ButtonEndTurn.onClick.AddListener(HandleButtonEndTurn);
        }

        private void RemoveOption()
        {
            View.ButtonEndTurn.onClick.RemoveListener(HandleButtonEndTurn);
            View.ButtonEndTurn.gameObject.SetActive(false);
        }
        
        private void HandleButtonEndTurn()
        {
            var playerEntityModel =
                Model.BattleInstanceModel.PlayerEntityModels[Model.BattleInstanceModel.CurrentPlayerId];
            Model.ActionRequestSender.SendActionRequest(new ActionRequestEndTurn
            {
                CasterId = playerEntityModel.Id
            });
        }
    }
}