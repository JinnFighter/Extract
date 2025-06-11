using Logic.GameStateEvents;

namespace Logic.Systems
{
    public interface IInitializeSystem
    {
        void Initialize(GameSetupInfo gameSetupInfo, LogicModel logicModel, GameEventLogger gameEventLogger);
    }
}