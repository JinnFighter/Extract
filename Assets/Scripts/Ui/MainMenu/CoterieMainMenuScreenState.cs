using UiService.Code.Widgets;

namespace Ui.MainMenu
{
    public class CoterieMainMenuScreenState : BaseUiScreenState<CoterieMainMenuStateModel, CoterieMainMenuStateView>
    {
        protected override void RegisterChildWidgets()
        {
            for (var i = 0; i < Model.UnitModels.Count; i++)
            {
                RegisterChildWidget<WidgetCoterieUnit>(Model.UnitModels[i], View.UnitViews[i]);
            }
        }

        protected override void InitInner()
        {
            SubscriptionAggregator.ListenEvent(View.ButtonBack.onClick, HandleButtonBackClicked);
        }

        private void HandleButtonBackClicked()
        {
            StateRouter.Pop();
        }
    }
}