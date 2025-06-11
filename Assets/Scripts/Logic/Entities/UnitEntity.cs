using UnityEngine;

namespace Logic.Entities
{
    public class UnitEntity : BaseEntity
    {
        public int OwnerId { get; set; }
        public string NameId { get; set; }
        public Vector2Int Position { get; }
        public Vector3 WorldPosition { get; }
    }
}