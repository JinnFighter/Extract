using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class BattleInstance
    {
        private Dictionary<Vector2Int, ITileEntityModel> _tileEntityModels;
        private Dictionary<int, IUnitEntityModel> _unitEntityModels;

        public void Init(Dictionary<Vector2Int, ITileEntityModel> tileEntityModels,
            Dictionary<int, IUnitEntityModel> unitEntityModels)
        {
            _tileEntityModels = tileEntityModels;
            _unitEntityModels = unitEntityModels;
        }

        public void Terminate()
        {
            _unitEntityModels.Clear();
            _unitEntityModels = null;
            _tileEntityModels.Clear();
            _tileEntityModels = null;
        }
    }
}