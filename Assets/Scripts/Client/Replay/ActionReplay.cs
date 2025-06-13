using System.Collections;
using Client.GameStateEvents;
using Logic;
using Logic.ActionEvents;
using UnityEngine;

namespace Client.Replay
{
    public class ActionReplay
    {
        public IActionViewer Viewer;
        public ActionEvent ActionEvent;
        public IActionEventHandler EventHandler;
        public IEnumerator Play(BattleInstanceClient battleInstance)
        {
            Debug.Log($"PLAYING {ActionEvent.EventType}");
            var isFrameReached = false;
            if (Viewer != null)
            {
                var viewerSequence = Viewer.Play(battleInstance, ActionEvent);
                while (viewerSequence.MoveNext())
                {
                    var isKeyFrameHit = viewerSequence.Current;
                    if (isKeyFrameHit)
                    {
                        EventHandler.HandleActionEvent(battleInstance, ActionEvent);
                        isFrameReached = true;
                    }

                    yield return null;
                }
            }

            if (!isFrameReached)
            {
                EventHandler.HandleActionEvent(battleInstance, ActionEvent);
            }
        }
    }
}