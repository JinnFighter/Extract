using System;

namespace Client
{
    public interface IEntitySelectorSystem
    {
        IUnitEntityModelClient SelectedUnit { get; }
        event Action<IUnitEntityModelClient> OnUnitEntitySelected;
        event Action<IUnitEntityModelClient> OnUnitEntityDeselected;
        public void SelectUnit(IUnitEntityModelClient unitEntityModelClient);
    }
}