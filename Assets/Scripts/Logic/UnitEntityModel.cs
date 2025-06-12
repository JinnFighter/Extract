using System;
using System.Collections.Generic;
using Logic.ActionRequests;
using UnityEngine;

namespace Logic
{
    public class UnitEntityModel : BaseEntityModel, IUnitEntityModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public int TeamId { get; set; }
        public string NameId { get; set; }
        public event Action<ActionRequestOption> OnOptionAdded;
        public event Action<ActionRequestOption> OnOptionRemoved;
        public Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; } = new();
        
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