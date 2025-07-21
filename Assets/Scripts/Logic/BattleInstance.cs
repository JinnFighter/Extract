using System.Collections.Generic;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using Logic.ActionRequests;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        private readonly LogicRunner _logicRunner = new();
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;
        [Inject] private IActionEventSender _actionEventSender;
        
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

                for (var i = 0; i < player.Coterie.Count; i++)
                {
                    var unitId = player.Coterie[i];
                    unitsSetupInfo.Add(new UnitSetupInfo
                    {
                        Id = id,
                        NameId = unitId,
                        OwnerId = player.OwnerId,
                        TeamId = teamId,
                        SpawnPosition = teamId == 0
                            ? _gameFieldSetup.Team1SpawnPoints[i]
                            : _gameFieldSetup.Team2SpawnPoints[i]
                    });
                    id++;
                }
                
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

            _logicRunner.StartGameLogic(setupInfo, _actionEventSender);
        }
    }
}