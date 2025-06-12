using System.Collections.Generic;
using Logic.Components;

namespace Logic.Entities
{
    public abstract class BaseEntityServer : IEntityServer
    {
        public int Id { get; set; }
        public Dictionary<EPropertyType, int> Properties { get; } = new();
    }
}