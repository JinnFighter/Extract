using System;
using System.Collections.Generic;
using Logic;
using Logic.ActionEvents;
using UnityEngine;

namespace Client
{
    public class LogicModelClient
    {
        private readonly Dictionary<Vector2Int, ITileEntityClient> _tileEntityModels = new();
        private readonly Dictionary<int, IUnitEntityModelClient> _unitEntityModels = new();
        public IReadOnlyDictionary<Vector2Int, ITileEntityClient> TileEntityModels => _tileEntityModels;
        public IReadOnlyDictionary<int, IUnitEntityModelClient> UnitEntityModels => _unitEntityModels;
        public event Action<ITileEntityClient> OnTileEntityAdded;
        public event Action<ITileEntityClient> OnTileEntityRemoved;
        public event Action<IUnitEntityModelClient> OnUnitEntityAdded;
        public event Action<IUnitEntityModelClient> OnUnitEntityRemoved;
        
        public int CurrentPlayerId { get; private set; }
        public int WinnerId { get; private set; } = -1;

        public IPlayerEntityClient Player1Entity { get; private set; } = new PlayerEntityClient();
        public IPlayerEntityClient Player2Entity { get; private set; } = new PlayerEntityClient();

        public IPlayerEntityClient GetPlayerEntity(int playerId) => playerId == 2 ? Player1Entity : Player2Entity;

        public void AddPlayer(ActionEventFullEntity entity)
        {
            var model = GetPlayerEntity(entity.Id);
            model.Id = entity.Id;
            model.NetId = entity.NetId;
            foreach (var kvp in entity.Properties)
            {
                model.Set(kvp.Key, kvp.Value);
            }
        }

        public void AddTile(ActionEventFullEntity entity)
        {
            var tileEntityModel = new TileEntityClient
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

        public void AddUnit(ActionEventFullEntity entity)
        {
            var model = new UnitEntityClient
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