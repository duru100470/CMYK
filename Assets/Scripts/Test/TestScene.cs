using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using MessageChannel;

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
    }

    public override void Unload()
    {
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
        builder.AddSingleton<Channel<ColorChangeEvent>>();
    }
}
