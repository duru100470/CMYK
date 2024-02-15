using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using Cysharp.Threading.Tasks;

public class ChapterScene2 : SceneScope, IScene
{
    public SceneScope SceneScope => this;
    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    private MapData _mapData;

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