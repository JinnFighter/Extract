using System;
using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.Components;

namespace Client
{
    public interface IPlayerEntityClient
    {
        int Id { get; set; }
        int NetId { get; set; }
        Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; }
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);
        void RemoveOption(ActionRequestOption option);
        void Set(EPropertyType type, int value);
    }
}