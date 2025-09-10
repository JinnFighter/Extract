using System.Collections.Generic;
using Logic.ActionEvents;
using UnityEngine;

namespace Client.Replay.Viewers
{
    public class ActionViewerMovement : BaseActionViewer<ActionEventPositionChanged>
    {
        protected override IEnumerator<bool> PlayInner(BattleInstanceClient battleInstance, ActionEventPositionChanged actionEvent)
        {
            var caster = battleInstance.ModelClient.UnitEntityModels[actionEvent.UnitId];

            var pathTarget = actionEvent.NewPosition;

            var worldTarget = battleInstance.ModelClient.TileEntityModels[pathTarget].WorldPosition;
            var currentPosition = caster.WorldPosition;
            var nextPosition = Vector3.MoveTowards(currentPosition, worldTarget, 1f * Time.deltaTime);
            caster.WorldPosition = nextPosition;

            if (Vector3.SqrMagnitude(worldTarget - nextPosition) <= 0.005f)
            {
                yield return true;
                yield break;
            }

            yield return false;
        }
    }
}