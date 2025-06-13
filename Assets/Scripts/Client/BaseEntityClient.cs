using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Client
{
    public abstract class BaseEntityClient : IEntityClient
    {
        private readonly Dictionary<EPropertyType, IEntityPropertyClient> _properties = new();

        #region IEntityModel Members

        public IEntityPropertyClient Get(EPropertyType propertyType)
        {
            return _properties[propertyType];
        }

        #endregion

        public void Set(EPropertyType type, int value)
        {
            Debug.Log($"Set {type} to {value.GetType()}");
            _properties[type] = new EntityPropertyClient(type, value);
        }
    }
}