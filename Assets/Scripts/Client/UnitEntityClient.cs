using System;
using System.Collections.Generic;
using Logic.ActionRequests;
using UnityEngine;

namespace Client
{
    public class UnitEntityClient : BaseEntityClient, IUnitEntityModelClient
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public event Action<Vector2Int> OnPositionUpdated;
        public event Action<Vector3> OnWorldPositionUpdated;

        public Vector2Int Position
        {
            get => _position;
            set
            {
                if (_position == value)
                {
                    return;
                }
                
                _position = value;
                OnPositionUpdated?.Invoke(_position);
            }
        }

        public Vector3 WorldPosition
        {
            get => _worldPosition;
            set
            {
                if (_worldPosition == value)
                {
                    return;
                }
                
                _worldPosition = value;
                OnWorldPositionUpdated?.Invoke(_worldPosition);
            }
        }
        public int TeamId { get; set; }
        public string NameId { get; set; }
        public event Action<ActionRequestOption> OnOptionAdded;
        public event Action<ActionRequestOption> OnOptionRemoved;
        public Dictionary<EActionRequestType, ActionRequestOption> ActionRequestOptions { get; } = new();

        private Vector2Int _position;
        private Vector3 _worldPosition;
        
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