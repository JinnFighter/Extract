using UnityEngine;

namespace Logic
{
    public interface ITileEntityModel : IEntityModel
    {
        Vector2Int Position { get; set; }
        Vector3 WorldPosition { get; set; }
        bool IsWalkable { get; set; }
    }
}