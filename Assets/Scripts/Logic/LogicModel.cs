using System.Collections.Generic;
using Logic.Entities;
using UnityEngine;

namespace Logic
{
    public class LogicModel
    {
        public GameEntity GameEntity { get; } = new();
        public Dictionary<int, PlayerEntity> PlayerEntities { get; } = new();
        public Dictionary<int, UnitEntity> UnitEntities { get; } = new();
        public Dictionary<Vector2Int, TileEntity> TileEntities { get; } = new();
    }
}
