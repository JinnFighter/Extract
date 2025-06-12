namespace Logic.Systems
{
    public interface IOptionSystem
    {
        void Run(LogicModelServer modelServer, IGameEventSender gameEventSender);
    }
}