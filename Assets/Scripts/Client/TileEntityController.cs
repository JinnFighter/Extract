using System.Collections.Generic;

namespace Client
{
    public class TileEntityController : BaseTileEntityController
    {
        private readonly List<ITileEntityController> _tileEntityControllers = new()
        {
            new TileEntitySelectorController()
        };

        protected override void InitInner()
        {
            foreach (var tileEntityController in _tileEntityControllers)
            {
                tileEntityController.Init(Model, View);
            }
        }

        protected override void TerminateInner()
        {
            foreach (var tileEntityController in _tileEntityControllers)
            {
                tileEntityController.Terminate();
            }
        }
    }
}