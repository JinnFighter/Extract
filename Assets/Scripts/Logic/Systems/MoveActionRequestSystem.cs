using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;
using UnityEngine;

namespace Logic.Systems
{
    public class MoveActionRequestSystem : BaseLogicSystem
    {
        private int _pathIndex;
        
        protected override IEnumerator<ActionEvent> RunPrepareLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            Debug.Log($"START MOVE ACTION REQUEST");
            _pathIndex = 0;
            yield return new ActionEventSequenceStart
            {
                SequenceType = EActionSequenceType.Move,
            };
        }

        protected override IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            var moveRequest = rootRequest as ActionRequestMove;
            Debug.Log($"Received move request with path {moveRequest.Path.Count}");
            var unitModel = modelServer.UnitEntities[rootRequest.CasterId];
            while (_pathIndex < moveRequest.Path.Count)
            {
                var currentPosition = unitModel.Position;
                var nextPosition = moveRequest.Path[_pathIndex];
                var currentTile = modelServer.TileEntities[unitModel.Position];
                var nextTile = modelServer.TileEntities[nextPosition];
                currentTile.OccupierId = -1;
                nextTile.OccupierId = unitModel.Id;
                unitModel.Position = nextPosition;
                _pathIndex++;
                yield return new ActionEventPositionChanged
                {
                    OldPosition = currentPosition,
                    NewPosition = nextPosition,
                    UnitId = unitModel.Id,
                };
            }
        }

        protected override IEnumerator<ActionEvent> RunCancelLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            yield return new ActionEventSequenceEnd
            {
                SequenceType = EActionSequenceType.Move,
            };
        }

        protected override IEnumerator<ActionEvent> RunFinishLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            yield return new ActionEventSequenceEnd
            {
                SequenceType = EActionSequenceType.Move,
            };
        }
    }
}