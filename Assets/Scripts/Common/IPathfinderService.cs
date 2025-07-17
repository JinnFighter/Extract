using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IPathfinderService
    {
        List<Vector2Int> GetPath(Vector2Int start, Vector2Int target, Vector2Int size);
    }
}
