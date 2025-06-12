using System.Collections.Generic;
using Client.Descriptions;
using UnityEngine;
using VContainer;

namespace Client
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private BattleInstanceClient _battleInstance;
        [SerializeField] private TileView _tileViewPrefab;
        private readonly Dictionary<ITileEntityClient, TileView> _tileViews = new();
        private readonly Dictionary<IUnitEntityModelClient, UnitView> _unitViews = new();
        [Inject]
        private UnitViewLibrary _unitViewLibrary;

        public void Init()
        {
            _battleInstance.ModelClient.OnTileEntityAdded += HandleTileEntityAdded;
            _battleInstance.ModelClient.OnUnitEntityAdded += HandleUnitAdded;
        }

        public void Terminate()
        {
            foreach (var kvp in _unitViews) kvp.Value.Terminate();

            _unitViews.Clear();

            foreach (var kvp in _tileViews) kvp.Value.Terminate();

            _tileViews.Clear();
            _battleInstance.ModelClient.OnTileEntityAdded -= HandleTileEntityAdded;
            _battleInstance.ModelClient.OnUnitEntityAdded -= HandleUnitAdded;
        }

        public void SpawnTile(ITileEntityClient tileEntityClient)
        {
            var tileView = Instantiate(_tileViewPrefab, transform);
            tileView.transform.position = tileEntityClient.WorldPosition;
            tileView.Init(tileEntityClient);
            _tileViews.Add(tileEntityClient, tileView);
        }

        public void SpawnUnit(IUnitEntityModelClient unit)
        {
            var unitView = Instantiate(_unitViewLibrary.Get(unit.NameId).View, transform);
            unitView.transform.position = unit.WorldPosition;
            unitView.Init(unit);
            _unitViews.Add(unit, unitView);
        }

        private void HandleUnitAdded(IUnitEntityModelClient obj)
        {
            SpawnUnit(obj);
        }

        private void HandleTileEntityAdded(ITileEntityClient obj)
        {
            SpawnTile(obj);
        }
    }
}