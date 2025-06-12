using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TileView : MonoBehaviour
    {
        [field: SerializeField] public ClickableGameObject ClickableGameObject { get; private set; }
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