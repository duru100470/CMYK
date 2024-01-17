using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BasicInjector;
using UnityEditor;
using UnityEngine;
using MessageChannel;

public class TestMapController : MapController, IInitializable
{
    [Inject]
    public TestView testView;
    [Inject]
    public Channel<PlayerMoveEvent> channel;

    [SerializeField]
    private ColorType _startBGColor;
    [SerializeField]
    private string _filename;
    private string _loadedFilename;
    private Stack<MapData> _moveRecord = new Stack<MapData>();

    private void OnDestroy()
    {
        channel.Unsubscribe(OnPlayerEventOccurred);
    }
    public void Initialize()
    {
        InitMap();
        channel.Subscribe(OnPlayerEventOccurred);
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
    }

    public string Load()
    {
#if UNITY_EDITOR
        var path = $"{Application.dataPath}/Maps/{_filename}.json";
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
        }

        _loadedFilename = _filename;
        return $"Map data in {path} was loaded.";
#else
        throw new InvalidOperationException("Map Editor is not available in production.");
#endif
    }

    public string Save()
    {
#if UNITY_EDITOR
        var path = $"{Application.dataPath}/Maps/{_filename}.json";
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

        json = testMapData.ExportData();

        if (File.Exists(path) && _filename != _loadedFilename)
        {
            throw new InvalidOperationException("There is already a file at the target directory. Change the filename before save it map.");
        }

        File.WriteAllText(path, json);
        return $"Map data was saved in {path}";
#else
        throw new InvalidOperationException("Map Editor is not available in production.");
#endif
    }

    public void Undo()
    {
        if (_moveRecord.Count == 0)
        {
            Debug.Log("push");
            return;
        }

        ResetMap();
        var tempMapData = _moveRecord.Pop();
        foreach (var (coor, info) in tempMapData.MapObjects)
        {
            var go =
                SceneLoader.Instance.CurrentSceneScope.Instantiate(assetLoader.LoadPrefab<GameObject>($"MapObjects/{info.Type}"), _puzzle);

            var mo = go.GetComponent<MapObject>();
            mo.Coordinate = coor;
            mo.Info = info;
            mo.Init();

            mapModel.AddMapObject(mo);
            Debug.Log($"Create MapObject! [{mo.Coordinate}, {mo.Info.Type}]");
        }

        mapModel.BackgroundColor.Value = tempMapData.InitColor;
    }

    public void OnPlayerEventOccurred(PlayerMoveEvent playerMoveEvent)
    {
        switch (playerMoveEvent.Type)
        {
            case PlayerMoveEventType.TrueMove:
                var tempMapData = new MapData();
                var mapObjects = _puzzle.GetComponentsInChildren<MapObject>();

                foreach (var o in mapObjects)
                {
                    o.Coordinate = Coordinate.WorldPointToCoordinate(o.GetComponent<Transform>().position);
                    tempMapData.MapObjects.Add((o.Coordinate, o.Info));
                }
                tempMapData.InitColor = mapModel.BackgroundColor.Value;

                _moveRecord.Push(tempMapData);
                break;

            case PlayerMoveEventType.FakeMove:
                _moveRecord.Pop();
                break;
        }

    }
}
