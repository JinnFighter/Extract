using System.Collections.Generic;
using Leopotam.Ecs;
using Logic.Components;

namespace Logic.Systems
{
    public class InitGameSystem : IEcsInitSystem
    {
        private readonly EcsFilter<GameSetupInfo> _filterGame = null;
        private readonly EcsWorld _world = null;
        public void Init()
        {
            foreach (var index in _filterGame)
            {
                var setupInfo = _filterGame.Get1(index);
                var gameEntity = _world.NewEntity();
                ref var componentGame = ref gameEntity.Get<ComponentGame>();
                componentGame.PlayerIds = new List<int>();
                foreach (var playerSetupInfo in setupInfo.PlayersSetupInfo)
                {
                    componentGame.PlayerIds.Add(playerSetupInfo.Id);
                    if (playerSetupInfo.Id == setupInfo.StartingPlayerId)
                    {
                        componentGame.CurrentPlayerIndex = componentGame.PlayerIds.Count - 1;
                    }
                }
            }
        }
    }
}