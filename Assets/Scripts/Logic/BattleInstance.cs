using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using Logic.ActionRequests;
using Logic.GameStateEvents;
using Logic.States;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour, IGameEventSender, IActionRequestSender
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        public readonly GameStateEventListener GameStateEventListener = new();
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;
        public BattleInstanceModel Model { get; } = new();
        public BattleStateMachine StateMachine { get; private set; }
        private readonly LogicRunner _logicRunner = new();

        public async void Init()
        {
            StateMachine =
                new BattleStateMachine(this, _gameFieldSetup, _userDataService, _lobbyService, _networkService);
            StateMachine.Init();
            _networkService.SubscribeClientBroadcast<GameStateEventGameStarted>(
                HandleGameStateEventGameStartedReceived);
            _networkService.SubscribeClientBroadcast<BroadcastBattleStateInit>(HandleBroadcastBattleStateInit);
            GameStateEventListener.Init(this, _networkService);
            _userDataService.LocalPlayer.SetReady(true);
            await UniTask.WaitUntil(() => _lobbyService.Players.All(player => player.IsReady));
            if (IsHostStarted)
            {
                _networkService.SubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
                _networkService.SendServerBroadcast(new BroadcastBattleStateInit());
            }
        }

        public void Terminate()
        {
            StateMachine?.Terminate();
            GameStateEventListener.Terminate();
            _networkService.UnsubscribeClientBroadcast<GameStateEventGameStarted>(
                HandleGameStateEventGameStartedReceived);
            _networkService.UnsubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
            _networkService.UnsubscribeClientBroadcast<BroadcastBattleStateInit>(HandleBroadcastBattleStateInit);
            _logicRunner.StopGameLogic();
        }

        public void SendActionRequest<T>(T actionRequest) where T : IActionRequest
        {
            _networkService.SendClientBroadcast(new ActionRequestBroadcast
            {
                ActionRequest = actionRequest
            });
        }

        private void HandleBroadcastBattleStateInit(BroadcastBattleStateInit arg1, Channel arg2)
        {
            StateMachine.ChangeState(arg1.Id);
        }

        private void HandleActionRequest(NetworkConnection arg1, ActionRequestBroadcast arg2, Channel arg3)
        {
            _logicRunner.RunLogic(arg2);
        }

        private void HandleGameStateEventGameStartedReceived(GameStateEventGameStarted arg1, Channel arg2)
        {
            StateMachine.ChangeState(Model.CurrentPlayerId == _userDataService.LocalPlayer.Id
                ? EBattleStateId.PlayerTurn
                : EBattleStateId.EnemyTurn);
        }

        public void SetupGameServer()
        {
            var playersSetupInfo = new List<PlayerSetupInfo>();
            var tilesSetupInfo = new List<TileSetupInfo>();
            var unitsSetupInfo = new List<UnitSetupInfo>();
            var id = 0;
            var teamId = 0;
            
            foreach (var player in _lobbyService.Players)
            {
                playersSetupInfo.Add(new PlayerSetupInfo
                {
                    Id = player.Id
                });
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

            foreach (var tileSetup in _gameFieldSetup.TileSetups)
            {
                var tileSetupInfo = new TileSetupInfo
                {
                    TilePosition = tileSetup.TilePosition,
                    WorldPosition = tileSetup.transform.position
                };
                tilesSetupInfo.Add(tileSetupInfo);
            }

            var setupInfo = new GameSetupInfo
            {
                PlayersSetupInfo = playersSetupInfo,
                UnitsSetupInfo = unitsSetupInfo,
                TilesSetupInfo = tilesSetupInfo,
                StartingPlayerId = _userDataService.LocalPlayer.Id,
            };

            _logicRunner.StartGameLogic(setupInfo, this);
            _networkService.SendServerBroadcast(setupInfo);
        }

        public void SendGameEvent<T>(T gameStateEvent) where T : struct, IGameStateEvent, IBroadcast
        {
            if (!IsServerInitialized) return;

            _networkService.SendServerBroadcast(gameStateEvent);
        }
    }

    public struct GameSetupInfo : IBroadcast
    {
        public List<PlayerSetupInfo> PlayersSetupInfo;
        public List<TileSetupInfo> TilesSetupInfo;
        public List<UnitSetupInfo> UnitsSetupInfo;
        public int StartingPlayerId;
    }

    public struct PlayerSetupInfo
    {
        public int Id;
    }

    public struct TileSetupInfo
    {
        public Vector2Int TilePosition;
        public Vector3 WorldPosition;
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

    public interface IBroadcastBattleState
    {
        EBattleStateId Id { get; }
    }

    public struct BroadcastBattleStateInit : IBroadcastBattleState, IBroadcast
    {
        public EBattleStateId Id => EBattleStateId.Init;
    }
}