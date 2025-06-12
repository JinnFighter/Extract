using Client;
using MVVM;

namespace Ui.Battle
{
    public class ModelWidgetSelectedUnit : IModel
    {
        public IUnitEntitySelectorSystem SelectorSystem { get; set; }
    }
}