using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public class ActionEventPositionChangedHandler : IActionEventHandler
    {
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != EActionEventType.PositionChanged)
            {
                return;
            }
                
            var positionEvent = (ActionEventPositionChanged)gameEvent;
            var oldTile = instance.ModelClient.TileEntityModels[positionEvent.OldPosition];
            var nextTile = instance.ModelClient.TileEntityModels[positionEvent.NewPosition];
            var unit = instance.ModelClient.UnitEntityModels[positionEvent.UnitId];
            oldTile.OccupierId = -1;
            nextTile.OccupierId = unit.Id;
            unit.Position = positionEvent.NewPosition;
        }
    }
}