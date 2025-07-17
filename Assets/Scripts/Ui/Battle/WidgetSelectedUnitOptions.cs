using Client;
using Reactivity;
using UiService.Code.Widgets;

namespace Ui.Battle
{
    public class WidgetSelectedUnitOptions : BaseUiWidget<WidgetSelectedUnitOptionsModel, WidgetViewSelectedUnitOptions>
    {
        private void HandleOptionRemoved(object sender, GenericPairEventArgs<int, WidgetActionOptionModel> e)
        {
            Close(e.Value);
        }

        private void HandleOptionAdded(object sender, GenericPairEventArgs<int, WidgetActionOptionModel> e)
        {
            OpenEmbedded<WidgetActionOption>(e.Value, View.Options[e.Key]);
        }

        protected override void InitInner()
        {
            Model.SelectorSystem.OnUnitEntitySelected += HandleUnitSelected;
            Model.SelectorSystem.OnUnitEntityDeselected += HandleUnitDeselected;
            SubscriptionAggregator.ListenListAddElement(Model.ActionOptions, HandleOptionAdded,true);
            SubscriptionAggregator.ListenListRemoveItem(Model.ActionOptions, HandleOptionRemoved);
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
            if (Model.SelectorSystem.SelectedUnit == null) DeselectUnit();
        }

        private void HandleUnitSelected(IUnitEntityModelClient obj)
        {
            SelectUnit(obj);
        }

        private void SelectUnit(IUnitEntityModelClient obj)
        {
            View.Panel.SetActive(true);
            for (var i = 0; i < obj.ActionRequestOptions.Count; i++)
            {
                Model.ActionOptions.Add(new WidgetActionOptionModel());
            }
        }

        private void DeselectUnit()
        {
            Model.ActionOptions.Clear();
            View.Panel.SetActive(false);
        }
    }
}