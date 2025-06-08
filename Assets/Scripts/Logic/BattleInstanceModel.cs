using System;
using System.Collections.Generic;
using Logic.GameStateEvents;
using UnityEngine;

namespace Logic
{
    public class BattleInstanceModel
    {
        private readonly Dictionary<Vector2Int, ITileEntityModel> _tileEntityModels = new();
        private readonly Dictionary<int, IUnitEntityModel> _unitEntityModels = new();
        public IReadOnlyDictionary<Vector2Int, ITileEntityModel> TileEntityModels => _tileEntityModels;
        public IReadOnlyDictionary<int, IUnitEntityModel> UnitEntityModels => _unitEntityModels;
        public Dictionary<int, IPlayerEntityModel> PlayerEntityModels { get; } = new();
        public event Action<ITileEntityModel> OnTileEntityAdded;
        public event Action<ITileEntityModel> OnTileEntityRemoved;
        public event Action<IUnitEntityModel> OnUnitEntityAdded;
        public event Action<IUnitEntityModel> OnUnitEntityRemoved;
        
        public int CurrentPlayerId { get; private set; }
        public int WinnerId { get; private set; } = -1;

        public void AddPlayer(GameStateEventFullEntity entity)
        {
            if (entity.EntityType != EEntityType.Player)
            {
                return;
            }

            var model = new PlayerEntityModel();
            foreach (var kvp in entity.Properties)
            {
                model.Set(kvp.Key, kvp.Value);
            }
            
            PlayerEntityModels.Add(entity.Id, model);
        }

        public void AddTile(GameStateEventFullEntity entity)
        {
            var tileEntityModel = new TileEntityModel
            {
                Position = entity.TilePosition,
                IsWalkable = true,
                WorldPosition = entity.WorldPosition
            };

            foreach (var kvp in entity.Properties)
            {
                tileEntityModel.Set(kvp.Key, kvp.Value);
            }

            _tileEntityModels.Add(tileEntityModel.Position, tileEntityModel);
            OnTileEntityAdded?.Invoke(tileEntityModel);
        }

        public void RemoveTile(Vector2Int position)
        {
            if (!_tileEntityModels.Remove(position, out var tileEntityModel)) return;

            OnTileEntityRemoved?.Invoke(tileEntityModel);
        }

        public void AddUnit(GameStateEventFullEntity entity)
        {
            var model = new UnitEntityModel
            {
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                NameId = entity.NameId,
                Position = entity.TilePosition,
                WorldPosition = entity.WorldPosition
            };

            foreach (var kvp in entity.Properties)
            {
                model.Set(kvp.Key, kvp.Value);
            }
            
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