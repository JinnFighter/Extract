using System.Collections.Generic;
using UnityEngine;

namespace Client.Descriptions
{
    [CreateAssetMenu(fileName = "UnitViewLibrary", menuName = "Scriptable Objects/UnitViewLibrary")]
    public class UnitViewLibrary : ScriptableObject
    {
        private readonly Dictionary<string, UnitViewDescription> _descriptions = new Dictionary<string, UnitViewDescription>();
        [field: SerializeField] public List<UnitViewDescription> UnitDescriptions { get; set; }

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

        public UnitViewDescription Get(string nameId)
        {
            return _descriptions[nameId];
        }
    }
}
