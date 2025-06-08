using Common;
using Leopotam.Ecs;
using Logic.ActionRequests;
using Logic.Descriptions;
using Logic.Systems;

namespace Logic
{
    public class LogicRunner
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _ecsSystems;
        private bool _isRunning;
        
        public void StartGameLogic(GameSetupInfo gameSetupInfo, IGameEventSender gameEventSender)
        {
            if (_isRunning)
            {
                return;
            }
            
            _isRunning = true;
            _ecsWorld = new EcsWorld();
            var entity = _ecsWorld.NewEntity();
            entity.Replace(gameSetupInfo);
            _ecsSystems = new EcsSystems(_ecsWorld);
            _ecsSystems
                .Inject(gameEventSender)
                .Inject(AutoResolver.Resolve<UnitDescriptionLibrary>())
                .Add(new InitGameSystem())
                .Add(new CheckGameOverSystem())
                .Add(new EndTurnSystem())
                .OneFrame<GameSetupInfo>()
                .OneFrame<ActionRequestEndTurn>()
                .Init();
        }

        public void StopGameLogic()
        {
            if (!_isRunning)
            {
                return;
            }
            _isRunning = false;
            _ecsSystems?.Destroy();
            _ecsSystems = null;
            _ecsWorld?.Destroy();
            _ecsWorld = null;
        }

        public void RunLogic(ActionRequestBroadcast request)
        {
            var entity = _ecsWorld.NewEntity();
            request.ActionRequest.AcceptEntity(entity);
            _ecsSystems.Run();
        }
    }
}