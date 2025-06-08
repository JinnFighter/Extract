using System.Collections.Generic;
using FishNet.Broadcast;
using Logic.Components;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public struct GameStateEventFullEntity : IGameStateEvent, IBroadcast
    {
        public int EventId { get; set; }
        public int TurnNumber { get; set; }
        public EGameStateEventType EventType => EGameStateEventType.FullEntity;
        public EEntityType EntityType;
        public int Id;
        public string NameId;
        public int OwnerId;
        public Vector2Int TilePosition;
        public Vector3 WorldPosition;
        public Dictionary<EPropertyType, int> Properties;
    }
}