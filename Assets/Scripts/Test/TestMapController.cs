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
    [SerializeField]
    private string _filename;


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

    [ContextMenu("Load Map")]
    public void Load()
    {
#if UNITY_EDITOR
        try
        {
            ResetMap();

            var path = $"{Application.dataPath}/Maps/{_filename}.json";
            var jsonData = File.ReadAllText(path);

            mapData.ImportData(jsonData);
            GenerateMapFromData();

            Debug.Log($"Map data in {path} was loaded.");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
#else
        Debug.LogError($"Map Editor is not available in production.");
#endif
    }

    [ContextMenu("Save Map")]
    public void Save()
    {
#if UNITY_EDITOR
        try
        {
            var json = mapData.ExportData();
            var path = $"{Application.dataPath}/Maps/{_filename}.json";

            File.WriteAllText(path, json);

            Debug.Log($"Map data was saved in {path}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
#else
        Debug.LogError($"Map Editor is not available in production.");
#endif
    }
}
