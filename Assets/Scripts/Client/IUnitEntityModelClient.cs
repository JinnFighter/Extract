using System;
using System.Collections.Generic;
using Logic.ActionRequests;
using UnityEngine;

namespace Client
{
    public interface IUnitEntityModelClient
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public event Action<Vector2Int> OnPositionUpdated;
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public int TeamId { get; set; }
        public string NameId { get; set; }
        Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; }
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);

        void RemoveOption(ActionRequestOption option);
    }
}