namespace UiService.Code.Widgets
{
    public interface IUiScreenState : IUiWidget
    {
        void SetRouter(IStateRouter stateRouter);
    }
}