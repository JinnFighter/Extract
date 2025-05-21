using System;
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
        private Dictionary<ITileEntityModel, TileView> _tileViews = new Dictionary<ITileEntityModel, TileView>();
        private Dictionary<IUnitEntityModel, UnitView> _unitViews = new Dictionary<IUnitEntityModel, UnitView>();

        public void Init()
        {
            _battleInstance.OnGameSetup += HandleGameSetup;
        }

        public void Terminate()
        {
            foreach (var kvp in _unitViews)
            {
                kvp.Value.Terminate();
            }
            
            _unitViews.Clear();

            foreach (var kvp in _tileViews)
            {
                kvp.Value.Terminate();
            }
            
            _tileViews.Clear();
            _battleInstance.OnGameSetup -= HandleGameSetup;
        }

        private void HandleGameSetup(GameSetupInfo obj)
        {
            foreach (var kvp in _battleInstance.TileEntityModels)
            {
                var tileView = Instantiate(_tileViewPrefab, transform);
                tileView.transform.position = kvp.Value.WorldPosition;
                tileView.Init(kvp.Value);
                _tileViews.Add(kvp.Value, tileView);
            }

            foreach (var kvp in _battleInstance.UnitEntityModels)
            {
                var unitView = Instantiate(_unitViewPrefab, transform);
                unitView.transform.position = kvp.Value.WorldPosition;
                unitView.Init(kvp.Value);
                _unitViews.Add(kvp.Value, unitView);
            }
        }
    }
}