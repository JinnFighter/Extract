using System.Collections;
using System.Collections.Generic;
using Client.Replay.Viewers;
using Logic.ActionEvents;
using UnityEngine;
using VContainer;

namespace Client.Replay
{
    public class ReplayService : MonoBehaviour
    {
        private readonly Dictionary<EActionSequenceType, IActionViewer> _eventViewers = new()
        {
            { EActionSequenceType.Move, new ActionViewerMovement() },
        };

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
            var currentSequence = ParseSequence(gameStateEvents);

            if (currentSequence.Count == 0)
            {
                return;
            }
            
            _unplayedSequences.Enqueue(currentSequence);
            
            StartReplay(_battleInstance);
        }

        private void HandleEventsLogged(List<ActionEvent> obj)
        {
            Replay(obj);
        }

        private ActionReplay ParseEvent(ActionEvent actionEvent, IActionViewer viewer)
        {
            var isPresent = _battleInstance.ActionEventListener.EventHandlers.TryGetValue(actionEvent.EventType, out var handler);
            if (!isPresent)
            {
                return null;
            }
            
            var replay = new ActionReplay
            {
                ActionEvent = actionEvent,
                EventHandler = handler,
                Viewer = viewer
            };
            return replay;
        }

        private List<ActionReplay> ParseSequence(List<ActionEvent> gameStateEvents)
        {
            var currentSequence = new List<ActionReplay>();
            var sequenceInfo = new Stack<IActionViewer>();
            sequenceInfo.Push(null);
            Debug.Log($"Received {gameStateEvents.Count} events to replay");
            foreach (var gameStateEvent in gameStateEvents)
            {
                switch (gameStateEvent.EventType)
                {
                    case EActionEventType.SequenceStart:
                    {
                        var sequenceStartType = ((ActionEventSequenceStart)gameStateEvent).SequenceType;
                        Debug.Log($"Starting sequence of type {sequenceStartType}");
                        _eventViewers.TryGetValue(sequenceStartType, out var viewer);
                        sequenceInfo.Push(viewer);
                        break;
                    }
                    case EActionEventType.SequenceEnd:
                        var sequenceEndType = ((ActionEventSequenceEnd)gameStateEvent).SequenceType;
                        Debug.Log($"Ending sequence of type {sequenceEndType}");
                        sequenceInfo.Pop();
                        break;
                    default:
                    {
                        Debug.Log($"Parsing event of type {gameStateEvent.EventType}");
                        var currentViewer = sequenceInfo.Peek();
                        var actionReplay = ParseEvent(gameStateEvent, currentViewer);
                        if (actionReplay == null)
                        {
                            continue;
                        }

                        currentSequence.Add(actionReplay);
                        break;
                    }
                }
            }

            return currentSequence;
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