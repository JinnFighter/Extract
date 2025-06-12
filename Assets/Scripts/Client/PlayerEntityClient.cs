using System;
using System.Collections.Generic;
using Logic.ActionRequests;

namespace Client
{
    public class PlayerEntityClient : BaseEntityClient, IPlayerEntityClient
    {
        public Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; } = new();
        public int Id { get; set; }
        public int NetId { get; set; }
        public event Action<ActionRequestOption> OnOptionAdded;
        public event Action<ActionRequestOption> OnOptionRemoved;

        public void AddOption(ActionRequestOption option)
        {
            if (ActionRequestOptions.TryGetValue(option.RequestType, out _)) return;

            ActionRequestOptions.Add(option.RequestType, option);
            OnOptionAdded?.Invoke(option);
        }

        public void RemoveOption(ActionRequestOption option)
        {
            if (!ActionRequestOptions.Remove(option.RequestType, out _)) return;

            OnOptionRemoved?.Invoke(option);
        }
    }
}