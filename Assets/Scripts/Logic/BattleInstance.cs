using System;
using System.Collections.Generic;
using Common;
using FishNet.Broadcast;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using VContainer;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour
    {
        public event Action<GameSetupInfo> OnGameSetup;
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        [Inject] private LobbyService _lobbyService;
        public Dictionary<Vector2Int, ITileEntityModel> TileEntityModels = new();
        public Dictionary<int, IUnitEntityModel> UnitEntityModels = new();
        [Inject] private NetworkService _networkService;

        public void Init()
        {
            _networkService.SubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            if (IsServerInitialized) SetupGameServer();
        }
        
        public void Terminate()
        {
            _networkService.UnsubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            UnitEntityModels?.Clear();
            UnitEntityModels = null;
            TileEntityModels?.Clear();
            TileEntityModels = null;
        }
        
        private void SetupGame(GameSetupInfo gameSetupInfo)
        {
            TileEntityModels = SetupGameField();
            UnitEntityModels = SetupUnits(gameSetupInfo);
            Debug.Log($"_tiles: {TileEntityModels.Count}");
            Debug.Log($"_units: {UnitEntityModels.Count}");
            OnGameSetup?.Invoke(gameSetupInfo);
        }

        private void SetupGameServer()
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
                UnitsSetupInfo = unitsSetupInfo
            };
            _networkService.SendServerBroadcast(setupInfo);
        }

        private Dictionary<Vector2Int, ITileEntityModel> SetupGameField()
        {
            Debug.Log(_gameFieldSetup.TileSetups.Count);
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
        
        private void HandleBroadcastGameSetupInfo(GameSetupInfo arg1, Channel arg2)
        {
            SetupGame(arg1);
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
}