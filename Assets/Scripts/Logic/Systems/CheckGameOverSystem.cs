using Leopotam.Ecs;
using Logic.Components;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class CheckGameOverSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ComponentGame> _filter = null;
        private int _turnCount;
        private readonly BattleInstance _battleInstance = null;
        public void Run()
        {
            if (_turnCount < 2)
            {
                _turnCount++;
                return;
            }
            
            ref var game = ref _filter.Get1(0);
            game.IsGameOver = true;
            game.WinnerId = game.CurrentPlayerId;
            _battleInstance.SendGameEvent(new GameStateEventGameEnded
            {
                EventId = 1,
                TurnNumber = 0,
                WinnerId = game.WinnerId,
            });
        }
    }
}
