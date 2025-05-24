using Common;
using MVVM;

namespace Ui.MainMenu
{
    public class MainMenuScreenStateModel : IModel
    {
        public LoadingService LoadingService;
        public LobbyService LobbyService;
        public  UserDataService UserDataService;
        public NetworkService NetworkService;
    }
}