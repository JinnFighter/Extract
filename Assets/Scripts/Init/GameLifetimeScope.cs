using Network;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Init
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private NetworkService _networkService;
        [SerializeField] private LobbyService _lobbyService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_networkService);
            builder.RegisterInstance(_lobbyService);
        }
    }
}