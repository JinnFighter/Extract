using UnityEngine;

namespace Client.Controllers
{
    public class UnitEntityMovementController : BaseUnitEntityController
    {
        protected override void InitInner()
        {
            Model.OnPositionUpdated += HandlePositionUpdated;
        }

        protected override void TerminateInner()
        {
            Model.OnPositionUpdated -= HandlePositionUpdated;
        }
        
        private void HandlePositionUpdated(Vector2Int obj)
        {
            View.transform.position = Model.WorldPosition;
        }
    }
}