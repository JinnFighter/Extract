using System;
using System.Collections.Generic;
using Common;
using FishNet.Transporting;
using Logic.ActionRequests;

namespace Logic.GameStateEvents
{
    public class GameStateEventListener
    {
        private readonly Stack<GameStateEvent> _sequenceStack = new();
        private readonly List<GameStateEvent> _lastLoggedEvents = new();
        private readonly Dictionary<EGameStateEventType, IGameStateEventHandler> _eventHandlers = new()
        {
            { EGameStateEventType.GameStart, new GameStateEventGameStartedHandler() },
            { EGameStateEventType.FullEntity, new GameStateEventFullEntityHandler() },
        };

        private BattleInstance _battleInstance;
        private NetworkService _networkService;
        public bool IsListening { get; private set; }
        public event Action<List<GameStateEvent>> OnEventsLogged;

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

        public void AddEventToQueue(GameStateEvent gameStateEvent)
        {
            if (gameStateEvent.IsInitEvent)
            {
                if (_eventHandlers.TryGetValue(gameStateEvent.EventType, out var handler))
                {
                    handler.HandleGameEvent(_battleInstance, gameStateEvent);
                }
                
                return;
            }
            
            if (gameStateEvent.EventType == EGameStateEventType.SequenceStart)
            {
                _sequenceStack.Push(gameStateEvent);
            }
            else if (gameStateEvent.EventType == EGameStateEventType.SequenceEnd)
            {
                _sequenceStack.Pop();
            }
            _lastLoggedEvents.Add(gameStateEvent);

            if (_sequenceStack.Count > 0)
            {
                return;
            }
            OnEventsLogged?.Invoke(new List<GameStateEvent>(_lastLoggedEvents));
            _lastLoggedEvents.Clear();
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
                    var entity = _battleInstance.Model.GetPlayerEntity(arg1.Option.EntityId);
                    if (arg1.Option.IsAdd)
                        entity.AddOption(arg1.Option);
                    else
                        entity.RemoveOption(arg1.Option);
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