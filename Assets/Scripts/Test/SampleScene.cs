using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class SampleScene : SceneScope, IScene
{
    [SerializeField]
    private TestMapController _testMapController;

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingletonAs<TestMapController, IMapController>(_testMapController);
    }

    public override void Load(object param = null)
    {
        base.Load();
        Debug.Log("Sample Scene is loaded!");

        _testMapController.InitMap(null);
    }

    public override void Unload()
    {
        Debug.Log("Sample Scene is unloaded!");
    }
}