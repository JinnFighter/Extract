using Logic.ActionEvents;

namespace Client.ActionEvents
{
    public abstract class BaseActionEventHandler<T> : IActionEventHandler where T : ActionEvent
    {
        public abstract EActionEventType RequestedType { get; }
        public void HandleActionEvent(BattleInstanceClient instance, ActionEvent gameEvent)
        {
            if (gameEvent.EventType != RequestedType)
            {
                return;
            }
            
            HandleActionEventInner(instance, gameEvent as T);
        }

        protected virtual void HandleActionEventInner(BattleInstanceClient instance, T gameEvent)
        {
        }
    }
}