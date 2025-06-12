using Client;
using MVVM;

namespace Ui.Battle
{
    public class ModelWidgetSelectedUnit : IModel
    {
        public IEntitySelectorSystem SelectorSystem { get; set; }
    }
}