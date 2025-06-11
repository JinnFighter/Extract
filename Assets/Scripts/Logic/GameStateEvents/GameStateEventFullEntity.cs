using System.Collections.Generic;
using Logic.Components;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventFullEntity : GameStateEvent
    {
        public override EGameStateEventType GetEventType() => EGameStateEventType.FullEntity;

        public EEntityType EntityType;
        public int Id;
        public string NameId;
        public int OwnerId;
        public Vector2Int TilePosition;
        public Vector3 WorldPosition;
        public Dictionary<EPropertyType, int> Properties;
    }
}