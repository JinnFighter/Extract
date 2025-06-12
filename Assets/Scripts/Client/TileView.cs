using Logic;
using UnityEngine;

namespace Client
{
    public class TileView : MonoBehaviour
    {
        private ITileEntityClient _tileEntityClient;

        public void Init(ITileEntityClient tileEntityClient)
        {
            _tileEntityClient = tileEntityClient;
        }

        public void Terminate()
        {
            _tileEntityClient = null;
        }
    }
}