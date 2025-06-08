using System;
using Logic.Components;
using UnityEngine;

namespace Logic.Descriptions
{
    [Serializable]
    public class EntityPropertyDescription
    {
        [field: SerializeField] public EPropertyType PropertyType { get; private set; }
        [field: SerializeField] public int DefaultValue { get; private set; }
        [field: SerializeField] public bool IsInitialTag { get; private set; }
    }
}