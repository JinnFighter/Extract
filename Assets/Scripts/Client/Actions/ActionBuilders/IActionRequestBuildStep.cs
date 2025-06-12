using Logic.Descriptions;

namespace Client.Actions.ActionBuilders
{
    public interface IActionRequestBuildStep
    {
        EActionBuildStepType StepType { get; }
    }
}