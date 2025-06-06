using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Client
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private BattleInstance _battleInstance;
        [SerializeField] private TileView _tileViewPrefab;
        [SerializeField] private UnitView _unitViewPrefab;
        private readonly Dictionary<ITileEntityModel, TileView> _tileViews = new();
        private readonly Dictionary<IUnitEntityModel, UnitView> _unitViews = new();

        public void Init()
        {
            _battleInstance.Model.OnTileEntityAdded += HandleTileEntityAdded;
            _battleInstance.Model.OnUnitEntityAdded += HandleUnitAdded;
        }

        public void Terminate()
        {
            foreach (var kvp in _unitViews) kvp.Value.Terminate();

            _unitViews.Clear();

            foreach (var kvp in _tileViews) kvp.Value.Terminate();

            _tileViews.Clear();
            _battleInstance.Model.OnTileEntityAdded -= HandleTileEntityAdded;
            _battleInstance.Model.OnUnitEntityAdded -= HandleUnitAdded;
        }

        public void SpawnTile(ITileEntityModel tileEntityModel)
        {
            var tileView = Instantiate(_tileViewPrefab, transform);
            tileView.transform.position = tileEntityModel.WorldPosition;
            tileView.Init(tileEntityModel);
            _tileViews.Add(tileEntityModel, tileView);
        }

        public void SpawnUnit(IUnitEntityModel unit)
        {
            var unitView = Instantiate(_unitViewPrefab, transform);
            unitView.transform.position = unit.WorldPosition;
            unitView.Init(unit);
            _unitViews.Add(unit, unitView);
        }

        private void HandleUnitAdded(IUnitEntityModel obj)
        {
            SpawnUnit(obj);
        }

        private void HandleTileEntityAdded(ITileEntityModel obj)
        {
            SpawnTile(obj);
        }
    }
}