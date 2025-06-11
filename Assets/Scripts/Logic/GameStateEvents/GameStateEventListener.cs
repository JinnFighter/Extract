using System.Collections.Generic;

namespace Logic.GameStateEvents
{
    public class GameStateEventListener
    {
        private readonly Dictionary<EGameStateEventType, IGameStateEventHandler> _eventHandlers = new()
        {
            { EGameStateEventType.GameStart, new GameStateEventGameStartedHandler() },
            { EGameStateEventType.PlayerTurn, new GameStateEventActivePlayerChangedHandler() },
            { EGameStateEventType.FullEntity, new GameStateEventFullEntityHandler() },
            { EGameStateEventType.GameEnd, new GameStateEventGameEndedHandler() }
        };

        private BattleInstance _battleInstance;
        public bool IsListening { get; private set; }

        public Queue<GameStateEvent> GameEventQueue { get; } = new();

        public void Init(BattleInstance battleInstance)
        {
            _battleInstance = battleInstance;
            SubscribeToGameStateEvents();
        }

        public void Terminate()
        {
            UnsubscribeFromGameStateEvents();
        }

        public void AddEventToQueue(GameStateEvent gameEvent)
        {
            GameEventQueue.Enqueue(gameEvent);
            if (_eventHandlers.TryGetValue(gameEvent.EventType, out var handler))
                handler.HandleGameEvent(_battleInstance, gameEvent);
        }

        private void SubscribeToGameStateEvents()
        {
            IsListening = true;
        }

        private void UnsubscribeFromGameStateEvents()
        {
            IsListening = false;
        }
    }
}