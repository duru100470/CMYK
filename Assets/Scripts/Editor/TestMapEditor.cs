using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
#if UNITY_EDITOR
    public class TestMapEditor : EditorWindow
    {
        private Vector2 _cellSize = new Vector2(1f, 1f);
        private bool _paintMode = false;
        [SerializeField]
        private int _paletteIndex;
        private int _editorMode = 0;
        private List<ColorType> _colors = new() { ColorType.None, ColorType.Cyan, ColorType.Magenta, ColorType.Yellow, ColorType.Red, ColorType.Green, ColorType.Blue, ColorType.Key };
        private int _colorIndex;
        private Transform _puzzleObject;
        private TestMapController _mapController;
        private string _message = "";
        private IMapModel _mapModel;

        // The window is selected if it already exists, else it's created.
        [MenuItem("Tools/Map Editor")]
        private static void ShowWindow()
        {
            GetWindow(typeof(TestMapEditor));
        }

        // Called to draw the MapEditor windows.
        private void OnGUI()
        {
            _editorMode = GUILayout.SelectionGrid(_editorMode, new string[2] { "Palette", "Map" }, 2);
            if (_editorMode == 0)
            {
                ShowPaletteEditor();
            }
            else
            {
                ShowMapLoader();
            }
        }

        private void ShowPaletteEditor()
        {
            GUILayout.Space(20f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            _paintMode = GUILayout.Toggle(_paintMode, "Start painting", "Button", GUILayout.Width(120f), GUILayout.Height(30f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);
            _puzzleObject = (Transform)EditorGUILayout.ObjectField("Target: ", _puzzleObject, typeof(Transform), true);
            GUILayout.Space(10f);

            List<GUIContent> colorIcons = new List<GUIContent>();
            for (int i = 0; i < 8; i++)
            {
                var texture = new Texture2D(15, 15);
                Color[] pixels = Enumerable.Repeat(_colors[i].ToColor(), 225).ToArray();
                texture.SetPixels(pixels);
                texture.Apply();
                colorIcons.Add(new GUIContent(texture));
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            _colorIndex = GUILayout.SelectionGrid(_colorIndex, colorIcons.ToArray(), 8);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10f);

            // Get a list of previews, one for each of our prefabs
            List<GUIContent> paletteIcons = new List<GUIContent>();
            foreach (GameObject prefab in _palette)
            {
                // Get a preview for the prefab
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                paletteIcons.Add(new GUIContent(texture));
            }

            // Display the grid
            _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), 6);
            GUILayout.Space(10f);
            GUILayout.Box(_message, GUILayout.ExpandWidth(true));
        }

        private void ShowMapLoader()
        {
            GUILayout.Space(20f);
            _mapController = (TestMapController)EditorGUILayout.ObjectField("Target: ", _mapController, typeof(TestMapController), true);

            if (_mapController is not null)
            {
                _mapController.Filename = EditorGUILayout.TextField("Filename: ", _mapController.Filename);
            }

            EditorGUILayout.BeginHorizontal();
            var boxStyle = new GUIStyle(GUI.skin.box);
            if (GUILayout.Button("Load"))
            {
                try
                {
                    _message = _mapController.Load();
                    boxStyle.normal.textColor = Color.green;
                }
                catch (Exception e)
                {
                    boxStyle.normal.textColor = Color.red;
                    _message = e.ToString();
                }
            }

            if (GUILayout.Button("Save"))
            {
                try
                {
                    _message = _mapController.Save();
                    boxStyle.normal.textColor = Color.green;
                }
                catch (Exception e)
                {
                    boxStyle.normal.textColor = Color.red;
                    _message = e.ToString();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);
            GUILayout.Box(_message, boxStyle, GUILayout.ExpandWidth(true));
        }

        // Does the rendering of the map editor in the scene view.
        private void OnSceneGUI(SceneView sceneView)
        {
            if (_paintMode)
            {
                var center = GetSelectedCell();

                DisplayVisualHelp(center);
                HandleSceneViewInputs(center);

                // Refresh the view
                sceneView.Repaint();
            }
        }

        private Vector2 GetSelectedCell()
        {
            // Get the mouse position in world space such as z = 0
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

            // Get the corresponding cell on our virtual grid
            var center = new Vector2(Mathf.FloorToInt(mousePosition.x) + .5f, Mathf.FloorToInt(mousePosition.y) + .5f);

            return center;
        }

        private void DisplayVisualHelp(Vector2 cellCenter)
        {

            // Vertices of our square
            Vector3 topLeft = cellCenter + Vector2.left * _cellSize * 0.5f + Vector2.up * _cellSize * 0.5f;
            Vector3 topRight = cellCenter - Vector2.left * _cellSize * 0.5f + Vector2.up * _cellSize * 0.5f;
            Vector3 bottomLeft = cellCenter + Vector2.left * _cellSize * 0.5f - Vector2.up * _cellSize * 0.5f;
            Vector3 bottomRight = cellCenter - Vector2.left * _cellSize * 0.5f - Vector2.up * _cellSize * 0.5f;

            // Rendering
            Handles.color = Color.green;
            Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
            Handles.DrawLines(lines);
        }

        private void HandleSceneViewInputs(Vector2 cellCenter)
        {
            // Filter the left click so that we can't select objects in the scene
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0); // Consume the event
            }

            // We have a prefab selected and we are clicking in the scene view with the left button
            if (_paletteIndex < _palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (_puzzleObject == null)
                {
                    _message = "Puzzle Object is not selected";
                    return;
                }

                if (_mapModel.TryGetObject(Coordinate.WorldPointToCoordinate(cellCenter), out var prev))
                {
                    _mapModel.RemoveMapObject(prev);
                    Undo.DestroyObjectImmediate(prev.gameObject);
                }

                // Create the prefab instance while keeping the prefab link
                var prefab = _palette[_paletteIndex];
                var gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                gameObject.transform.position = cellCenter;
                gameObject.transform.SetParent(_puzzleObject);

                var mo = gameObject.GetComponent<MapObject>();
                mo.Coordinate = Coordinate.WorldPointToCoordinate(cellCenter);
                mo.Info.Color = _colors[_colorIndex];
                mo.Init();
                _mapModel.AddMapObject(mo);

                // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                Undo.RegisterCreatedObjectUndo(gameObject, "");
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                if (_mapModel.TryGetObject(Coordinate.WorldPointToCoordinate(cellCenter), out var mo))
                {
                    _mapModel.RemoveMapObject(mo);
                    Undo.DestroyObjectImmediate(mo.gameObject);
                }
            }
        }

        // A list containing the available prefabs.
        [SerializeField]
        private List<GameObject> _palette = new List<GameObject>();
        private string _path = "Assets/Resources/Prefabs/MapObjects/";

        private void RefreshPalette()
        {
            _palette.Clear();

            string[] prefabFiles = System.IO.Directory.GetFiles(_path, "*.prefab");
            foreach (string prefabFile in prefabFiles)
                _palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
        }

        private void RefreshModel()
        {
            _mapModel = new EditorMapModel();

            if (_puzzleObject == null)
            {
                _message = "Puzzle Object is not selected";
                return;
            }

            var mapObjects = _puzzleObject.GetComponentsInChildren<MapObject>();

            foreach (var mo in mapObjects)
            {
                _mapModel.AddMapObject(mo);
            }

            _message = $"Successfully load puzzle ({mapObjects.Length} objects)";
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI; // Just in case
            SceneView.duringSceneGui += this.OnSceneGUI;

            RefreshPalette();
            RefreshModel();

            _mapController = GameObject.Find("Controllers").GetComponent<TestMapController>();
            _puzzleObject = GameObject.Find("Puzzle").GetComponent<Transform>();
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
    }
#endif
}