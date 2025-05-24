using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using Leopotam.Ecs;
using Logic.ActionRequests;
using Logic.GameStateEvents;
using Logic.Systems;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;
        public Dictionary<Vector2Int, ITileEntityModel> TileEntityModels = new();
        public Dictionary<int, IUnitEntityModel> UnitEntityModels = new();
        public readonly GameStateEventListener GameStateEventListener = new();
        private int _setupCount = 0;
        private EcsWorld _ecsWorld;
        private EcsSystems _ecsSystems;

        public event Action<GameSetupInfo> OnGameSetup;
        public event Action<int> OnGameStarted;

        public async void Init()
        {
            _networkService.SubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            _networkService.SubscribeClientBroadcast<GameStateEventGameStarted>(HandleGameStateEventGameStartedReceived);
            GameStateEventListener.Init(_networkService);
            _userDataService.LocalPlayer.SetReady(true);
            await UniTask.WaitUntil(() => _lobbyService.Players.All(player => player.IsReady));
            if (IsServerInitialized)
            {
                _networkService.SubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
                _networkService.SubscribeServerBroadcast<BroadcastSetupComplete>(HandleBroadcastSetupCompleteReceived);
                SetupGameServer();
            }
        }
        
        public void Terminate()
        {
            GameStateEventListener.Terminate();
            _networkService.UnsubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            _networkService.UnsubscribeClientBroadcast<GameStateEventGameStarted>(HandleGameStateEventGameStartedReceived);
            _networkService.UnsubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
            _networkService.UnsubscribeServerBroadcast<BroadcastSetupComplete>(HandleBroadcastSetupCompleteReceived);
            UnitEntityModels?.Clear();
            UnitEntityModels = null;
            TileEntityModels?.Clear();
            TileEntityModels = null;
            _ecsSystems?.Destroy();
            _ecsSystems = null;
            _ecsWorld?.Destroy();
            _ecsWorld = null;
        }

        public void SendActionRequest<T>(T actionRequest) where T : IActionRequest
        {
            _networkService.SendClientBroadcast(new ActionRequestBroadcast
            {
                ActionRequest = actionRequest
            });
        }
        
        private void HandleActionRequest(NetworkConnection arg1, ActionRequestBroadcast arg2, Channel arg3)
        {
            var entity = _ecsWorld.NewEntity();
            arg2.ActionRequest.AcceptEntity(entity);
            _ecsSystems.Run();
        }

        private void HandleGameStateEventGameStartedReceived(GameStateEventGameStarted arg1, Channel arg2)
        {
            _networkService.UnsubscribeClientBroadcast<GameStateEventGameStarted>(HandleGameStateEventGameStartedReceived);
            OnGameStarted?.Invoke(arg1.StartingPlayerId);
        }

        private void HandleBroadcastSetupCompleteReceived(NetworkConnection arg1, BroadcastSetupComplete arg2, Channel arg3)
        {
            _setupCount++;
            if (_setupCount != _lobbyService.Players.Count)
            {
                return;
            }
            _networkService.UnsubscribeServerBroadcast<BroadcastSetupComplete>(HandleBroadcastSetupCompleteReceived);
            SendGameEvent(new GameStateEventGameStarted
            {
                EventId = 0,
                TurnNumber = 0,
                StartingPlayerId = _userDataService.LocalPlayer.Id
            });
        }

        private void HandleBroadcastGameSetupInfo(GameSetupInfo arg1, Channel arg2)
        {
            SetupGame(arg1);
        }

        public void SetupGame(GameSetupInfo gameSetupInfo)
        {
            TileEntityModels = SetupGameField();
            UnitEntityModels = SetupUnits(gameSetupInfo);
            OnGameSetup?.Invoke(gameSetupInfo);
            _networkService.SendClientBroadcast(new BroadcastSetupComplete
            {
                UserId = OwnerId
            });
        }

        public void SetupGameServer()
        {
            var unitsSetupInfo = new List<UnitSetupInfo>();
            var id = 0;
            var teamId = 0;
            foreach (var player in _lobbyService.Players)
            {
                unitsSetupInfo.Add(new UnitSetupInfo
                {
                    Id = id,
                    NameId = "test",
                    OwnerId = player.OwnerId,
                    TeamId = teamId,
                    SpawnPosition = teamId == 0
                        ? _gameFieldSetup.Team1SpawnPoints[0]
                        : _gameFieldSetup.Team2SpawnPoints[0]
                });
                id++;
                teamId++;
            }

            var setupInfo = new GameSetupInfo
            {
                UnitsSetupInfo = unitsSetupInfo,
            };

            _ecsWorld = new EcsWorld();
            _ecsSystems = new EcsSystems(_ecsWorld);
                _ecsSystems
                    .Inject(this)
                    .Add(new EndTurnSystem())
                    .OneFrame<ActionRequestEndTurn>()
                    .Init();
            
            _networkService.SendServerBroadcast(setupInfo);
        }

        private Dictionary<Vector2Int, ITileEntityModel> SetupGameField()
        {
            var dict = new Dictionary<Vector2Int, ITileEntityModel>();
            foreach (var tileSetup in _gameFieldSetup.TileSetups)
            {
                var tileEntityModel = new TileEntityModel
                {
                    Position = tileSetup.TilePosition,
                    IsWalkable = tileSetup.Walkable,
                    WorldPosition = tileSetup.transform.position
                };
                dict[tileSetup.TilePosition] = tileEntityModel;
            }

            return dict;
        }

        private Dictionary<int, IUnitEntityModel> SetupUnits(GameSetupInfo gameSetupInfo)
        {
            var dict = new Dictionary<int, IUnitEntityModel>();
            foreach (var unit in gameSetupInfo.UnitsSetupInfo)
                dict[unit.Id] = new UnitEntityModel
                {
                    Id = unit.Id,
                    OwnerId = unit.OwnerId,
                    Position = new Vector2Int((int)unit.SpawnPosition.x, (int)unit.SpawnPosition.y),
                    WorldPosition = unit.SpawnPosition
                };
            return dict;
        }

        public void SendGameEvent<T>(T gameStateEvent) where T : struct, IGameStateEvent, IBroadcast
        {
            if (!IsServerInitialized)
            {
                return;
            }
            
            _networkService.SendServerBroadcast(gameStateEvent);
        }
    }

    public struct GameSetupInfo : IBroadcast
    {
        public List<UnitSetupInfo> UnitsSetupInfo;
    }

    public struct UnitSetupInfo
    {
        public int Id;
        public string NameId;
        public int OwnerId;
        public int TeamId;
        public Vector3 SpawnPosition;
    }

    public struct BroadcastSetupComplete : IBroadcast
    {
        public int UserId;
    }
}