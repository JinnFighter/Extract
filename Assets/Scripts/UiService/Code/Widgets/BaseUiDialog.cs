using MVVM;

namespace UiService.Code.Widgets
{
    public abstract class BaseUiDialog<TModel, TView> : BaseUiWidget<TModel, TView>, IUiDialog
        where TModel : IModel where TView : UiView
    {
    }
}