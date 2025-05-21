using System.Collections.Generic;
using Common;
using FishNet.Object;
using UnityEngine;
using VContainer;

namespace Logic
{
    public class BattleInstance : NetworkBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        [Inject] private LobbyService _lobbyService;
        private Dictionary<Vector2Int, ITileEntityModel> _tileEntityModels = new();
        private Dictionary<int, IUnitEntityModel> _unitEntityModels = new();

        public void Init()
        {
            if (IsServerInitialized) SetupGameServer();
        }

        public void Terminate()
        {
            _unitEntityModels?.Clear();
            _unitEntityModels = null;
            _tileEntityModels?.Clear();
            _tileEntityModels = null;
        }

        [Rpc(RunLocally = true)]
        private void SetupGame(GameSetupInfo gameSetupInfo)
        {
            _tileEntityModels = SetupGameField();
            _unitEntityModels = SetupUnits(gameSetupInfo);
            Debug.Log($"_tiles: {_tileEntityModels.Count}");
            Debug.Log($"_units: {_unitEntityModels.Count}");
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
            SetupGame(setupInfo);
        }

        private Dictionary<Vector2Int, ITileEntityModel> SetupGameField()
        {
            var dict = new Dictionary<Vector2Int, ITileEntityModel>();
            foreach (var kvp in _gameFieldSetup.TilesSetup)
            {
                var tileEntityModel = new TileEntityModel
                {
                    Position = kvp.Key,
                    IsWalkable = kvp.Value.Walkable,
                    WorldPosition = kvp.Value.transform.position
                };
                dict[kvp.Key] = tileEntityModel;
            }

            return dict;
        }

        private Dictionary<int, IUnitEntityModel> SetupUnits(GameSetupInfo gameSetupInfo)
        {
            var dict = new Dictionary<int, IUnitEntityModel>();
            foreach (var unit in gameSetupInfo.UnitsSetupInfo) dict[unit.Id] = new UnitEntityModel();
            return dict;
        }
    }

    public struct GameSetupInfo
    {
        public List<UnitSetupInfo> UnitsSetupInfo;
    }

    public struct UnitSetupInfo
    {
        public int Id;
        public string NameId;
        public int OwnerId;
        public int TeamId;
        public Vector2 SpawnPosition;
    }
}