using System.Collections.Generic;
using UnityEngine;

namespace Logic.ActionRequests
{
    public class ActionRequestMove : ActionRequest
    {
        public List<Vector2Int> Path { get; set; }
        public override EActionRequestType ActionRequestType => EActionRequestType.Move;

        public override bool IsValid(LogicModelServer logicModelServer)
        {
            if (!logicModelServer.UnitEntities.TryGetValue(CasterId, out _)) return false;
            if (Path.Count == 0) return false;
            foreach (var position in Path)
                if (logicModelServer.TileEntities[position].OccupierId >= 0)
                    return false;

            return true;
        }
    }
}