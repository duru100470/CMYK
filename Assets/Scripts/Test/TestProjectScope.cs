using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using Unity.VisualScripting;
using UnityEngine;

public class TestProjectScope : ProjectScope
{
    public override void InitializeContainer(ContainerBuilder builder)
    {
        Debug.Log("Project Scope!!");
        builder.AddSingleton<Channel<TestMessage>>();
        builder.AddSingleton<GameSetting>();
        builder.AddTransient<AssetLoader>();
    }
}
