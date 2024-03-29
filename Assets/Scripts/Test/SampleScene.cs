using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class SampleScene : SceneScope, IScene
{
    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    [SerializeField]
    private TestMono _testMono;
    public SceneScope SceneScope => this;

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingleton<MapData>(null);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<TestView>(_testView);
        builder.AddSingleton<MapController>(_mapController);
        builder.AddSingleton<TestMono>(_testMono);
    }

    public override void Load(object param = null)
    {
        base.Load();
        Debug.Log("Sample Scene is loaded!");
    }

    public override void Unload()
    {
        Debug.Log("Sample Scene is unloaded!");
    }
}