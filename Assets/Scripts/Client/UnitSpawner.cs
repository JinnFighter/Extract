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
        private readonly Dictionary<ITileEntityClient, (ITileEntityController controller, TileView view)> _createdTiles = new();

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
            
            _createdUnits.Clear();

            var createdTiles = _createdTiles.ToList();
            foreach (var kvp in createdTiles)
            {
                DespawnTile(kvp.Key);
            }
            _createdTiles.Clear();
            
            _battleInstance.ModelClient.OnTileEntityAdded -= HandleTileEntityAdded;
            _battleInstance.ModelClient.OnUnitEntityAdded -= HandleUnitAdded;
        }

        public void SpawnTile(ITileEntityClient tileEntityClient)
        {
            var tileView = Instantiate(_tileViewPrefab, transform);
            tileView.transform.position = tileEntityClient.WorldPosition;
            tileView.Init(tileEntityClient);
            var controller = new TileEntityController();
            controller.Init(tileEntityClient, tileView);
            _createdTiles[tileEntityClient] = (controller,tileView);
        }

        public void DespawnTile(ITileEntityClient tileEntityClient)
        {
            if (!_createdTiles.Remove(tileEntityClient, out var value))
            {
                return;
            }
            
            value.controller.Terminate();
            Destroy(value.view.gameObject);
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
            Destroy(value.view.gameObject);
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