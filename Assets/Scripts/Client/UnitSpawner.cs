using System.Collections.Generic;
using System.Linq;
using Client.Controllers;
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

        private readonly Dictionary<IUnitEntityModelClient, (IUnitEntityController controller, UnitView view)>
            _createdUnits = new();
        
        [Inject]
        private UnitViewLibrary _unitViewLibrary;

        public void Init()
        {
            _battleInstance.ModelClient.OnTileEntityAdded += HandleTileEntityAdded;
            _battleInstance.ModelClient.OnUnitEntityAdded += HandleUnitAdded;
        }

        public void Terminate()
        {
            var createdUnits = _createdUnits.ToList();
            foreach (var kvp in createdUnits)
            {
                DespawnUnit(kvp.Key);
            }

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
            var controller = new UnitEntityController();
            controller.Init(unit, unitView);
            _createdUnits[unit] = (controller, unitView);
        }

        public void DespawnUnit(IUnitEntityModelClient unit)
        {
            if (!_createdUnits.Remove(unit, out var value))
            {
                return;
            }
            
            value.controller.Terminate();
            Destroy(value.view);
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