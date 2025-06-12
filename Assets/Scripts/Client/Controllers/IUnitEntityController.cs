namespace Client.Controllers
{
    public interface IUnitEntityController
    {
        public void Init(IUnitEntityModelClient unitEntityModelClient, UnitView unitView);
        public void Terminate();
    }
}