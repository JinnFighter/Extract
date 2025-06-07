using UnityEngine;

namespace Logic
{
    public class TileEntityModel : BaseEntityModel, ITileEntityModel
    {
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public bool IsWalkable { get; set; }
    }
}