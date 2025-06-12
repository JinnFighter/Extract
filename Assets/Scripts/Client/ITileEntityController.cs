namespace Client
{
    public interface ITileEntityController
    {
        void Init(ITileEntityClient tileEntityClient, TileView tileView);
        void Terminate();
    }
}