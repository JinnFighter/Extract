using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic
{
    public class GameFieldSetup : MonoBehaviour
    {
        [field: SerializeField] public Transform LeftBoundary { get; private set; }
        [field: SerializeField] public Transform RightBoundary { get; private set; }

        [SerializeField] private Transform _dataTilemap;
        [SerializeField] private Tilemap _objectsTilemap;
        [SerializeField] private Tilemap _areaTilemap;
        public Dictionary<Vector2Int, TileSetup> TilesSetup { get; } = new();

        public void Setup()
        {
            TilesSetup.Clear();
            for (var i = _dataTilemap.transform.childCount; i > 0; --i)
                DestroyImmediate(_dataTilemap.transform.GetChild(0).gameObject);

            var start = _areaTilemap.WorldToCell(LeftBoundary.position);
            var end = _areaTilemap.WorldToCell(RightBoundary.position);
            for (var i = start.x; i <= end.x; i++)
            for (var j = start.y; j >= end.y; j--)
            {
                var obj = new GameObject($"Tile {i}-{j}");
                obj.transform.SetParent(_dataTilemap.transform);
                obj.transform.position = _areaTilemap.CellToWorld(new Vector3Int(i, j, 0)) + new Vector3(0.5f, 0, 0.5f);
                var setup = obj.AddComponent<TileSetup>();
                TilesSetup.Add(new Vector2Int(i, j), setup);
            }
        }
    }


    [CustomEditor(typeof(GameFieldSetup))]
    public class GameFieldSetupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Game Field Setup")) ((GameFieldSetup)target).Setup();
        }
    }
}