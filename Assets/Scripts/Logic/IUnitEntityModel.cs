using UnityEngine;

namespace Logic
{
    public interface IUnitEntityModel : IEntityModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Vector2Int Position { get; set; }
        public int TeamId { get; set; }
    }
}