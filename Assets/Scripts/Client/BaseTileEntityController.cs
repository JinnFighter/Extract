namespace Client
{
    public abstract class BaseTileEntityController : ITileEntityController
    {
        protected ITileEntityClient Model { get; private set; }
        protected TileView View { get; private set; }
        public void Init(ITileEntityClient tileEntityClient, TileView tileView)
        {
            Model = tileEntityClient;
            View = tileView;
            
            InitInner();
        }

        public void Terminate()
        {
            TerminateInner();
        }

        protected virtual void InitInner()
        {
            
        }

        protected virtual void TerminateInner()
        {
            
        }
    }
}