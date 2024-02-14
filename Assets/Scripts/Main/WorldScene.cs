using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MessageChannel;

public class WorldScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;

    [HideInInspector]
    [Inject]
    public GameSetting _gameSetting;
    [Inject]
    public WorldLoader _worldLoader;

    void Awake()
    {
        Load(null);
    }
    public override void Load(object param)
    {
        base.Load();
        Debug.Log("World scene is loaded!");

        _mapController.InitMap();

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
        builder.AddSingleton<MapData>(null);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MapController>(_mapController);
        builder.AddSingleton<TestView>(_testView);
        builder.AddSingleton<MessageChannel.Channel<PlayerEvent>>();
        builder.AddSingleton<MessageChannel.Channel<PlayerMoveEvent>>();
    }
}