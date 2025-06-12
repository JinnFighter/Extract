using System.Collections;
using System.Collections.Generic;
using Client.GameStateEvents;
using Logic.GameStateEvents;
using UnityEngine;
using VContainer;

namespace Client.Replay
{
    public class ReplayService : MonoBehaviour
    {
        private readonly Dictionary<EActionEventType, IActionEventHandler> _eventHandlers = new()
        {
            { EActionEventType.PlayerTurn, new ActionEventActivePlayerChangedHandler() },
            { EActionEventType.FullEntity, new ActionEventFullEntityHandler() },
            { EActionEventType.GameEnd, new ActionEventGameEndedHandler() }
        };

        private readonly Dictionary<EActionEventType, IActionViewer> _eventViewers = new();

        private readonly Queue<List<ActionReplay>> _unplayedSequences = new();
        [Inject] private BattleInstanceClient _battleInstance;

        private bool _isPlaying;

        public void Init()
        {
            _battleInstance.ActionEventListener.OnEventsLogged += HandleEventsLogged;
        }

        public void Terminate()
        {
            _battleInstance.ActionEventListener.OnEventsLogged -= HandleEventsLogged;
        }

        private void Replay(List<ActionEvent> gameStateEvents)
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

        private void HandleEventsLogged(List<ActionEvent> obj)
        {
            Replay(obj);
        }

        private ActionReplay ParseEvent(ActionEvent actionEvent)
        {
            _eventViewers.TryGetValue(actionEvent.EventType, out var viewer);
            var replay = new ActionReplay
            {
                ActionEvent = actionEvent,
                EventHandler = _eventHandlers[actionEvent.EventType],
                Viewer = viewer
            };
            return replay;
        }

        private void StartReplay(BattleInstanceClient battleInstance)
        {
            if (_isPlaying) return;
            
            StartCoroutine(ReplayInner(battleInstance));
        }

        private IEnumerator ReplayInner(BattleInstanceClient battleInstance)
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