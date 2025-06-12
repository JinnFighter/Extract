using System;

namespace Client
{
    public class UnitEntitySelectorSystem : IUnitEntitySelectorSystem
    {
        public IUnitEntityModelClient SelectedUnit { get; private set; }
        public event Action<IUnitEntityModelClient> OnUnitEntitySelected;
        public event Action<IUnitEntityModelClient> OnUnitEntityDeselected;
        public void Select(IUnitEntityModelClient unitEntityModelClient)
        {
            if (unitEntityModelClient == SelectedUnit)
            {
                DeselectUnitInner(SelectedUnit);
            }
            else
            {
                var selectedUnit = SelectedUnit;
                SelectUnitInner(unitEntityModelClient);
                if (selectedUnit != null)
                {
                    DeselectUnitInner(selectedUnit);
                }
            }
        }

        private void SelectUnitInner(IUnitEntityModelClient unitEntityModelClient)
        {
            SelectedUnit = unitEntityModelClient;
            OnUnitEntitySelected?.Invoke(unitEntityModelClient);
        }

        private void DeselectUnitInner(IUnitEntityModelClient unitEntityModelClient)
        {
            SelectedUnit = null;
            OnUnitEntityDeselected?.Invoke(unitEntityModelClient);
        }
    }
}