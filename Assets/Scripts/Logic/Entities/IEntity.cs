using System.Collections.Generic;
using Logic.Components;

namespace Logic.Entities
{
    public interface IEntity
    {
        int Id { get; }
        Dictionary<EPropertyType, int> Properties { get; }
    }
}
