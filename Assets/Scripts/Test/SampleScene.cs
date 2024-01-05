using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class SampleScene : SceneScope, IScene
{
    public override void InitializeContainer(ContainerBuilder containerBuilder)
    {
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