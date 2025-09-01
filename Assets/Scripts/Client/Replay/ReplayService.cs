using System.Collections;
using System.Collections.Generic;
using Logic.ActionEvents;
using UnityEngine;
using VContainer;

namespace Client.Replay
{
    public class ReplayService : MonoBehaviour
    {
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
                if (actionReplay == null)
                {
                    continue;
                }
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
            var isPresent = _battleInstance.ActionEventListener.EventHandlers.TryGetValue(actionEvent.EventType, out var handler);
            if (!isPresent)
            {
                return null;
            }
            _eventViewers.TryGetValue(actionEvent.EventType, out var viewer);
            var replay = new ActionReplay
            {
                ActionEvent = actionEvent,
                EventHandler = handler,
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
                foreach (var actionReplay in currentSequence) yield return Play(actionReplay, battleInstance);
            }
            
            _isPlaying = false;
        }
        
        private IEnumerator Play(ActionReplay replay, BattleInstanceClient battleInstance)
        {
            Debug.Log($"PLAYING {replay.ActionEvent.EventType}");
            var isFrameReached = false;
            if (replay.Viewer != null)
            {
                var viewerSequence = replay.Viewer.Play(battleInstance, replay.ActionEvent);
                while (viewerSequence.MoveNext())
                {
                    var isKeyFrameHit = viewerSequence.Current;
                    if (isKeyFrameHit)
                    {
                        replay.EventHandler.HandleActionEvent(battleInstance, replay.ActionEvent);
                        isFrameReached = true;
                    }

                    yield return null;
                }
            }

            if (!isFrameReached)
            {
                replay.EventHandler.HandleActionEvent(battleInstance, replay.ActionEvent);
            }
        }
    }
}