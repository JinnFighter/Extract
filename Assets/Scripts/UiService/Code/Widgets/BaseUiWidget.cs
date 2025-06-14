using MVVM;
using Reactivity;

namespace UiService.Code.Widgets
{
    public abstract class BaseUiWidget<TModel, TView> : IUiWidget where TModel : IModel where TView : UiView
    {
        protected readonly SubscriptionAggregator SubscriptionAggregator = new();
        protected IUiService UiService { get; private set; }
        protected TModel Model { get; private set; }
        protected TView View { get; private set; }
        public bool IsEnabled { get; private set; } = true;

        public void Init()
        {
            RegisterChildWidgets();
            InitInner();
        }

        public void Terminate()
        {
            SubscriptionAggregator.Unsubscribe();
            TerminateInner();
        }

        public void Setup(IModel model, IUiView view, IUiService uiService)
        {
            Model = (TModel)model;
            View = (TView)view;
            UiService = uiService;
        }

        public void Enable()
        {
            if (IsEnabled)
                return;

            EnableInner();
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;

            DisableInner();
            IsEnabled = false;
        }

        protected virtual void InitInner()
        {
        }

        protected virtual void TerminateInner()
        {
        }

        protected virtual void RegisterChildWidgets()
        {
        }

        protected virtual void EnableInner()
        {
            View.gameObject.SetActive(true);
        }

        protected virtual void DisableInner()
        {
            View.gameObject.SetActive(false);
        }

        protected void RegisterChildWidget<TWidget>(IModel model, UiView view)
            where TWidget : IUiWidget
        {
            UiService.OpenEmbedded<TWidget>(model, view, Model);
        }

        protected void OpenEmbedded<TWidget>(IModel model, UiView view)
            where TWidget : IUiWidget
        {
            UiService.OpenEmbedded<TWidget>(model, view, Model);
        }

        protected void Close(IModel model)
        {
            UiService.Close(model);
        }
    }
}