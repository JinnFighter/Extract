using Logic.GameStateEvents;
using UnityEngine;

namespace Client.GameStateEvents
{
    public class ActionEventFullEntityHandler : IActionEventHandler
    {
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != EActionEventType.FullEntity)
            {
                return;
            }

            var eventData = gameEvent as ActionEventFullEntity;
            Debug.Log($"eventData: {eventData.EntityType}");
            switch (eventData.EntityType)
            {
                case EEntityType.Player:
                    instance.ModelClient.AddPlayer(eventData);
                    break;
                case EEntityType.Unit:
                    if (instance.ModelClient.UnitEntityModels.TryGetValue(eventData.Id, out _))
                    {
                        return;
                    }

                    instance.ModelClient.AddUnit(eventData);
                    break;
                case EEntityType.Tile:

                    if (instance.ModelClient.TileEntityModels.TryGetValue(eventData.TilePosition, out _))
                    {
                        return;
                    };
                    
                    instance.ModelClient.AddTile(eventData);
                    break;
                default:
                    return;
            }
        }
    }
}