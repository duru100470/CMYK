using BasicInjector;
using System.Collections.Generic;
using UnityEngine;

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

    private Stack<MapData> _moveRecord = new Stack<MapData>();

    private Camera camera;

    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

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

        mapModel.BackgroundColor.Value = mapData.InitColor;

        switch (mapData.MapSize)
        {
            case 0:
                camera.orthographicSize = 5;
                break;
            case 1:
                camera.orthographicSize = 10;
                break;
            case 2:
                camera.orthographicSize = 13;
                break;
        }
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
