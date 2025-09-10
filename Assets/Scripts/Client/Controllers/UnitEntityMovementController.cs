using UnityEngine;

namespace Client.Controllers
{
    public class UnitEntityMovementController : BaseUnitEntityController
    {
        protected override void InitInner()
        {
            Model.OnPositionUpdated += HandlePositionUpdated;
            Model.OnWorldPositionUpdated += HandleWorldPositionUpdated;
        }

        protected override void TerminateInner()
        {
            Model.OnPositionUpdated -= HandlePositionUpdated;
            Model.OnWorldPositionUpdated -= HandleWorldPositionUpdated;
        }
        
        private void HandlePositionUpdated(Vector2Int obj)
        {
            View.transform.position = Model.WorldPosition;
        }
        
        private void HandleWorldPositionUpdated(Vector3 obj)
        {
            View.transform.position = obj;
        }
    }
}