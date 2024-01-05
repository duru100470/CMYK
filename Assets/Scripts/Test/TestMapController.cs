using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class TestMapController : MapController, IInitializable
{
    [Inject]
    public IMapModel mapModel;
    [Inject]
    public TestView testView;
    [Inject]
    public MapData mapData;
    [SerializeField]
    private Transform _puzzle;
    [SerializeField]
    private ColorType _startBGColor;


    public void Initialize()
    {
        InitMap();
    }

    public override void InitMap()
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mapModel.BackgroundColor.Value = ColorType.Cyan;
        }
    }

    public override void ResetMap()
    {
        throw new System.NotImplementedException();
    }
}
