using System.Collections.Generic;
using ArtificeToolkit.Attributes;
using Logic.ActionRequests;
using UnityEngine;

namespace Logic.Descriptions
{
    [CreateAssetMenu(fileName = "ActionDescription", menuName = "Scriptable Objects/ActionDescription")]
    public class ActionDescription : ScriptableObject
    {
        public EActionRequestType RequestType;

        [field: SerializeReference]
        [field: ForceArtifice]
        public List<ActionBuildStepDescription> BuildSteps { get; private set; }
    }
}