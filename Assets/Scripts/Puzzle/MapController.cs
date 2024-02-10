using BasicInjector;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using System;
public abstract class MapController : MonoBehaviour
{
    [Inject]
    public IMapModel mapModel;
    [Inject]
    public MapData mapData;
    [Inject]
    public AssetLoader assetLoader;

    [SerializeField]
    protected Transform _puzzle;
    [SerializeField]
    protected Transform _decorations;

    private Stack<MapData> _moveRecord = new Stack<MapData>();

    public abstract void InitMap();
    public abstract void ResetMap();

    protected void GenerateMapFromData()
    {
        foreach (var (coor, info) in mapData.MapObjects)
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

        foreach (var (coor, name) in mapData.DecorationObjects)
        {
            var go =
                SceneLoader.Instance.CurrentSceneScope.Instantiate(assetLoader.LoadPrefab<GameObject>($"Decorations/{name}"), _decorations);
            go.transform.position = Coordinate.CoordinateToWorldPoint(coor);

            Debug.Log($"Create DecorationObject! [{coor}, {name}]");
        }

        mapModel.BackgroundColor.Value = mapData.InitColor;
        ChangeCameraSize(mapData.MapSize);
    }

    protected void ChangeCameraSize(int size)
    {
        Camera.main.orthographicSize = size switch
        {
            0 => 5,
            1 => 10,
            2 => 13,
            _ => throw new InvalidOperationException()
        };
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

    protected void OnPlayerMoveEventOccurred(PlayerMoveEvent playerMoveEvent)
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
