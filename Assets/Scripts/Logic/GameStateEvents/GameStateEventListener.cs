using System.Collections.Generic;
using Common;
using FishNet.Transporting;
using Logic.ActionRequests;

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
        private NetworkService _networkService;
        public bool IsListening { get; private set; }

        public Queue<GameStateEvent> GameEventQueue { get; } = new();

        public void Init(BattleInstance battleInstance, NetworkService networkService)
        {
            _battleInstance = battleInstance;
            _networkService = networkService;
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
            _networkService.SubscribeClientBroadcast<BroadcastOption>(HandleOptionBroadcast);
            IsListening = true;
        }

        private void UnsubscribeFromGameStateEvents()
        {
            _networkService.UnsubscribeClientBroadcast<BroadcastOption>(HandleOptionBroadcast);
            IsListening = false;
        }

        private void HandleOptionBroadcast(BroadcastOption arg1, Channel arg2)
        {
            switch (arg1.Option.EntityType)
            {
                case EEntityType.Player:
                    if (arg1.Option.IsAdd)
                        _battleInstance.Model.PlayerEntityModels[arg1.Option.EntityId].AddOption(arg1.Option);
                    else
                        _battleInstance.Model.PlayerEntityModels[arg1.Option.EntityId].RemoveOption(arg1.Option);
                    break;
                case EEntityType.Unit:
                    if (arg1.Option.IsAdd)
                        _battleInstance.Model.UnitEntityModels[arg1.Option.EntityId].AddOption(arg1.Option);
                    else
                        _battleInstance.Model.UnitEntityModels[arg1.Option.EntityId].RemoveOption(arg1.Option);
                    break;
            }
        }
    }
}