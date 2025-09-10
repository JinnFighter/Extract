using UnityEngine;

namespace Logic.ActionEvents
{
    public class ActionEventPropertyUpdate : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.PropertyUpdate;
        public EEntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public Vector2Int EntityPosition { get; set; }
        public EPropertyType PropertyType { get; set; }
        public int Value { get; set; }
    }
}