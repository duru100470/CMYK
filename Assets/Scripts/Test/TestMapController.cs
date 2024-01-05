using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class TestMapController : MapController
{
    [Inject]
    public IMapModel mapModel;
    [SerializeField]
    private Transform _puzzle;
    [SerializeField]
    private ColorType _startBGColor;

    public override void InitMap(MapData mapData)
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
        mapModel.BackgroundColor.OnValueChanged += f => Camera.main.backgroundColor = f.ToColor();
    }

    public override void ResetMap()
    {
        throw new System.NotImplementedException();
    }
}
