using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class MainProjectScope : ProjectScope
{
    [SerializeField]
    private WorldScriptableObject _world;

    public override void InitializeContainer(ContainerBuilder builder)
    {
        Debug.Log("Project Scope!!");
        builder.AddSingleton<GameSetting>();
        builder.AddTransient<AssetLoader>();
        builder.AddSingleton<WorldScriptableObject>(_world);
        builder.AddSingleton<WorldClearData>();
    }
}
