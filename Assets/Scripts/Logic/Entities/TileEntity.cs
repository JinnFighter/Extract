using UnityEngine;

namespace Logic.Entities
{
    public class TileEntity : BaseEntity
    {
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
    }
}