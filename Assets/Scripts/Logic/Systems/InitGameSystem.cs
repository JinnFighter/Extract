using System.Collections.Generic;
using System.Linq;
using Common;
using Logic.ActionEvents;
using Logic.Descriptions;
using Logic.Entities;
using UnityEngine;

namespace Logic.Systems
{
    public class InitGameSystem : IInitializeSystem
    {
        public void Initialize(GameSetupInfo setupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger)
        {
            GenerateGameAndPlayersEntities(setupInfo, logicModelServer, actionEventLogger);

            GenerateTileEntities(setupInfo, logicModelServer, actionEventLogger);

            GenerateUnitEntities(setupInfo, logicModelServer, actionEventLogger);
        }

        private void GenerateGameAndPlayersEntities(GameSetupInfo setupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger)
        {
            actionEventLogger.LogGameEvent(new ActionEventGameStarted
            {
                IsInitEvent = true
            });
            var gameEntity = logicModelServer.GameEntityServer;
            gameEntity.Id = 1;
            gameEntity.PlayerIds.Clear();
            gameEntity.CurrentPlayerIndex = 0;
            foreach (var playerSetupInfo in setupInfo.PlayersSetupInfo)
            {
                var playerEntity = new PlayerEntityServer
                {
                    Id = gameEntity.Id + gameEntity.PlayerIds.Count + 1,
                    OwnerId = gameEntity.Id + gameEntity.PlayerIds.Count + 1,
                    NetId = playerSetupInfo.Id,
                };
                gameEntity.PlayerIds.Add(playerEntity.Id);
                logicModelServer.PlayerEntities.Add(playerEntity.Id, playerEntity);
                var playerProperties = new Dictionary<EPropertyType, int>();

                var state = new ActionEventFullEntity
                {
                    EntityType = EEntityType.Player,
                    Id = playerEntity.Id,
                    NetId = playerEntity.NetId,
                    OwnerId = playerEntity.OwnerId,
                    Properties = playerProperties,
                    IsInitEvent = true
                };
                actionEventLogger.LogGameEvent(state);
            }
        }

        private void GenerateTileEntities(GameSetupInfo setupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger)
        {
            foreach (var tileSetupInfo in setupInfo.TilesSetupInfo)
            {
                var tileEntity = new TileEntityServer
                {
                    Position = tileSetupInfo.TilePosition,
                    WorldPosition = tileSetupInfo.WorldPosition
                };
                logicModelServer.TileEntities.Add(tileEntity.Position, tileEntity);
                    
                var propertyDict = new Dictionary<EPropertyType, int>();
                var state = new ActionEventFullEntity
                {
                    EntityType = EEntityType.Tile,
                    Properties = propertyDict,
                    TilePosition = tileSetupInfo.TilePosition,
                    WorldPosition = tileSetupInfo.WorldPosition,
                    IsInitEvent = true
                };
                actionEventLogger.LogGameEvent(state);
            }
        }

        private void GenerateUnitEntities(GameSetupInfo setupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger)
        {
            var unitDescriptionLibrary = AutoResolver.Resolve<UnitDescriptionLibrary>();
            foreach (var unitSetupInfo in setupInfo.UnitsSetupInfo)
            {
                var unitDesc = unitDescriptionLibrary.Get(unitSetupInfo.NameId);
                var unitEntity = new UnitEntityServer
                {
                    Id = unitSetupInfo.Id,
                    OwnerId = unitSetupInfo.OwnerId,
                    NameId = unitSetupInfo.NameId,
                    Position = new Vector2Int((int)unitSetupInfo.SpawnPosition.x, (int)unitSetupInfo.SpawnPosition.y),
                };
                logicModelServer.UnitEntities.Add(unitSetupInfo.Id, unitEntity);
                logicModelServer.TileEntities[unitEntity.Position].OccupierId = unitEntity.Id;

                var propertyDict = new Dictionary<EPropertyType, int>();
                foreach (var propertyDesc in unitDesc.Properties.Where(propDesc => propDesc.IsInitialTag))
                {
                    propertyDict.Add(propertyDesc.PropertyType, propertyDesc.DefaultValue);
                }
                
                var state = new ActionEventFullEntity
                {
                    EntityType = EEntityType.Unit,
                    Id = unitEntity.Id,
                    NameId = unitEntity.NameId,
                    OwnerId = unitEntity.OwnerId,
                    Properties = propertyDict,
                    TilePosition = new Vector2Int((int)unitSetupInfo.SpawnPosition.x,
                        (int)unitSetupInfo.SpawnPosition.y),
                    WorldPosition = unitSetupInfo.SpawnPosition,
                    IsInitEvent = true
                };
                        
                actionEventLogger.LogGameEvent(state);
            }
        }
    }
}