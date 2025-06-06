using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class BattleInstanceModel
    {
        private readonly Dictionary<Vector2Int, ITileEntityModel> _tileEntityModels = new();
        private readonly Dictionary<int, IUnitEntityModel> _unitEntityModels = new();
        public IReadOnlyDictionary<Vector2Int, ITileEntityModel> TileEntityModels => _tileEntityModels;
        public IReadOnlyDictionary<int, IUnitEntityModel> UnitEntityModels => _unitEntityModels;
        public event Action<ITileEntityModel> OnTileEntityAdded;
        public event Action<ITileEntityModel> OnTileEntityRemoved;
        public event Action<IUnitEntityModel> OnUnitEntityAdded;
        public event Action<IUnitEntityModel> OnUnitEntityRemoved;
        
        public int CurrentPlayerId { get; private set; }
        public int WinnerId { get; private set; } = -1;

        public void AddTile(TileSetup tileSetup)
        {
            var tileEntityModel = new TileEntityModel
            {
                Position = tileSetup.TilePosition,
                IsWalkable = tileSetup.Walkable,
                WorldPosition = tileSetup.transform.position
            };

            _tileEntityModels.Add(tileEntityModel.Position, tileEntityModel);
            OnTileEntityAdded?.Invoke(tileEntityModel);
        }

        public void RemoveTile(Vector2Int position)
        {
            if (!_tileEntityModels.Remove(position, out var tileEntityModel)) return;

            OnTileEntityRemoved?.Invoke(tileEntityModel);
        }

        public void AddUnit(UnitSetupInfo unitSetupInfo)
        {
            var model = new UnitEntityModel
            {
                Id = unitSetupInfo.Id,
                OwnerId = unitSetupInfo.OwnerId,
                Position = new Vector2Int((int)unitSetupInfo.SpawnPosition.x, (int)unitSetupInfo.SpawnPosition.y),
                WorldPosition = unitSetupInfo.SpawnPosition
            };
            _unitEntityModels.Add(model.Id, model);
            OnUnitEntityAdded?.Invoke(model);
        }

        public void RemoveUnit(int id)
        {
            if (!_unitEntityModels.Remove(id, out var model)) return;

            OnUnitEntityRemoved?.Invoke(model);
        }

        public void SetCurrentPlayer(int playerId)
        {
            CurrentPlayerId = playerId;
        }

        public void SetWinner(int winnerId)
        {
            WinnerId = winnerId;
        }
    }
}