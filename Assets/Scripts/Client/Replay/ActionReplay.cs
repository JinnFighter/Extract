using System.Collections;
using Logic;
using Logic.GameStateEvents;
using UnityEngine;

namespace Client.Replay
{
    public class ActionReplay
    {
        public IActionViewer Viewer;
        public GameStateEvent GameStateEvent;
        public IGameStateEventHandler EventHandler;
        public IEnumerator Play(BattleInstance battleInstance)
        {
            Debug.Log($"PLAYING {GameStateEvent.EventType}");
            var isFrameReached = false;
            if (Viewer != null)
            {
                var viewerSequence = Viewer.Play(battleInstance, GameStateEvent);
                while (viewerSequence.MoveNext())
                {
                    var isKeyFrameHit = viewerSequence.Current;
                    if (isKeyFrameHit)
                    {
                        EventHandler.HandleGameEvent(battleInstance, GameStateEvent);
                        isFrameReached = true;
                    }

                    yield return null;
                }
            }

            if (!isFrameReached)
            {
                EventHandler.HandleGameEvent(battleInstance, GameStateEvent);
            }
        }
    }
}