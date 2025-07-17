using MVVM;
using Reactivity;

namespace Ui.Battle
{
    public class WidgetActionOptionModel : IModel
    {
        private readonly ReactiveProperty<string> _actionName = new ();
        public IReactiveProperty<string> ActionName => _actionName;
    }
}