using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventFullEntityHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, GameStateEvent gameEvent)
        {
            if (gameEvent.EventType != EGameStateEventType.FullEntity)
            {
                return;
            }

            var eventData = gameEvent as GameStateEventFullEntity;
            Debug.Log($"eventData: {eventData.EntityType}");
            switch (eventData.EntityType)
            {
                case EEntityType.Player:
                    instance.Model.AddPlayer(eventData);
                    break;
                case EEntityType.Unit:
                    if (instance.Model.UnitEntityModels.TryGetValue(eventData.Id, out _))
                    {
                        return;
                    }

                    instance.Model.AddUnit(eventData);
                    break;
                case EEntityType.Tile:

                    if (instance.Model.TileEntityModels.TryGetValue(eventData.TilePosition, out _))
                    {
                        return;
                    };
                    
                    instance.Model.AddTile(eventData);
                    break;
                default:
                    return;
            }
        }
    }
}