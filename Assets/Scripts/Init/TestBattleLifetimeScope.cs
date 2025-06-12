using Client;
using Client.Actions;
using Client.Replay;
using Logic;
using Logic.ActionRequests;
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
            builder.Register<UnitEntitySelectorSystem>(Lifetime.Singleton).As<IUnitEntitySelectorSystem>();
            builder.Register<TileEntityEntitySelectorSystem>(Lifetime.Singleton).As<ITileEntitySelectorSystem>();
            builder.Register<ActionRequestBuilderSystem>(Lifetime.Singleton).As<IActionRequestBuilderSystem>();
            builder.RegisterInstance(_battleInstance).As<BattleInstance>();
            builder.RegisterInstance(_battleInstanceClient).As<BattleInstanceClient>();
            builder.RegisterInstance(_replayService).As<ReplayService>();
        }
    }
}