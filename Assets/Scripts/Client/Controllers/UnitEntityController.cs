using System.Collections.Generic;

namespace Client.Controllers
{
    public class UnitEntityController : BaseUnitEntityController
    {
        private readonly List<IUnitEntityController> _unitControllers = new()
        {
            new UnitEntitySelectorController(),
            new UnitEntityMovementController(),
        };

        protected override void InitInner()
        {
            foreach (var controller in _unitControllers)
            {
                controller.Init(Model, View);
            }
        }

        protected override void TerminateInner()
        {
            foreach (var controller in _unitControllers)
            {
                controller.Terminate();
            }
        }
    }
}