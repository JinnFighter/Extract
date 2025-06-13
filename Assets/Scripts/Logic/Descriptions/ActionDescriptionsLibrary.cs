using ArtificeToolkit.Runtime.SerializedDictionary;
using UnityEngine;

namespace Logic.Descriptions
{
    [CreateAssetMenu(fileName = "ActionDescriptionsLibrary", menuName = "Scriptable Objects/ActionDescriptionsLibrary")]
    public class ActionDescriptionsLibrary : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<string, ActionDescription> ActionDescriptions { get; private set; }
    }
}