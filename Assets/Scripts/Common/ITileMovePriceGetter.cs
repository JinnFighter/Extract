using UnityEngine;

namespace Common
{
    public interface ITileMovePriceGetter
    {
        int GetPrice(Vector2Int position);
    }
}