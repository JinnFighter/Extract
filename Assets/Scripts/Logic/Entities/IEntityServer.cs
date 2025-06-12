using System.Collections.Generic;
using Logic.Components;

namespace Logic.Entities
{
    public interface IEntityServer
    {
        int Id { get; }
        Dictionary<EPropertyType, int> Properties { get; }
    }
}
