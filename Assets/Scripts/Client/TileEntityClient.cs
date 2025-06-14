using UnityEngine;

namespace Client
{
    public class TileEntityClient : BaseEntityClient, ITileEntityClient
    {
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public bool IsWalkable { get; set; }
        public int OccupierId { get; set; } = -1;
    }
}