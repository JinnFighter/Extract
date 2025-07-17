using Reactivity;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class WidgetActionOption : BaseUiWidget<WidgetActionOptionModel, WidgetViewActionOption>
    {
        protected override void InitInner()
        {
            SubscriptionAggregator.ListenEvent(Model.ActionName, HandleActionNameChanged, true);
            SubscriptionAggregator.ListenEvent(View.ButtonCallAction.onClick, HandleButtonCallActionClicked);
        }

        private void HandleActionNameChanged(object sender, GenericEventArg<string> e)
        {
            View.TextActionName.text = e.Value;
        }

        private void HandleButtonCallActionClicked()
        {
        }
    }
}