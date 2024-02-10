using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [HideInInspector]
    [Inject]
    public GameSetting _gameSetting;
    [Inject]
    public WorldLoader _worldLoader;

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
        await (_gameSetting.LoadAsync(), _worldLoader.InitWorlds(_gameSetting));
    }

    private void Update()
    {
        if (Input.anyKeyDown && _worldLoader.IsWorldLoaded)
        {
            if (_worldLoader.TryLoadMap(0, 0, out var data))
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(data).Forget();
        }
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
    }
}