using System;

namespace Client
{
    public class TileEntityEntitySelectorSystem : ITileEntitySelectorSystem
    {
        public ITileEntityClient SelectedTile { get; private set; }
        public event Action<ITileEntityClient> OnTileEntitySelected;
        public event Action<ITileEntityClient> OnTileEntityDeselected;
        public void Select(ITileEntityClient unitEntityModelClient)
        {
            if (unitEntityModelClient == SelectedTile)
            {
                DeselectInner(SelectedTile);
            }
            else
            {
                var selectedUnit = SelectedTile;
                SelectInner(unitEntityModelClient);
                if (selectedUnit != null)
                {
                    DeselectInner(selectedUnit);
                }
            }
        }
        
        private void SelectInner(ITileEntityClient unitEntityModelClient)
        {
            SelectedTile = unitEntityModelClient;
            OnTileEntitySelected?.Invoke(unitEntityModelClient);
        }

        private void DeselectInner(ITileEntityClient unitEntityModelClient)
        {
            SelectedTile = null;
            OnTileEntityDeselected?.Invoke(unitEntityModelClient);
        }
    }
}