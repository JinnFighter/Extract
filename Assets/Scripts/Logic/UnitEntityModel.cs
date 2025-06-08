using UnityEngine;

namespace Logic
{
    public class UnitEntityModel : BaseEntityModel, IUnitEntityModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public int TeamId { get; set; }
        public string NameId { get; set; }
    }
}