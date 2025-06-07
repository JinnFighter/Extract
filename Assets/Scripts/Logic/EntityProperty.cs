using Logic.Components;
using UnityEngine;

namespace Logic
{
    public class EntityProperty : BaseEntityProperty<int>
    {
        public EntityProperty(EPropertyType propertyType = EPropertyType.None, int value = default)
        {
            PropertyType = propertyType;
            Value = value;
            Debug.Log($"Adding entity property {propertyType} with value {Value}");
        }
    }
}