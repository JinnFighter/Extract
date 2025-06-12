using System;

namespace Client
{
    public interface ITileEntitySelectorSystem
    {
        ITileEntityClient SelectedTile { get; }
        event Action<ITileEntityClient> OnTileEntitySelected;
        event Action<ITileEntityClient> OnTileEntityDeselected;
        public void Select(ITileEntityClient unitEntityModelClient);
    }
}