using Network;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Init
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private NetworkService _networkService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_networkService);
        }
    }
}