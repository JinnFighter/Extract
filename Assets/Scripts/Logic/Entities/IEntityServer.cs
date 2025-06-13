using System.Collections.Generic;

namespace Logic.Entities
{
    public interface IEntityServer
    {
        int Id { get; }
        Dictionary<EPropertyType, int> Properties { get; }
    }
}
