using System;
using System.Collections.Generic;
using Logic.ActionRequests;

namespace Logic
{
    public interface IPlayerEntityModel
    {
        int Id { get; set; }
        Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; }
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);

        void RemoveOption(ActionRequestOption option);
    }
}