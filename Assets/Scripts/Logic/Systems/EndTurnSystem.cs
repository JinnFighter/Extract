using Leopotam.Ecs;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public class EndTurnSystem : IEcsRunSystem
    {
        private readonly BattleInstance _battleInstance;
        private readonly EcsFilter<ActionRequestEndTurn> _filter = null;

        public void Run()
        {
            if (_filter.IsEmpty()) return;

            _battleInstance.SendGameEvent(new GameStateEventActivePlayerChanged
            {
                EventId = 1,
                TurnNumber = 0,
                NewPlayerId = 1
            });
        }
    }
}