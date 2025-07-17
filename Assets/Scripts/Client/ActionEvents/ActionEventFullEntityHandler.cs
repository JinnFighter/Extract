using Logic.ActionEvents;
using UnityEngine;

namespace Client.ActionEvents
{
    public class ActionEventFullEntityHandler : BaseActionEventHandler<ActionEventFullEntity>
    {
        public override EActionEventType RequestedType => EActionEventType.FullEntity;

        protected override void HandleActionEventInner(BattleInstanceClient instance, ActionEventFullEntity gameEvent)
        {
            Debug.Log($"Full Entity Received, eventData: {gameEvent.EntityType}");
            switch (gameEvent.EntityType)
            {
                case EEntityType.Player:
                    instance.ModelClient.AddPlayer(gameEvent);
                    break;
                case EEntityType.Unit:
                    if (instance.ModelClient.UnitEntityModels.TryGetValue(gameEvent.Id, out _))
                    {
                        return;
                    }

                    instance.ModelClient.AddUnit(gameEvent);
                    break;
                case EEntityType.Tile:

                    if (instance.ModelClient.TileEntityModels.TryGetValue(gameEvent.TilePosition, out _))
                    {
                        return;
                    };
                    
                    instance.ModelClient.AddTile(gameEvent);
                    break;
                default:
                    return;
            }
        }
    }
}