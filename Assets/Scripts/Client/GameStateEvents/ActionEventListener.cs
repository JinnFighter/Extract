using System;
using System.Collections.Generic;
using Common;
using FishNet.Transporting;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Client.GameStateEvents
{
    public class ActionEventListener
    {
        private readonly Stack<ActionEvent> _sequenceStack = new();
        private readonly List<ActionEvent> _lastLoggedEvents = new();
        private readonly Dictionary<EActionEventType, IActionEventHandler> _eventHandlers = new()
        {
            { EActionEventType.GameStart, new ActionEventGameStartedHandler() },
            { EActionEventType.FullEntity, new ActionEventFullEntityHandler() },
        };

        private BattleInstanceClient _battleInstance;
        private NetworkService _networkService;
        public bool IsListening { get; private set; }
        public event Action<List<ActionEvent>> OnEventsLogged;

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
                if (_eventHandlers.TryGetValue(actionEvent.EventType, out var handler))
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
            OnEventsLogged?.Invoke(new List<ActionEvent>(_lastLoggedEvents));
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