using Logic;
using UnityEngine;

namespace Client
{
    public class EntityPropertyClient : BaseEntityPropertyClient<int>
    {
        public EntityPropertyClient(EPropertyType propertyType = EPropertyType.None, int value = default)
        {
            PropertyType = propertyType;
            Value = value;
            Debug.Log($"Adding entity property {propertyType} with value {Value}");
        }
    }
}