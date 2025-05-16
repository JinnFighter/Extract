using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Init
{
    public class InitTestBattle : MonoBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        private readonly BattleInstance _battleInstance = new();

        private void Start()
        {
            var tileEntityModels = SetupGameField();
            var unitEntityModels = SetupUnits();
            _battleInstance.Init(tileEntityModels, unitEntityModels);
        }

        private void OnDestroy()
        {
            _battleInstance.Terminate();
        }

        private Dictionary<Vector2Int, ITileEntityModel> SetupGameField()
        {
            var dict = new Dictionary<Vector2Int, ITileEntityModel>();
            foreach (var kvp in _gameFieldSetup.TilesSetup)
            {
                var tileEntityModel = new TileEntityModel
                {
                    Position = kvp.Key,
                    IsWalkable = kvp.Value.Walkable,
                    WorldPosition = kvp.Value.transform.position
                };
                dict[kvp.Key] = tileEntityModel;
            }

            return dict;
        }

        private Dictionary<int, IUnitEntityModel> SetupUnits()
        {
            var dict = new Dictionary<int, IUnitEntityModel>();
            return dict;
        }
    }
}