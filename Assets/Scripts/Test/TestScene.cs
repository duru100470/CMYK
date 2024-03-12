using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using MessageChannel;

// TODO: Deprecated

public class TestScene : SceneScope, IScene
{
    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    public SceneScope SceneScope => this;

    public override void Load(object param)
    {
        base.Load();
        Debug.Log("Test scene is loaded!");

        _mapController.InitMap();
    }

    public override void Unload()
    {
        _mapController.ResetMap();
        Debug.Log("Test scene is unloaded!");
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingleton<MapData>(null);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MapController>(_mapController);
        builder.AddSingleton<TestView>(_testView);
        builder.AddSingleton<Channel<PlayerEvent>>();
        builder.AddSingleton<Channel<PlayerMoveEvent>>();
    }
}
