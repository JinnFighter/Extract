using System.Collections.Generic;
using Logic.Components;
using UnityEngine;

namespace Logic
{
    public abstract class BaseEntityModel : IEntityModel
    {
        private readonly Dictionary<EPropertyType, IEntityProperty> _properties = new();

        #region IEntityModel Members

        public IEntityProperty Get(EPropertyType propertyType)
        {
            return _properties[propertyType];
        }

        #endregion

        public void Set(EPropertyType type, int value)
        {
            Debug.Log($"Set {type} to {value.GetType()}");
            _properties[type] = new EntityProperty(type, value);
        }
    }
}