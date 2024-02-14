using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MessageChannel;

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

    public override void InitializeContainer(ContainerBuilder builder)
    {
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MessageChannel.Channel<PlayerMoveEvent>>();
    }
}