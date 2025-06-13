using UnityEngine;

namespace Logic.ActionEvents
{
    public class ActionEventPositionChanged : ActionEvent
    {
        public override EActionEventType EventType => EActionEventType.PositionChanged;
        public Vector2Int OldPosition;
        public Vector2Int NewPosition;
    }
}