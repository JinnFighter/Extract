using System;
using System.Collections.Generic;
using Logic;
using Logic.ActionRequests;

namespace Client
{
    public interface IPlayerEntityClient : IEntityClient
    {
        int Id { get; set; }
        int NetId { get; set; }
        Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; }
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);
        void RemoveOption(ActionRequestOption option);
    }
}