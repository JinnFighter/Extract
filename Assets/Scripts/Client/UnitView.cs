using Logic;
using UnityEngine;

namespace Client
{
    public class UnitView : MonoBehaviour
    {
        private IUnitEntityModel _unitEntityModel;

        public void Init(IUnitEntityModel unitEntityModel)
        {
            _unitEntityModel = unitEntityModel;
        }

        public void Terminate()
        {
            _unitEntityModel = null;
        }
    }
}