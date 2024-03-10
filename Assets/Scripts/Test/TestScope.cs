using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe.VContainer;
using MessagePipe;

public class TestScope : LifetimeScope
{
    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    [SerializeField]
    private TestSoundController _testSoundController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameSetting>(Lifetime.Singleton);
        builder.Register<AssetLoader>(Lifetime.Transient);
        builder.Register<MapData>(Lifetime.Singleton);
        builder.Register<IMapModel, MapModel>(Lifetime.Singleton);
        builder.RegisterComponent(_mapController);
        builder.RegisterComponent(_testView);
        builder.RegisterComponent(_testSoundController).As<ISoundController>();

        // Register MessagePipe
        var options = builder.RegisterMessagePipe();
        builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
        builder.RegisterMessageBroker<PlayerEvent>(options);
        builder.RegisterMessageBroker<PlayerMoveEvent>(options);
    }
}