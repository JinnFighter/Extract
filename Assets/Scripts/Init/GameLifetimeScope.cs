using Client.Descriptions;
using Common;
using Logic.Descriptions;
using UiService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Init
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private NetworkService _networkService;
        [SerializeField] private LobbyService _lobbyService;
        [SerializeField] private LoadingService _loadingService;
        [SerializeField] private UserDataService _userDataService;
        [SerializeField] private UiService.UiService _uiService;
        [SerializeField] private UnitDescriptionLibrary _unitDescriptionLibrary;
        [SerializeField] private UnitViewLibrary _unitViewLibrary;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_unitDescriptionLibrary);
            builder.RegisterInstance(_unitViewLibrary);
            builder.RegisterInstance(_networkService);
            builder.RegisterInstance(_lobbyService);
            builder.RegisterInstance(_loadingService);
            builder.RegisterInstance(_userDataService);
            builder.RegisterInstance<IUiService>(_uiService);
        }
    }
}