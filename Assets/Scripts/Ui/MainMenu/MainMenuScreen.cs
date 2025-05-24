using UiService.Code.Widgets;

namespace Ui.MainMenu
{
    public class MainMenuScreen : BaseUiScreenWithStates<MainMenuScreenModel, MainMenuScreenView>
    {
        protected override void RegisterStates()
        {
            RegisterState<MenuMainMenuScreenState>(Model.MainMenuStateModel, View.MainMenuStateView, true);
        }
    }
}