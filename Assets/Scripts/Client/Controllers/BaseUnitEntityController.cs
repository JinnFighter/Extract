namespace Client.Controllers
{
    public abstract class BaseUnitEntityController : IUnitEntityController
    {
        protected IUnitEntityModelClient Model { get; private set; }
        protected UnitView View { get; private set; }

        #region IUnitEntityController Members

        public void Init(IUnitEntityModelClient unitEntityModelClient, UnitView unitView)
        {
            Model = unitEntityModelClient;
            View = unitView;
            InitInner();
        }

        public void Terminate()
        {
            TerminateInner();
        }

        #endregion

        protected virtual void InitInner()
        {
        }

        protected virtual void TerminateInner()
        {
        }
    }
}