namespace Logic.GameStateEvents
{
    public interface IGameStateEventHandler
    {
        void HandleGameEvent(BattleInstance instance, IGameStateEvent gameEvent);
    }
}