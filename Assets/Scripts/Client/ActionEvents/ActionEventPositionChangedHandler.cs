using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public class ActionEventPositionChangedHandler : BaseActionEventHandler<ActionEventPositionChanged>
    {
        public override EActionEventType RequestedType => EActionEventType.PositionChanged;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventPositionChanged gameEvent)
        {
            var oldTile = instance.ModelClient.TileEntityModels[gameEvent.OldPosition];
            var nextTile = instance.ModelClient.TileEntityModels[gameEvent.NewPosition];
            var unit = instance.ModelClient.UnitEntityModels[gameEvent.UnitId];
            oldTile.OccupierId = -1;
            nextTile.OccupierId = unit.Id;
            unit.WorldPosition = instance.ModelClient.TileEntityModels[gameEvent.NewPosition].WorldPosition;
            unit.Position = gameEvent.NewPosition;
        }
    }
}