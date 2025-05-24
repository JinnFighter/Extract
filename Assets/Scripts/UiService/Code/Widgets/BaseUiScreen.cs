using MVVM;

namespace UiService.Code.Widgets
{
    public abstract class BaseUiScreen<TModel, TView> : BaseUiWidget<TModel, TView>, IUiScreen
        where TModel : IModel where TView : UiView
    {
    }
}