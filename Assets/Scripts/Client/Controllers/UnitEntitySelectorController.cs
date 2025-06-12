using Common;
using UnityEngine;

namespace Client.Controllers
{
    public class UnitEntitySelectorController : BaseUnitEntityController
    {
        private IEntitySelectorSystem _entitySelectorSystem;

        protected override void InitInner()
        {
            _entitySelectorSystem = AutoResolver.Resolve<IEntitySelectorSystem>();
            View.ClickableGameObject.OnObjectClicked.AddListener(HandleClickableGameObjectClicked);
        }

        protected override void TerminateInner()
        {
            View.ClickableGameObject.OnObjectClicked.AddListener(HandleClickableGameObjectClicked);
        }

        private void HandleClickableGameObjectClicked()
        {
            Debug.Log($"Clicked on Unit id {Model.Id}");
            _entitySelectorSystem.SelectUnit(Model);
        }
    }
}