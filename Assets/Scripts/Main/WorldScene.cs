using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WorldScene : SceneScope, IScene
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
        Debug.Log("World scene is loaded!");

        LoadAsync().Forget();
    }

    public override void Unload()
    {
        Debug.Log("World scene is unloaded!");
    }

    private async UniTaskVoid LoadAsync()
    {
        await (_gameSetting.LoadAsync(), _worldLoader.InitWorlds(_gameSetting));
    }
    
    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingletonAs<MapModel, IMapModel>();
    }
}