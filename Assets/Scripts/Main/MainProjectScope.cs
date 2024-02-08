using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class MainProjectScope : ProjectScope
{
    [SerializeField]
    private WorldLoader _worldLoader;
    [SerializeField]
    private GameSetting _gameSetting = new();

    public override void InitializeContainer(ContainerBuilder builder)
    {
        Debug.Log("Project Scope!!");
        builder.AddSingleton<GameSetting>(_gameSetting);
        builder.AddTransient<AssetLoader>();
        builder.AddSingleton<WorldLoader>(_worldLoader);
    }

    [ContextMenu("Reset Settings")]
    public void ResetSetting()
    {
        PlayerPrefs.DeleteKey("GameSettings");
    }
}
