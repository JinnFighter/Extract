using Logic;
using UnityEngine;

namespace Client
{
    public class TileView : MonoBehaviour
    {
        private ITileEntityModel _tileEntityModel;

        public void Init(ITileEntityModel tileEntityModel)
        {
            _tileEntityModel = tileEntityModel;
        }

        public void Terminate()
        {
            _tileEntityModel = null;
        }
    }
}