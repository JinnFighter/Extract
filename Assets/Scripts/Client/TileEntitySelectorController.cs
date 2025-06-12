using Common;
using UnityEngine;

namespace Client
{
    public class TileEntitySelectorController : BaseTileEntityController
    {
        private ITileEntitySelectorSystem _unitEntitySelectorSystem;

        protected override void InitInner()
        {
            _unitEntitySelectorSystem = AutoResolver.Resolve<ITileEntitySelectorSystem>();
            View.ClickableGameObject.OnObjectClicked.AddListener(HandleClickableGameObjectClicked);
        }

        protected override void TerminateInner()
        {
            View.ClickableGameObject.OnObjectClicked.AddListener(HandleClickableGameObjectClicked);
        }

        private void HandleClickableGameObjectClicked()
        {
            Debug.Log($"Clicked on Tile Position {Model.Position}");
            _unitEntitySelectorSystem.Select(Model);
        }
    }
}