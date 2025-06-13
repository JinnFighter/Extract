using UnityEngine;

namespace Logic.Entities
{
    public class UnitEntityServer : BaseEntityServer
    {
        public int OwnerId { get; set; }
        public string NameId { get; set; }
        public Vector2Int Position { get; set; }
    }
}