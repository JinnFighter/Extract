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
        [field: SerializeField] public List<Vector3> Team1SpawnPoints { get; private set; }
        [field: SerializeField] public List<Vector3> Team2SpawnPoints { get; private set; }

        [SerializeField] private Transform _dataTilemap;
        [SerializeField] private Tilemap _objectsTilemap;
        [SerializeField] private Tilemap _areaTilemap;
        [field: SerializeField] public List<TileSetup> TileSetups { get; set; } = new();

        public void Setup()
        {
            TileSetups.Clear();
            for (var i = _dataTilemap.transform.childCount; i > 0; --i)
                DestroyImmediate(_dataTilemap.transform.GetChild(0).gameObject);

            var start = _areaTilemap.WorldToCell(LeftBoundary.position);
            var end = _areaTilemap.WorldToCell(RightBoundary.position);
            for (var i = start.x; i <= end.x; i++)
            for (var j = start.y; j >= end.y; j--)
            {
                var obj = new GameObject($"Tile {i}-{j}");
                obj.transform.SetParent(_dataTilemap.transform);
                obj.transform.position = _areaTilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
                var setup = obj.AddComponent<TileSetup>();
                setup.TilePosition = new Vector2Int(i, j);
                Undo.RecordObject(setup, "Setup");
                EditorUtility.SetDirty(setup); 
                TileSetups.Add(setup);
            }
            
            Undo.RecordObject(this, "Test Scriptable Editor Modify"); 
            EditorUtility.SetDirty(this); 
        }
    }


    [CustomEditor(typeof(GameFieldSetup))]
    public class GameFieldSetupEditor : Editor
    {
        private SerializedProperty _tileSetupsProperty;
        private void OnEnable()
        {
            _tileSetupsProperty = serializedObject.FindProperty("TileSetups");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Game Field Setup"))
            {
                ((GameFieldSetup)target).Setup();
            }
        }
    }
}