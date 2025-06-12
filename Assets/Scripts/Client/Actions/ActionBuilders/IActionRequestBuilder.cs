using System.Collections.Generic;
using Logic.ActionRequests;

namespace Client.Actions.ActionBuilders
{
    public interface IActionRequestBuilder
    {
        int OwnerId { get; }
        int CasterId { get; }
        List<IActionRequestBuildStep> Steps { get; }
        void Reset();
        IActionRequestBuilder SetOwner(int ownerId);
        IActionRequestBuilder SetCaster(int casterId);
        bool Validate();
        ActionRequest BuildAction();
    }
}
