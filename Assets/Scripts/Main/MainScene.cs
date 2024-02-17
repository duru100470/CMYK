using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MessageChannel;
using TMPro;

public class MainScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    [SerializeField]
    private TextMeshProUGUI _version;

    [HideInInspector]
    [Inject]
    public GameSetting _gameSetting;
    [Inject]
    public WorldLoader _worldLoader;

    public override void Load(object param)
    {
        base.Load();
        Debug.Log("Main scene is loaded!");

        _mapController.InitMap();

        LoadAsync().Forget();

        _version.text = "Version: " + Application.version;
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
        builder.AddSingleton<MapData>(null);
        builder.AddSingletonAs<MapModel, IMapModel>();
        builder.AddSingleton<MapController>(_mapController);
        builder.AddSingleton<TestView>(_testView);
        builder.AddSingleton<MessageChannel.Channel<PlayerEvent>>();
        builder.AddSingleton<MessageChannel.Channel<PlayerMoveEvent>>();
    }
}