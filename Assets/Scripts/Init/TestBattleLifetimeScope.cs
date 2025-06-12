using Client;
using Client.Replay;
using Logic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Init
{
    public class TestBattleLifetimeScope : LifetimeScope
    {
        [SerializeField] private ReplayService _replayService;
        [SerializeField] private BattleInstance _battleInstance;
        [SerializeField] private BattleInstanceClient _battleInstanceClient;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_battleInstance).As<BattleInstance>();
            builder.RegisterInstance(_battleInstanceClient).As<BattleInstanceClient>();
            builder.RegisterInstance(_replayService).As<ReplayService>();
        }
    }
}