using Common;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_networkService);
            builder.RegisterInstance(_lobbyService);
            builder.RegisterInstance(_loadingService);
            builder.RegisterInstance(_userDataService);
            builder.RegisterInstance<IUiService>(_uiService);
        }
    }
}