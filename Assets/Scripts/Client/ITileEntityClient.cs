using UnityEngine;

namespace Client
{
    public interface ITileEntityClient
    {
        Vector2Int Position { get; set; }
        Vector3 WorldPosition { get; set; }
        bool IsWalkable { get; set; }
        int OccupierId { get; set; }
    }
}