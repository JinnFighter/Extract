using Client;
using MVVM;
using Reactivity;

namespace Ui.Battle
{
    public class WidgetSelectedUnitOptionsModel : IModel
    {
        public IUnitEntitySelectorSystem SelectorSystem { get; set; }
        public ReactiveList<WidgetActionOptionModel> ActionOptions { get; set; } = new();
    }
}