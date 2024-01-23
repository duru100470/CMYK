using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using MessageChannel;
using UnityEngine;

public class MainScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [Inject]
    public WorldScriptableObject _world;
    private bool _isMapLoaded = false;

    public override void Load(object param)
    {
        base.Load();
        Debug.Log("Main scene is loaded!");

        LoadWorldDataAsync().Forget();
    }

    public override void Unload()
    {
        Debug.Log("Main scene is unloaded!");
    }

    private async UniTaskVoid LoadWorldDataAsync()
    {
        await _world.LoadMapDataAsync();

        Debug.Log("World data is loaded!");
        _isMapLoaded = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && _isMapLoaded)
        {
            SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(_world.Maps[0].Data).Forget();
        }
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
    }
}