using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class PuzzleScene : SceneScope, IScene
{
    [SerializeField]
    private MapController _mapController;
    private MapData _mapData;

    public SceneScope SceneScope => this;

    public override void Load(object param)
    {
        _mapData = param as MapData;

        base.Load();
        Debug.Log("Puzzle scene is loaded!");
    }

    public override void Unload()
    {
        Debug.Log("Puzzle scene is unloaded!");
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingleton<MapData>(_mapData);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MapController>(_mapController);
    }
}
