using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;
using Logic.Systems;

namespace Logic
{
    public class LogicRunner
    {
        private bool _isStarted;
        private readonly ActionEventLogger _logger = new();
        private IGameEventSender _gameEventSender;
        private readonly LogicModelServer _logicModelServer = new();
        private ILogicSystem _rootSystem;
        public bool IsRunning => _rootSystem != null;

        private readonly Dictionary<EActionRequestType, ILogicSystem> _requestSystems = new()
        {
            { EActionRequestType.EndTurn, new EndTurnSystem() },
            { EActionRequestType.Move, new MoveActionRequestSystem() }
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

        private readonly List<IOptionSystem> _preRunOptionSystems = new()
        {
            new RemovePlayerEndTurnOptionSystem(),
        };
        
        private readonly List<IOptionSystem> _postRunOptionSystems = new()
        {
            new AddPlayerEndTurnOptionSystem()
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
                initSystem.Initialize(gameSetupInfo, _logicModelServer, _logger);
            }

            foreach (var postRunOptionSystem in _postRunOptionSystems)
            {
                postRunOptionSystem.Run(_logicModelServer, _gameEventSender);
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
            if (IsRunning || _logicModelServer.GameEntityServer.IsGameOver)
            {
                return;
            }

            if (!IsValidRequest(request))
            {
                return;
            }

            foreach (var preRunOptionSystem in _preRunOptionSystems)
            {
                preRunOptionSystem.Run(_logicModelServer, _gameEventSender);
            }
            
            _rootSystem = _requestSystems[request.ActionRequestType];
            
            RunEventSequence(request, _rootSystem);
            
            foreach (var postRunSystem in _postRunSystems)
            {
                RunEventSequence(request, postRunSystem);
            }
            
            foreach (var postRunOptionSystem in _postRunOptionSystems)
            {
                postRunOptionSystem.Run(_logicModelServer, _gameEventSender);
            }

            _rootSystem = null;
        }
        
        private void RunEventSequence(ActionRequest request, ILogicSystem system)
        {
            var sequence = CreateGameStateEventSequence(request, _logicModelServer, system);

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
        }

        private IEnumerator<List<ActionEvent>> CreateGameStateEventSequence(ActionRequest rootRequest, LogicModelServer logicModelServer, ILogicSystem logicSystem)
        {
            var sequence = RunSystemSequence(rootRequest, logicModelServer, logicSystem);
            while (sequence.MoveNext())
            {
                yield return sequence.Current;
            }
        }

        private IEnumerator<List<ActionEvent>> RunSystemSequence(ActionRequest rootRequest, LogicModelServer logicModelServer, ILogicSystem logicSystem)
        {
            var prepareRun = logicSystem.RunPrepareLogic(rootRequest, logicModelServer);
            while (prepareRun.MoveNext())
            {
                yield return prepareRun.Current;
            }

            if (logicSystem.IsCancelled)
            {
                var cancelRun = logicSystem.RunCancelLogic(rootRequest, logicModelServer);
                while (cancelRun.MoveNext())
                {
                    yield return cancelRun.Current;
                }
                
                yield break;
            }
            
            var logicRun = logicSystem.RunLogic(rootRequest, logicModelServer);
            while (logicRun.MoveNext())
            {
                yield return logicRun.Current;
            }
            
            var finishRun = logicSystem.RunFinishLogic(rootRequest, logicModelServer);
            while (finishRun.MoveNext())
            {
                yield return logicRun.Current;
            }
        }
        
        private void HandleEventsLogged(List<ActionEvent> obj)
        {
            foreach (var gameStateEvent in obj)
            {
                _gameEventSender.SendGameEvent(gameStateEvent);
            }
        }

        private bool IsValidRequest(ActionRequest actionRequest)
        {
            return !IsRunning && actionRequest.IsValid(_logicModelServer);
        }
    }
}