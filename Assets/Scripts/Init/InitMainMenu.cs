using Common;
using Logic.Descriptions;
using Ui.MainMenu;
using UiService;
using UnityEngine;
using VContainer;

namespace Init
{
    public class InitMainMenu : MonoBehaviour
    {
        [Inject] private IUiService _uiService;
        [Inject] private LoadingService _loadingService;
        [Inject] private NetworkService _networkService;
        [Inject] private LobbyService _lobbyService;
        [Inject] private UserDataService _userDataService;
        [Inject] private UnitDescriptionLibrary _unitDescriptionLibrary;
        private MainMenuScreenModel _model;

        private void Start()
        {
            _model = new MainMenuScreenModel(new MainMenuScreenStateModel
            {
                LoadingService = _loadingService,
                NetworkService = _networkService,
                LobbyService = _lobbyService,
                UserDataService = _userDataService,
            }, new CoterieMainMenuStateModel(_unitDescriptionLibrary, _userDataService));
            _uiService.Open<MainMenuScreen>(_model, typeof(MainMenuScreenView));
        }

        private void OnDestroy()
        {
            if (_model == null)
            {
                return;
            }
            _uiService.Close(_model);
        }
    }
}