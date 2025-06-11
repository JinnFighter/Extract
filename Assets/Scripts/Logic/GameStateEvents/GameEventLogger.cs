using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameEventLogger
    {
        public int TurnNumber { get; private set; }
        public int EventNumber { get; private set; }
        private readonly List<GameStateEvent> _lastLoggedEvents = new();
        private readonly Stack<GameStateEvent> _sequenceStack = new();

        public event Action<List<GameStateEvent>> OnEventsLogged;

        public void LogGameEvent(GameStateEvent gameStateEvent)
        {
            EventNumber++;
            if (gameStateEvent is GameStateEventPlayerChanged)
            {
                TurnNumber++;
            }
            gameStateEvent.EventId = EventNumber;
            gameStateEvent.TurnNumber = TurnNumber;
            if (gameStateEvent.EventType == EGameStateEventType.SequenceStart)
            {
                _sequenceStack.Push(gameStateEvent);
            }
            else if (gameStateEvent.EventType == EGameStateEventType.SequenceEnd)
            {
                _sequenceStack.Pop();
            }
            _lastLoggedEvents.Add(gameStateEvent);
            Debug.Log($"Logged event: {gameStateEvent.EventType}");

            if (_sequenceStack.Count > 0)
            {
                return;
            }
            OnEventsLogged?.Invoke(_lastLoggedEvents);
            _lastLoggedEvents.Clear();
        }
    }
}