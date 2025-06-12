using Logic;
using UnityEngine;

namespace Client
{
    public class UnitView : MonoBehaviour
    {
        private IUnitEntityModelClient _unitEntityModelClient;

        public void Init(IUnitEntityModelClient unitEntityModelClient)
        {
            _unitEntityModelClient = unitEntityModelClient;
        }

        public void Terminate()
        {
            _unitEntityModelClient = null;
        }
    }
}