using Common;
using UnityEngine;

namespace Client.Pathfinding
{
    public class ClientTileMovePriceGetter : ITileMovePriceGetter
    {
        private readonly LogicModelClient _gameFieldModel;

        public ClientTileMovePriceGetter(BattleInstanceClient battleInstanceModel)
        {
            _gameFieldModel = battleInstanceModel.ModelClient;
        }

        public int GetPrice(Vector2Int position)
        {
            return _gameFieldModel.TileEntityModels[position].OccupierId >= 0 ? -1 : 0;
        }
    }
}
