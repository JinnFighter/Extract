using MVVM;

namespace Ui.MainMenu
{
    public class MainMenuScreenModel : IModel
    {
        public MainMenuScreenModel(MainMenuScreenStateModel model, CoterieMainMenuStateModel coterieMainMenuStateModel)
        {
            MainMenuStateModel = model;
            CoterieStateModel = coterieMainMenuStateModel;
        }

        public MainMenuScreenStateModel MainMenuStateModel { get; }
        public CoterieMainMenuStateModel CoterieStateModel { get; }
    }
}