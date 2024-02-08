using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
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

        LoadAsync().Forget();
    }

    public override void Unload()
    {
        Debug.Log("Main scene is unloaded!");
    }

    private async UniTaskVoid LoadAsync()
    {
        var setting = Container.Resolve<GameSetting>();

        await UniTask.WhenAll(setting.LoadAsync(), LoadWorldDataAsync(setting));
    }

    private async UniTask LoadWorldDataAsync(GameSetting setting)
    {
        await _world.LoadAsync(setting);

        Debug.Log("World data is loaded!");
        _isMapLoaded = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && _isMapLoaded)
        {
            if (_world.TryGetMapData(0, out var data))
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(data).Forget();
        }
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
    }
}