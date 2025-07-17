using System.Collections.Generic;
using UnityEngine;

namespace Logic.Descriptions
{
    [CreateAssetMenu(fileName = "UnitDescription", menuName = "Scriptable Objects/UnitDescription")]
    public class UnitDescription : ScriptableObject
    {
        [field: SerializeField] public string NameId { get; private set; }
        [field: SerializeField] public string ViewNameId { get; private set; }
        [field: SerializeField] public List<EntityPropertyDescription> Properties { get; private set; }
        [field: SerializeField] public List<ActionDescription> Actions { get; private set; }
    }
}
