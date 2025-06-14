using System;

namespace Logic.Descriptions
{
    [Serializable]
    public class ActionBuildStepDescriptionSelectTileMove : ActionBuildStepDescription
    {
        public override EActionBuildStepType StepType => EActionBuildStepType.SelectTileMovement;
    }
}