using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class ChapterScene0 : SceneScope, IScene
{
    public SceneScope SceneScope => this;
    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    [SerializeField]
    private GameObject _camera;
    private MapData _mapData;
    private MainPlayer _character;
    private int _position;

    [Inject]
    public GameSetting _gameSetting;
    [Inject]
    public WorldLoader _worldLoader;

    public override void Load(object param)
    {
        base.Load();
        Debug.Log("Main scene is loaded!");

        _mapController.InitMap();

        LoadAsync().Forget();

        _position = (int)param;
        _character = _testView.mapModel.GetObjects().First(obj => obj.Info.Type == ObjectType.Player) as MainPlayer;
        _character.Coordinate = new Coordinate(_position, -1);
        _character.GetComponent<Transform>().position = Coordinate.CoordinateToWorldPoint(_character.Coordinate);
        _camera.GetComponent<Transform>().position = new Vector3(_character.GetComponent<Transform>().position.x, 0.5f, -10);
        _character.PosInit();
    }

    public override void Unload()
    {
        Debug.Log("Main scene is unloaded!");
    }

    private async UniTaskVoid LoadAsync()
    {
        await (_gameSetting.LoadAsync(), _worldLoader.InitWorlds(_gameSetting));
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingleton<MapData>(_mapData);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MapController>(_mapController);
        builder.AddSingleton<TestView>(_testView);
        builder.AddSingleton<MessageChannel.Channel<PlayerEvent>>();
        builder.AddSingleton<MessageChannel.Channel<PlayerMoveEvent>>();
    }
}
