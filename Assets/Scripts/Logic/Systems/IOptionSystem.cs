namespace Logic.Systems
{
    public interface IOptionSystem
    {
        void Run(LogicModel model, IGameEventSender gameEventSender);
    }
}