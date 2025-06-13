using System;

namespace Logic.Descriptions
{
    [Serializable]
    public abstract class ActionBuildStepDescription
    {
        public abstract EActionBuildStepType StepType { get; }
    }
}