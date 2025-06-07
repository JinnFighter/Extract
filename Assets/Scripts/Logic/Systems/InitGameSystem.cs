using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using Logic.Components;
using Logic.Descriptions;
using Logic.GameStateEvents;
using UnityEngine;

namespace Logic.Systems
{
    public class InitGameSystem : IEcsInitSystem
    {
        private readonly UnitDescriptionLibrary _unitDescriptionLibrary = null;
        private readonly IGameEventSender _gameEventSender = null;
        private readonly EcsFilter<GameSetupInfo> _filterGame = null;
        private readonly EcsWorld _world = null;
        public void Init()
        {
            foreach (var index in _filterGame)
            {
                var setupInfo = _filterGame.Get1(index);
                var gameEntity = _world.NewEntity();
                ref var componentGame = ref gameEntity.Get<ComponentGame>();
                componentGame.PlayerIds = new List<int>();
                foreach (var playerSetupInfo in setupInfo.PlayersSetupInfo)
                {
                    var playerEntity = _world.NewEntity();
                    ref var componentPlayer = ref playerEntity.Get<ComponentPlayer>();
                    componentPlayer.Id = playerSetupInfo.Id;
                    componentGame.PlayerIds.Add(playerSetupInfo.Id);
                    var playerProperties = new Dictionary<EPropertyType, int>();
                    if (playerSetupInfo.Id == setupInfo.StartingPlayerId)
                    {
                        componentGame.CurrentPlayerIndex = componentGame.PlayerIds.Count - 1;
                    }
                    
                    _gameEventSender.SendGameEvent(new GameStateEventFullEntity
                    {
                        EntityType = EEntityType.Player,
                        EventId = 0,
                        Id = playerSetupInfo.Id,
                        OwnerId = playerSetupInfo.Id,
                        Properties = playerProperties
                    });
                }

                foreach (var tileSetupInfo in setupInfo.TilesSetupInfo)
                {
                    var tileEntity = _world.NewEntity();
                    ref var componentTile = ref tileEntity.Get<ComponentTile>();
                    componentTile.TilePosition = tileSetupInfo.TilePosition;
                    componentTile.WorldPosition = tileSetupInfo.WorldPosition;
                    
                    var propertyDict = new Dictionary<EPropertyType, int>();
                    _gameEventSender.SendGameEvent(new GameStateEventFullEntity
                    {
                        EntityType = EEntityType.Tile,
                        EventId = 0,
                        Properties = propertyDict,
                        TilePosition = tileSetupInfo.TilePosition,
                        WorldPosition = tileSetupInfo.WorldPosition
                    });
                }

                foreach (var unitSetupInfo in setupInfo.UnitsSetupInfo)
                {
                    var unitDesc = _unitDescriptionLibrary.Get(unitSetupInfo.NameId);
                    var unitEntity = _world.NewEntity();
                    ref var componentUnit = ref unitEntity.Get<ComponentUnit>();
                    componentUnit.Id = unitSetupInfo.Id;
                    componentUnit.OwnerId = unitSetupInfo.OwnerId;

                    var propertyDict = new Dictionary<EPropertyType, int>();
                    foreach (var propertyDesc in unitDesc.Properties.Where(propDesc => propDesc.IsInitialTag))
                    {
                        var component = _unitDescriptionLibrary.GetPropertyComponent(propertyDesc.PropertyType, unitEntity);
                        component.Value = propertyDesc.DefaultValue;
                        propertyDict.Add(propertyDesc.PropertyType, component.Value);
                    }
                    
                    _gameEventSender.SendGameEvent(new GameStateEventFullEntity
                    {
                        EntityType = EEntityType.Unit,
                        EventId = 0,
                        Id = componentUnit.Id,
                        NameId = unitDesc.NameId,
                        OwnerId = componentUnit.OwnerId,
                        Properties = propertyDict,
                        TilePosition = new Vector2Int((int)unitSetupInfo.SpawnPosition.x, (int)unitSetupInfo.SpawnPosition.y),
                        WorldPosition = unitSetupInfo.SpawnPosition
                    });
                }
            }
        }
    }
}