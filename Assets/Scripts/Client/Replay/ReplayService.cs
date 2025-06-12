using System.Collections;
using System.Collections.Generic;
using Logic;
using Logic.GameStateEvents;
using UnityEngine;
using VContainer;

namespace Client.Replay
{
    public class ReplayService : MonoBehaviour
    {
        private readonly Dictionary<EGameStateEventType, IGameStateEventHandler> _eventHandlers = new()
        {
            { EGameStateEventType.PlayerTurn, new GameStateEventActivePlayerChangedHandler() },
            { EGameStateEventType.FullEntity, new GameStateEventFullEntityHandler() },
            { EGameStateEventType.GameEnd, new GameStateEventGameEndedHandler() }
        };

        private readonly Dictionary<EGameStateEventType, IActionViewer> _eventViewers = new();

        private readonly Queue<List<ActionReplay>> _unplayedSequences = new();
        [Inject] private BattleInstance _battleInstance;

        private bool _isPlaying;

        public void Init()
        {
            _battleInstance.GameStateEventListener.OnEventsLogged += HandleEventsLogged;
        }

        public void Terminate()
        {
            _battleInstance.GameStateEventListener.OnEventsLogged -= HandleEventsLogged;
        }

        private void Replay(List<GameStateEvent> gameStateEvents)
        {
            var currentSequence = new List<ActionReplay>();
            foreach (var gameStateEvent in gameStateEvents)
            {
                var actionReplay = ParseEvent(gameStateEvent);
                currentSequence.Add(actionReplay);
            }
            
            _unplayedSequences.Enqueue(currentSequence);
            
            StartReplay(_battleInstance);
        }

        private void HandleEventsLogged(List<GameStateEvent> obj)
        {
            Replay(obj);
        }

        private ActionReplay ParseEvent(GameStateEvent gameStateEvent)
        {
            _eventViewers.TryGetValue(gameStateEvent.EventType, out var viewer);
            var replay = new ActionReplay
            {
                GameStateEvent = gameStateEvent,
                EventHandler = _eventHandlers[gameStateEvent.EventType],
                Viewer = viewer
            };
            return replay;
        }

        private void StartReplay(BattleInstance battleInstance)
        {
            if (_isPlaying) return;
            
            StartCoroutine(ReplayInner(battleInstance));
        }

        private IEnumerator ReplayInner(BattleInstance battleInstance)
        {
            _isPlaying = true;
            while (_unplayedSequences.Count > 0)
            {
                var currentSequence = _unplayedSequences.Dequeue();
                foreach (var actionReplay in currentSequence) yield return actionReplay.Play(battleInstance);
            }
            
            _isPlaying = false;
        }
    }
}