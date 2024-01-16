using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BasicInjector;
using UnityEditor;
using UnityEngine;

public class TestMapController : MapController, IInitializable
{
    [Inject]
    public TestView testView;

    [SerializeField]
    private ColorType _startBGColor;
    public string Filename;
    private string _loadedFilename;


    public void Initialize()
    {
        InitMap();
    }

    public override void InitMap()
    {
        GenerateMapFromScene();
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
