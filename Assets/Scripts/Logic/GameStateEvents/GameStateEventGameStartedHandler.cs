using Logic.States;

namespace Logic.GameStateEvents
{
    public class GameStateEventGameStartedHandler : IGameStateEventHandler
    {
        public void HandleGameEvent(BattleInstance instance, GameStateEvent gameEvent)
        {
            if (gameEvent.GetEventType() != EGameStateEventType.GameStart) return;
            
            instance.StateMachine.ChangeState(EBattleStateId.Init);
        }
    }
}