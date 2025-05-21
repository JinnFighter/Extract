using UnityEngine;

namespace Logic
{
    public class TileSetup : MonoBehaviour
    {
        [field: SerializeField] public bool Walkable { get; private set; } = true;
        [field: SerializeField] public Vector2Int TilePosition { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }
}