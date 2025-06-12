using System;

namespace Client
{
    public interface IUnitEntitySelectorSystem
    {
        IUnitEntityModelClient SelectedUnit { get; }
        event Action<IUnitEntityModelClient> OnUnitEntitySelected;
        event Action<IUnitEntityModelClient> OnUnitEntityDeselected;
        public void Select(IUnitEntityModelClient unitEntityModelClient);
    }
}