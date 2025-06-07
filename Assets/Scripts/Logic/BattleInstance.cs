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
using Logic.States;
using Logic.Systems;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        public readonly GameStateEventListener GameStateEventListener = new();
        private EcsSystems _ecsSystems;
        private EcsWorld _ecsWorld;
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;
        public BattleInstanceModel Model { get; } = new();
        public BattleStateMachine StateMachine { get; private set; }

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

        private void HandleBroadcastBattleStateInit(BroadcastBattleStateInit arg1, Channel arg2)
        {
            StateMachine.ChangeState(arg1.Id);
        }

        private void HandleActionRequest(NetworkConnection arg1, ActionRequestBroadcast arg2, Channel arg3)
        {
            var entity = _ecsWorld.NewEntity();
            arg2.ActionRequest.AcceptEntity(entity);
            _ecsSystems.Run();
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

            var setupInfo = new GameSetupInfo
            {
                PlayersSetupInfo = playersSetupInfo,
                UnitsSetupInfo = unitsSetupInfo,
                StartingPlayerId = _userDataService.LocalPlayer.Id,
            };

            _ecsWorld = new EcsWorld();
            var entity = _ecsWorld.NewEntity();
            entity.Replace(setupInfo);
            _ecsSystems = new EcsSystems(_ecsWorld);
            _ecsSystems
                .Inject(this)
                .Add(new InitGameSystem())
                .Add(new CheckGameOverSystem())
                .Add(new EndTurnSystem())
                .OneFrame<GameSetupInfo>()
                .OneFrame<ActionRequestEndTurn>()
                .Init();

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
        public List<UnitSetupInfo> UnitsSetupInfo;
        public int StartingPlayerId;
    }

    public struct PlayerSetupInfo
    {
        public int Id;
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