using System.Collections.Generic;
using UnityEngine;
using System;
using VContainer;
public abstract class MapController : MonoBehaviour
{
    [Inject]
    public IMapModel _mapModel;
    [Inject]
    public MapData _mapData;
    [Inject]
    public AssetLoader _assetLoader;

    [SerializeField]
    protected Transform _puzzle;
    [SerializeField]
    protected Transform _decorations;

    private Stack<MapData> _moveRecord = new Stack<MapData>();

    public abstract void InitMap();
    public abstract void ResetMap();

    protected void GenerateMapFromData()
    {
        foreach (var (coor, info) in _mapData.MapObjects)
        {
            var go =
                SceneLoader.Instance.CurrentSceneScope.Instantiate(_assetLoader.LoadPrefab<GameObject>($"MapObjects/{info.Type}"), _puzzle);

            var mo = go.GetComponent<MapObject>();
            mo.Coordinate = coor;
            mo.Info = info;
            mo.Init();

            _mapModel.AddMapObject(mo);
            Debug.Log($"Create MapObject! [{mo.Coordinate}, {mo.Info.Type}]");
        }

        foreach (var (coor, name) in _mapData.DecorationObjects)
        {
            var go =
                SceneLoader.Instance.CurrentSceneScope.Instantiate(_assetLoader.LoadPrefab<GameObject>($"Decorations/{name}"), _decorations);
            go.transform.position = Coordinate.CoordinateToWorldPoint(coor);

            Debug.Log($"Create DecorationObject! [{coor}, {name}]");
        }

        _mapModel.BackgroundColor.Value = _mapData.InitColor;
        ChangeCameraSize(_mapData.MapSize);
    }

    protected void ChangeCameraSize(int size)
    {
        Camera.main.orthographicSize = size switch
        {
            0 => 5.6f,
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
                SceneLoader.Instance.CurrentSceneScope.Instantiate(_assetLoader.LoadPrefab<GameObject>($"MapObjects/{info.Type}"), _puzzle);

            var mo = go.GetComponent<MapObject>();
            mo.Coordinate = coor;
            mo.Info = info;
            mo.Init();

            _mapModel.AddMapObject(mo);
            if (info.Type == ObjectType.Wall)
            {
                go.GetComponent<Wall>().SpriteInit();
            }

            if (info.Type == ObjectType.Player || info.Type == ObjectType.Flag)
            {
                go.GetComponent<Flicker>()._isMoving = true;
            }
        }

        _mapModel.BackgroundColor.Value = tempMapData.InitColor;
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
                tempMapData.InitColor = _mapModel.BackgroundColor.Value;

                _moveRecord.Push(tempMapData);
                break;

            case PlayerMoveEventType.FakeMove:
                _moveRecord.Pop();
                break;
        }
    }
}
