using System.Collections.Generic;
using Logic.Components;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class ActionEventFullEntity : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.FullEntity;

        public EEntityType EntityType;
        public int Id;
        public string NameId;
        public int OwnerId;
        public int NetId;
        public Vector2Int TilePosition;
        public Vector3 WorldPosition;
        public Dictionary<EPropertyType, int> Properties;
    }
}