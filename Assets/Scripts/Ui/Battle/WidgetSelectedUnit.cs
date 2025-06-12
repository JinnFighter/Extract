using Client;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class WidgetSelectedUnit : BaseUiWidget<ModelWidgetSelectedUnit, ViewWidgetSelectedUnit>
    {
        protected override void InitInner()
        {
            Model.SelectorSystem.OnUnitEntitySelected += HandleUnitSelected;
            Model.SelectorSystem.OnUnitEntityDeselected += HandleUnitDeselected;
            if (Model.SelectorSystem.SelectedUnit != null)
            {
                SelectUnit(Model.SelectorSystem.SelectedUnit);
            }
        }
        
        protected override void TerminateInner()
        {
            DeselectUnit();
            Model.SelectorSystem.OnUnitEntitySelected -= HandleUnitSelected;
            Model.SelectorSystem.OnUnitEntityDeselected -= HandleUnitDeselected;
        }

        private void HandleUnitDeselected(IUnitEntityModelClient obj)
        {
            if (Model.SelectorSystem.SelectedUnit == null)
            {
                DeselectUnit();
            }
        }

        private void HandleUnitSelected(IUnitEntityModelClient obj)
        {
            SelectUnit(obj);
        }

        private void SelectUnit(IUnitEntityModelClient obj)
        {
            View.UnitInfoPanel.SetActive(true);
        }

        private void DeselectUnit()
        {
            View.UnitInfoPanel.SetActive(false);
        }
    }
}