using System.Collections.Generic;
using System.Linq;
using Common;
using Logic.Components;
using Logic.Descriptions;
using Logic.Entities;
using Logic.GameStateEvents;
using UnityEngine;

namespace Logic.Systems
{
    public class InitGameSystem : IInitializeSystem
    {
        public void Initialize(GameSetupInfo setupInfo, LogicModel logicModel, GameEventLogger gameEventLogger)
        {
            GenerateGameAndPlayersEntities(setupInfo, logicModel, gameEventLogger);

            GenerateTileEntities(setupInfo, logicModel, gameEventLogger);

            GenerateUnitEntities(setupInfo, logicModel, gameEventLogger);
        }

        private void GenerateGameAndPlayersEntities(GameSetupInfo setupInfo, LogicModel logicModel, GameEventLogger gameEventLogger)
        {
            gameEventLogger.LogGameEvent(new GameStateEventGameStarted());
            var gameEntity = logicModel.GameEntity;
            gameEntity.PlayerIds.Clear();
            gameEntity.CurrentPlayerIndex = 0;
            foreach (var playerSetupInfo in setupInfo.PlayersSetupInfo)
            {
                var playerEntity = new PlayerEntity
                {
                    Id = playerSetupInfo.Id,
                    OwnerId = playerSetupInfo.Id
                };
                gameEntity.PlayerIds.Add(playerSetupInfo.Id);
                logicModel.PlayerEntities.Add(playerEntity.Id, playerEntity);
                var playerProperties = new Dictionary<EPropertyType, int>();

                var state = new GameStateEventFullEntity
                {
                    EntityType = EEntityType.Player,
                    Id = playerSetupInfo.Id,
                    OwnerId = playerSetupInfo.Id,
                    Properties = playerProperties
                };
                gameEventLogger.LogGameEvent(state);
            }
        }

        private void GenerateTileEntities(GameSetupInfo setupInfo, LogicModel logicModel, GameEventLogger gameEventLogger)
        {
            foreach (var tileSetupInfo in setupInfo.TilesSetupInfo)
            {
                var tileEntity = new TileEntity
                {
                    Position = tileSetupInfo.TilePosition,
                    WorldPosition = tileSetupInfo.WorldPosition
                };
                logicModel.TileEntities.Add(tileEntity.Position, tileEntity);
                    
                var propertyDict = new Dictionary<EPropertyType, int>();
                var state = new GameStateEventFullEntity
                {
                    EntityType = EEntityType.Tile,
                    Properties = propertyDict,
                    TilePosition = tileSetupInfo.TilePosition,
                    WorldPosition = tileSetupInfo.WorldPosition
                };
                gameEventLogger.LogGameEvent(state);
            }
        }

        private void GenerateUnitEntities(GameSetupInfo setupInfo, LogicModel logicModel, GameEventLogger gameEventLogger)
        {
            var unitDescriptionLibrary = AutoResolver.Resolve<UnitDescriptionLibrary>();
            foreach (var unitSetupInfo in setupInfo.UnitsSetupInfo)
            {
                var unitDesc = unitDescriptionLibrary.Get(unitSetupInfo.NameId);
                var unitEntity = new UnitEntity
                {
                    Id = unitSetupInfo.Id,
                    OwnerId = unitSetupInfo.OwnerId,
                    NameId = unitSetupInfo.NameId,
                };
                logicModel.UnitEntities.Add(unitSetupInfo.Id, unitEntity);

                var propertyDict = new Dictionary<EPropertyType, int>();
                foreach (var propertyDesc in unitDesc.Properties.Where(propDesc => propDesc.IsInitialTag))
                {
                    propertyDict.Add(propertyDesc.PropertyType, propertyDesc.DefaultValue);
                }
                
                var state = new GameStateEventFullEntity
                {
                    EntityType = EEntityType.Unit,
                    Id = unitEntity.Id,
                    NameId = unitEntity.NameId,
                    OwnerId = unitEntity.OwnerId,
                    Properties = propertyDict,
                    TilePosition = new Vector2Int((int)unitSetupInfo.SpawnPosition.x,
                        (int)unitSetupInfo.SpawnPosition.y),
                    WorldPosition = unitSetupInfo.SpawnPosition
                };
                        
                gameEventLogger.LogGameEvent(state);
            }
        }
    }
}