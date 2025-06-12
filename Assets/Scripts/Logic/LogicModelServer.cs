using System.Collections.Generic;
using Logic.Entities;
using UnityEngine;

namespace Logic
{
    public class LogicModelServer
    {
        public GameEntityServer GameEntityServer { get; } = new();
        public Dictionary<int, PlayerEntityServer> PlayerEntities { get; } = new();
        public Dictionary<int, UnitEntityServer> UnitEntities { get; } = new();
        public Dictionary<Vector2Int, TileEntityServer> TileEntities { get; } = new();
    }
}
