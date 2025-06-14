using System.Collections.Generic;
using Logic.ActionRequests;
using UnityEngine;

namespace Client.Actions.ActionBuilders
{
    public class ActionRequestBuilderMove : BaseActionRequestBuilder
    {
        public List<Vector2Int> _path = new();
        public override void Reset()
        {
            _path.Clear();
        }

        protected override bool ValidateInner()
        {
            return _path.Count > 0;
        }

        public void SetPath(List<Vector2Int> path)
        {
            _path.AddRange(path);
        }

        protected override ActionRequest BuildActionInner()
        {
            return new ActionRequestMove
            {
                CasterId = CasterId,
                Path = _path
            };
        }
    }
}