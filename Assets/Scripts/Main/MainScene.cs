using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class MainScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    public override void Load(object param)
    {
        Debug.Log("Main scene is loaded!");
    }

    public override void Unload()
    {
        Debug.Log("Main scene is unloaded!");
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
    }
}