namespace Logic.Systems
{
    public interface IOptionSystem
    {
        void Run(LogicModelServer modelServer, IActionEventSender actionEventSender);
    }
}