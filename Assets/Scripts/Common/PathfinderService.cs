using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class PathfinderService : IPathfinderService
    {
        private readonly List<Vector2Int> _directions = new()
        {
            Vector2Int.up,
            new Vector2Int(-1, 1),
            Vector2Int.left,
            new Vector2Int(-1, -1),
            Vector2Int.down,
            new Vector2Int(1, -1),
            Vector2Int.right,
            new Vector2Int(1, 1)
        };

        private readonly ITileMovePriceGetter _tileMovePriceGetter;

        public PathfinderService(ITileMovePriceGetter priceGetter)
        {
            _tileMovePriceGetter = priceGetter;
        }

        public List<Vector2Int> GetPath(Vector2Int start, Vector2Int target, Vector2Int size)
        {
            var movementContainer = FindPath(start, target, size);
            return CollectPath(start, target, movementContainer);
        }

        private Dictionary<Vector2Int, (Vector2Int nextPosition, Vector2Int cameFrom)> FindPath(Vector2Int start,
            Vector2Int target, Vector2Int size)
        {
            var movementContainer = new Dictionary<Vector2Int, (Vector2Int nextPosition, Vector2Int cameFrom)>();

            var border = new PriorityQueue<Vector2Int>();
            border.Enqueue(start, 0);

            var costSoFar = new Dictionary<Vector2Int, int> { [start] = 0 };

            while (border.Count > 0)
            {
                var currentPosition = border.Dequeue();

                if (currentPosition == target)
                    break;

                foreach (var direction in _directions)
                {
                    var nextPosition = currentPosition + direction;
                    if (nextPosition == currentPosition || !IsClamped(nextPosition, size)) continue;

                    var nextWeight = _tileMovePriceGetter.GetPrice(nextPosition);
                    if (nextWeight < 0 && !nextPosition.Equals(target)) continue;

                    var nextCost = costSoFar[currentPosition] + nextWeight;

                    if (costSoFar.TryGetValue(nextPosition, out var nextCostSoFar) &&
                        nextCost >= nextCostSoFar) continue;

                    costSoFar[nextPosition] = nextCost;
                    border.Enqueue(nextPosition, nextCost + GetHeuristicDistance(nextPosition, target));

                    movementContainer[nextPosition] = (nextPosition, currentPosition);
                }
            }

            return movementContainer;
        }

        private List<Vector2Int> CollectPath(Vector2Int start, Vector2Int target,
            Dictionary<Vector2Int, (Vector2Int nextPosition, Vector2Int cameFrom)>
                movementContainer)
        {
            var curPosition = target;
            var path = new Stack<Vector2Int>();

            while (!curPosition.Equals(start) && movementContainer.TryGetValue(curPosition, out var data))
            {
                path.Push(data.nextPosition);
                curPosition = data.cameFrom;
            }

            return curPosition.Equals(start) ? new List<Vector2Int>(path) : new List<Vector2Int>();
        }

        private int GetHeuristicDistance(Vector2Int start, Vector2Int end)
        {
            return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
        }

        private bool IsClamped(Vector2Int value, Vector2Int bounds)
        {
            return value.x >= 0 && value.x < bounds.x && value.y >= 0 && value.y < bounds.y;
        }
    }
}