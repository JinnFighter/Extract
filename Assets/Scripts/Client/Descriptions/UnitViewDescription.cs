using UnityEngine;

namespace Client.Descriptions
{
    [CreateAssetMenu(fileName = "UnitViewDescription", menuName = "Scriptable Objects/UnitViewDescription")]
    public class UnitViewDescription : ScriptableObject
    {
        [field: SerializeField] public string NameId { get; private set; }
        [field: SerializeField] public UnitView View { get; private set; }
    }
}
