using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BasicInjector;
using UnityEditor;
using UnityEngine;
using MessageChannel;
using UnityEngine.U2D;

public class TestMapController : MapController, IInitializable
{
    [Inject]
    public TestView testView;
    [Inject]
    public Channel<PlayerMoveEvent> channel;

    [SerializeField]
    private ColorType _startBGColor;
    public string Filename;
    private string _loadedFilename;
    private Stack<MapData> _moveRecord = new Stack<MapData>();

    private void OnDestroy()
    {
        channel.Unsubscribe(OnPlayerMoveEventOccurred);
    }
    public void Initialize()
    {
        channel.Subscribe(OnPlayerMoveEventOccurred);
    }

    public override void InitMap()
    {
        GenerateMapFromScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // TODO : 게임 클리어 상황에서 뒤로가기 비활성화
            Undo();
        }
    }

    private void GenerateMapFromScene()
    {
        var testMapData = new MapData();
        var mapObjects = _puzzle.GetComponentsInChildren<MapObject>();

        foreach (var o in mapObjects)
        {
            o.Coordinate = Coordinate.WorldPointToCoordinate(o.GetComponent<Transform>().position);
            testMapData.MapObjects.Add((o.Coordinate, o.Info));
            mapModel.AddMapObject(o);

            Debug.Log($"Add MapObject! [{o.Coordinate}, {o.Info.Type}]");
        }

        mapModel.BackgroundColor.Value = _startBGColor;
        mapData = testMapData;
    }

    public override void ResetMap()
    {
        int children = _puzzle.transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            var go = _puzzle.GetChild(i).gameObject;
            mapModel.RemoveMapObject(go.GetComponent<MapObject>());
            Destroy(go);
        }

        int children2 = _decorations.transform.childCount;

        for (int i = children2 - 1; i >= 0; i--)
        {
            var go = _decorations.GetChild(i).gameObject;
            Destroy(go);
        }
    }

    public string Load()
    {
#if UNITY_EDITOR
        var path = $"{Application.dataPath}/Maps/{Filename}.json";
        var jsonData = File.ReadAllText(path);

        if (Application.isPlaying)
        {
            ResetMap();
            mapData.ImportData(jsonData);
            GenerateMapFromData();
        }
        else
        {
            int children = _puzzle.transform.childCount;

            for (int i = children - 1; i >= 0; i--)
            {
                var go = _puzzle.GetChild(i).gameObject;
                DestroyImmediate(go);
            }

            int children2 = _decorations.transform.childCount;

            for (int i = children2 - 1; i >= 0; i--)
            {
                var go = _decorations.GetChild(i).gameObject;
                DestroyImmediate(go);
            }

            var data = new MapData();
            data.ImportData(jsonData);

            var assetLoader = new AssetLoader();

            foreach (var (coor, info) in data.MapObjects)
            {
                var go =
                    Instantiate(assetLoader.LoadPrefab<GameObject>($"MapObjects/{info.Type}"), _puzzle);

                var mo = go.GetComponent<MapObject>();
                mo.Coordinate = coor;
                mo.Info = info;
                mo.Init();

                Debug.Log($"Create MapObject! [{mo.Coordinate}, {mo.Info.Type}]");
            }

            foreach (var (coor, name) in data.DecorationObjects)
            {
                var go =
                    Instantiate(assetLoader.LoadPrefab<GameObject>($"Decorations/{name}"), _decorations);
                go.transform.position = Coordinate.CoordinateToWorldPoint(coor);

                Debug.Log($"Create DecorationObject! [{coor}, {name}]");
            }
        }

        _loadedFilename = Filename;
        return $"Map data in {path} was loaded.";
#else
        throw new InvalidOperationException("Map Editor is not available in production.");
#endif
    }

    public string Save()
    {
#if UNITY_EDITOR
        var path = $"{Application.dataPath}/Maps/{Filename}.json";
        var json = "";

        if (Application.isPlaying)
            throw new InvalidOperationException("Cannot save a map while playing.");

        var testMapData = new MapData();
        var mapObjects = _puzzle.GetComponentsInChildren<MapObject>();

        foreach (var o in mapObjects)
        {
            o.Coordinate = Coordinate.WorldPointToCoordinate(o.GetComponent<Transform>().position);
            testMapData.MapObjects.Add((o.Coordinate, o.Info));

            Debug.Log($"Add MapObject! [{o.Coordinate}, {o.Info.Type}]");
        }

        var decorationObjects = _decorations.GetComponentsInChildren<SpriteRenderer>();
        foreach (var o in decorationObjects)
        {
            var coor = Coordinate.WorldPointToCoordinate(o.GetComponent<Transform>().position);
            testMapData.DecorationObjects.Add((coor, o.gameObject.name));

            Debug.Log($"Add DecorationObject! [{coor}, {o.gameObject.name}]");
        }

        json = testMapData.ExportData();

        if (File.Exists(path) && Filename != _loadedFilename)
        {
            throw new InvalidOperationException("There is already a file at the target directory. Change the filename before save it map.");
        }

        File.WriteAllText(path, json);
        return $"Map data was saved in {path}";
#else
        throw new InvalidOperationException("Map Editor is not available in production.");
#endif
    }
}
