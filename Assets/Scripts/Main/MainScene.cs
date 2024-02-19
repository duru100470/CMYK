using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MessageChannel;
using TMPro;
using System.Linq;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class MainScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [SerializeField]
    private MapController _mapController;
    [SerializeField]
    private TestView _testView;
    [SerializeField]
    private TextMeshProUGUI _version;
    [SerializeField]
    private TextMeshProUGUI _id;
    [SerializeField]
    private GameObject _camera;
    private MainPlayer _character;
    private int _position;

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

        _position = (int)param;
        _character = _testView.mapModel.GetObjects().First(obj => obj.Info.Type == ObjectType.Player) as MainPlayer;
        _character.Coordinate = new Coordinate(_position, -2);
        _character.GetComponent<Transform>().position = Coordinate.CoordinateToWorldPoint(_character.Coordinate);
        _camera.GetComponent<Transform>().position = new Vector3(_character.GetComponent<Transform>().position.x, 0.5f, -10);
        _character.PosInit();
    }

    public override void Unload()
    {
        Debug.Log("Main scene is unloaded!");
    }

    private async UniTaskVoid LoadAsync()
    {
        await (_gameSetting.LoadAsync(), _worldLoader.InitWorlds(_gameSetting));

        _id.text = "UID: " + _gameSetting.Id;
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

    [ContextMenu("Reset Settings")]
    public void ResetSetting()
    {
        PlayerPrefs.DeleteKey("GameSettings");
        Application.Quit();
    }
}
