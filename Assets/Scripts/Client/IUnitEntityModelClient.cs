using System;
using Logic.ActionRequests;
using UnityEngine;

namespace Client
{
    public interface IUnitEntityModelClient
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Vector2Int Position { get; set; }
        public Vector3 WorldPosition { get; set; }
        public int TeamId { get; set; }
        public string NameId { get; set; }
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);

        void RemoveOption(ActionRequestOption option);
    }
}