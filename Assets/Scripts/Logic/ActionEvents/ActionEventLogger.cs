using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.ActionEvents
{
    public class ActionEventLogger
    {
        public int TurnNumber { get; private set; }
        public int EventNumber { get; private set; }
        private readonly List<ActionEvent> _lastLoggedEvents = new();
        private readonly Stack<ActionEvent> _sequenceStack = new();

        public event Action<List<ActionEvent>> OnEventsLogged;

        public void LogGameEvent(ActionEvent actionEvent)
        {
            EventNumber++;
            if (actionEvent is ActionEventPlayerChanged)
            {
                TurnNumber++;
            }
            actionEvent.EventId = EventNumber;
            actionEvent.TurnNumber = TurnNumber;
            if (actionEvent.EventType == EActionEventType.SequenceStart)
            {
                _sequenceStack.Push(actionEvent);
            }
            else if (actionEvent.EventType == EActionEventType.SequenceEnd)
            {
                _sequenceStack.Pop();
            }
            _lastLoggedEvents.Add(actionEvent);
            Debug.Log($"Logged event: {actionEvent.EventType}");

            if (_sequenceStack.Count > 0)
            {
                return;
            }
            OnEventsLogged?.Invoke(new List<ActionEvent>(_lastLoggedEvents));
            _lastLoggedEvents.Clear();
        }
    }
}