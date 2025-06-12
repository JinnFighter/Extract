using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using Logic.ActionRequests;
using Logic.GameStateEvents;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour, IGameEventSender
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        private readonly LogicRunner _logicRunner = new();
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;

        public void SendGameEvent(ActionEvent actionEvent)
        {
            if (!IsServerInitialized) return;

            _networkService.SendServerBroadcast(new BroadcastActionEvent
            {
                ActionEvent = actionEvent
            });
        }

        public void SendOption(ActionRequestOption option)
        {
            if (!IsServerInitialized) return;

            _networkService.SendServerBroadcast(new BroadcastOption
            {
                Option = option
            });
        }

        public async void Init()
        {
            _userDataService.LocalPlayer.SetReady(true);
            await UniTask.WaitUntil(() => _lobbyService.Players.All(player => player.IsReady));
            if (IsHostStarted)
            {
                _networkService.SubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
                SetupGameServer();
            }
        }

        public void Terminate()
        {
            _networkService.UnsubscribeServerBroadcast<ActionRequestBroadcast>(HandleActionRequest);
            _logicRunner.StopGameLogic();
        }

        private void HandleActionRequest(NetworkConnection arg1, ActionRequestBroadcast arg2, Channel arg3)
        {
            _logicRunner.RunLogic(arg2.ActionRequest);
        }

        private void SetupGameServer()
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
                TilesSetupInfo = tilesSetupInfo
            };

            _logicRunner.StartGameLogic(setupInfo, this);
        }
    }

    public struct BroadcastActionEvent : IBroadcast
    {
        public ActionEvent ActionEvent { get; set; }
    }

    public struct GameSetupInfo
    {
        public List<PlayerSetupInfo> PlayersSetupInfo;
        public List<TileSetupInfo> TilesSetupInfo;
        public List<UnitSetupInfo> UnitsSetupInfo;
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
}