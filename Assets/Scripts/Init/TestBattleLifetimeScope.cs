using Client;
using Client.Actions;
using Client.Pathfinding;
using Client.Replay;
using Common;
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
            builder.Register<ClientTileMovePriceGetter>(Lifetime.Singleton).As<ITileMovePriceGetter>();
            builder.Register<PathfinderService>(Lifetime.Singleton).As<IPathfinderService>();
            builder.RegisterInstance(_battleInstance).As<BattleInstance>();
            builder.RegisterInstance(_battleInstanceClient).As<BattleInstanceClient>();
            builder.RegisterInstance(_replayService).As<ReplayService>();
        }
    }
}