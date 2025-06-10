using Leopotam.Ecs;
using Logic.ActionRequests;
using Logic.Components;
using Logic.GameStateEvents;
using UnityEngine;

namespace Logic.Systems
{
    public class EndTurnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameEventLogger _gameEventLogger = null;
        private readonly EcsFilter<ActionRequestEndTurn> _filter = null;
        private readonly EcsFilter<ComponentGame> _filterGame = null;

        public void Init()
        {
            ref var componentGame = ref _filterGame.Get1(0);
            _gameEventLogger.LogGameEvent(new GameStateEventPlayerChanged
            {
                NewPlayerId = componentGame.CurrentPlayerId
            });
        }
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                var request = _filter.Get1(index);
                Debug.Log($"EndTurnSystem request received from player{request.CasterId}");
                ref var componentGame = ref _filterGame.Get1(0);

                if (componentGame.IsGameOver)
                {
                    return;
                }
                
                if (componentGame.CurrentPlayerId != request.CasterId)
                {
                    return;
                }
                
                componentGame.CurrentPlayerIndex = componentGame.CurrentPlayerIndex + 1 >= componentGame.PlayerIds.Count
                    ? 0
                    : componentGame.CurrentPlayerIndex + 1;
                Debug.Log($"Player {componentGame.CurrentPlayerId} is now active");
                _gameEventLogger.LogGameEvent(new GameStateEventPlayerChanged
                {
                    NewPlayerId = componentGame.CurrentPlayerId
                });
            }
        }
    }
}