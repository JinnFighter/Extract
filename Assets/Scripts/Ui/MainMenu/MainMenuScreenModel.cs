using MVVM;

namespace Ui.MainMenu
{
    public class MainMenuScreenModel : IModel
    {
        public MainMenuScreenModel(MainMenuScreenStateModel model)
        {
            MainMenuStateModel = model;
        }

        public MainMenuScreenStateModel MainMenuStateModel { get; }
    }
}