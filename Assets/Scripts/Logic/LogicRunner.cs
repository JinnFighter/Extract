using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;
using Logic.Systems;

namespace Logic
{
    public class LogicRunner
    {
        private bool _isStarted;
        private readonly GameEventLogger _logger = new();
        private IGameEventSender _gameEventSender;
        private readonly LogicModel _logicModel = new();
        private ILogicSystem _rootSystem;
        public bool IsRunning => _rootSystem != null;

        private readonly Dictionary<EActionRequestType, ILogicSystem> _requestSystems = new()
        {
            { EActionRequestType.EndTurn, new EndTurnSystem() },
        };
        private readonly List<IInitializeSystem> _initializeSystems = new()
        {
            new InitGameSystem(),
            new EndTurnSystem()
        };

        private readonly List<ILogicSystem> _postRunSystems = new()
        {
            new CheckGameOverSystem(),
        };
        
        public void StartGameLogic(GameSetupInfo gameSetupInfo, IGameEventSender gameEventSender)
        {
            if (_isStarted)
            {
                return;
            }
            
            _gameEventSender = gameEventSender;
            
            _isStarted = true;
            _logger.OnEventsLogged += HandleEventsLogged;

            foreach (var initSystem in _initializeSystems)
            {
                initSystem.Initialize(gameSetupInfo, _logicModel, _logger);
            }
        }
        
        public void StopGameLogic()
        {
            if (!_isStarted)
            {
                return;
            }
            _isStarted = false;
            
            _logger.OnEventsLogged -= HandleEventsLogged;
        }

        public void RunLogic(ActionRequest request)
        {
            if (IsRunning || _logicModel.GameEntity.IsGameOver)
            {
                return;
            }
            
            _rootSystem = _requestSystems[request.ActionRequestType];

            var sequence = CreateGameStateEventSequence(request, _logicModel, _rootSystem);

            while (sequence.MoveNext())
            {
                var gameStateEvents = sequence.Current;
                if (gameStateEvents == null)
                {
                    continue;
                }
                foreach (var gameStateEvent in gameStateEvents)
                {
                    _logger.LogGameEvent(gameStateEvent);
                }
            }
            
            foreach (var postRunSystem in _postRunSystems)
            {
                var postSequence = CreateGameStateEventSequence(request, _logicModel, postRunSystem);

                while (postSequence.MoveNext())
                {
                    var gameStateEvents = postSequence.Current;
                    if (gameStateEvents == null)
                    {
                        continue;
                    }
                    foreach (var gameStateEvent in gameStateEvents)
                    {
                        _logger.LogGameEvent(gameStateEvent);
                    }
                }
            }

            _rootSystem = null;
        }

        private IEnumerator<List<GameStateEvent>> CreateGameStateEventSequence(ActionRequest rootRequest, LogicModel logicModel, ILogicSystem logicSystem)
        {
            var sequence = RunSystemSequence(rootRequest, logicModel, logicSystem);
            while (sequence.MoveNext())
            {
                yield return sequence.Current;
            }
        }

        private IEnumerator<List<GameStateEvent>> RunSystemSequence(ActionRequest rootRequest, LogicModel logicModel, ILogicSystem logicSystem)
        {
            var logicRun = logicSystem.RunLogic(rootRequest, logicModel);
            while (logicRun.MoveNext())
            {
                yield return logicRun.Current;
            }
        }
        
        private void HandleEventsLogged(List<GameStateEvent> obj)
        {
            foreach (var gameStateEvent in obj)
            {
                _gameEventSender.SendGameEvent(gameStateEvent);
            }
        }
    }
}