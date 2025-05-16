using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class BattleInstance
    {
        private Dictionary<Vector2Int, ITileEntityModel> _tileEntityModels;
        
        public void Init(Dictionary<Vector2Int, ITileEntityModel> tileEntityModels)
        {
            _tileEntityModels = tileEntityModels;
        }

        public void Terminate()
        {
            _tileEntityModels.Clear();
            _tileEntityModels = null;
        }
    }
}
