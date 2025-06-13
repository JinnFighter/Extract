using Logic.ActionEvents;

namespace Logic.Systems
{
    public interface IInitializeSystem
    {
        void Initialize(GameSetupInfo gameSetupInfo, LogicModelServer logicModelServer, ActionEventLogger actionEventLogger);
    }
}