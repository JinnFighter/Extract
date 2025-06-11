using System.Collections.Generic;
using UnityEngine;

namespace Logic.Descriptions
{
    [CreateAssetMenu(fileName = "UnitDescriptionLibrary", menuName = "Scriptable Objects/UnitDescriptionLibrary")]
    public class UnitDescriptionLibrary : ScriptableObject
    {
        private readonly Dictionary<string, UnitDescription> _descriptions = new();
        [field: SerializeField] public List<UnitDescription> UnitDescriptions { get; set; }

        public void Init()
        {
            foreach (var unitDesc in UnitDescriptions)
            {
                _descriptions.Add(unitDesc.NameId, unitDesc);
            }
        }

        public void Terminate()
        {
            _descriptions.Clear();
        }

        public UnitDescription Get(string nameId)
        {
            return _descriptions[nameId];
        }
    }
}
