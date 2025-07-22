using System;
using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public class ActionEventPropertyUpdateHandler : BaseActionEventHandler<ActionEventPropertyUpdate>
    {
        public override EActionEventType RequestedType => EActionEventType.PropertyUpdate;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventPropertyUpdate gameEvent)
        {
            IEntityClient entity = gameEvent.EntityType switch
            {
                EEntityType.Player => gameEvent.EntityId == 2 ? instance.ModelClient.Player1Entity : instance.ModelClient.Player2Entity,
                EEntityType.Unit => instance.ModelClient.UnitEntityModels[gameEvent.EntityId],
                EEntityType.Tile => instance.ModelClient.TileEntityModels[gameEvent.EntityPosition],
                EEntityType.None => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (entity == null)
            {
                throw new Exception("Invalid entity type");
            }
            
            entity.Set(gameEvent.PropertyType, gameEvent.Value);
        }
    }
}