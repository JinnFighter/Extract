using System;
using System.Collections.Generic;
using Common;
using FishNet.Transporting;
using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Client.ActionEvents
{
    public class ActionEventListener
    {
        private readonly Stack<ActionEvent> _sequenceStack = new();
        private readonly List<ActionEvent> _lastLoggedEvents = new();
        private readonly List<ActionEvent> _eventsBuffer = new();

        private BattleInstanceClient _battleInstance;
        private NetworkService _networkService;
        public bool IsListening { get; private set; }
        public event Action<List<ActionEvent>> OnEventsLogged;

        public readonly Dictionary<EActionEventType, IActionEventHandler> EventHandlers = new()
        {
            { EActionEventType.InitStart, new ActionEventInitStartedHandler() },
            { EActionEventType.GameStart , new ActionEventGameStartHandler() },
            { EActionEventType.FullEntity, new ActionEventFullEntityHandler() },
            { EActionEventType.PlayerTurn, new ActionEventActivePlayerChangedHandler() },
            { EActionEventType.PositionChanged, new ActionEventPositionChangedHandler() },
            { EActionEventType.GameEnd, new ActionEventGameEndedHandler() }
        };

        public void Init(BattleInstanceClient battleInstance, NetworkService networkService)
        {
            _battleInstance = battleInstance;
            _networkService = networkService;
            SubscribeToGameStateEvents();
        }

        public void Terminate()
        {
            UnsubscribeFromGameStateEvents();
        }

        public void AddEventToQueue(ActionEvent actionEvent)
        {
            if (actionEvent.IsInitEvent)
            {
                if (EventHandlers.TryGetValue(actionEvent.EventType, out var handler))
                {
                    handler.HandleActionEvent(_battleInstance, actionEvent);
                }
                
                return;
            }
            
            if (actionEvent.EventType == EActionEventType.SequenceStart)
            {
                _sequenceStack.Push(actionEvent);
            }
            else if (actionEvent.EventType == EActionEventType.SequenceEnd)
            {
                _sequenceStack.Pop();
            }
            _lastLoggedEvents.Add(actionEvent);

            if (_sequenceStack.Count > 0)
            {
                return;
            }
            _eventsBuffer.Clear();
            _eventsBuffer.AddRange(_lastLoggedEvents);
            OnEventsLogged?.Invoke(_eventsBuffer);
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
                    var entity = _battleInstance.ModelClient.GetPlayerEntity(arg1.Option.EntityId);
                    if (arg1.Option.IsAdd)
                        entity.AddOption(arg1.Option);
                    else
                        entity.RemoveOption(arg1.Option);
                    break;
                case EEntityType.Unit:
                    if (arg1.Option.IsAdd)
                        _battleInstance.ModelClient.UnitEntityModels[arg1.Option.EntityId].AddOption(arg1.Option);
                    else
                        _battleInstance.ModelClient.UnitEntityModels[arg1.Option.EntityId].RemoveOption(arg1.Option);
                    break;
            }
        }
    }
}