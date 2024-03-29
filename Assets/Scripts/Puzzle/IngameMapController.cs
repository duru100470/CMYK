using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BasicInjector;
using Cysharp.Threading.Tasks;
using MessageChannel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class IngameMapController : MapController, IInitializable
{
    [Inject]
    public MessageChannel.Channel<PlayerEvent> _playerEventChannel;
    [Inject]
    public MessageChannel.Channel<PlayerMoveEvent> _playerMoveEventChannel;
    [Inject]
    public WorldLoader _worldLoader;
    [Inject]
    public ISoundController _soundController;
    [Inject]
    public GameSetting _gameSetting;

    private string _url = "http://duruchigy.ddns.net/cmyk/events";

    private bool _moveable = true;
    private ColorType _myColorType;

    public void Initialize()
    {
        _playerEventChannel.Subscribe(OnPlayerEventOccurred);
        _playerMoveEventChannel.Subscribe(OnPlayerMoveEventOccurred);
        mapModel.BackgroundColor.OnValueChanged += OnColorEventOccurred;
        _myColorType = mapModel.BackgroundColor.Value;

        var e = new GameEvent(_gameSetting.Id, "puzzle_start", Application.version, _worldLoader.CurrentMapIndex.Item1 * 1000 + _worldLoader.CurrentMapIndex.Item2);
        NetworkManager.SendToServer(_url, SendType.POST, e.ToJson()).Forget();
    }

    private void OnDestroy()
    {
        _playerEventChannel.Unsubscribe(OnPlayerEventOccurred);
        _playerMoveEventChannel.Unsubscribe(OnPlayerMoveEventOccurred);
        mapModel.BackgroundColor.OnValueChanged -= OnColorEventOccurred;
    }

    public override void InitMap()
    {
        GenerateMapFromData();
    }

    public override void ResetMap()
    {
        int children = _puzzle.transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            var go = _puzzle.GetChild(i).gameObject;
            mapModel.RemoveMapObject(go.GetComponent<MapObject>());
            Destroy(go);
        }
    }

    private void OnPlayerEventOccurred(PlayerEvent @event)
    {
        if (@event.Type == PlayerEventType.GameClear)
        {
            _moveable = false;

            var e = new GameEvent(_gameSetting.Id, "puzzle_clear", Application.version, _worldLoader.CurrentMapIndex.Item1 * 1000 + _worldLoader.CurrentMapIndex.Item2);
            NetworkManager.SendToServer(_url, SendType.POST, e.ToJson()).Forget();

            Invoke("SceneChange", 2);

            _soundController.PlayEffect(SFXType.GameClear, 1f, 1f);
            // if (_world.Maps.Count > _worldClear.LastID)
            //     SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(_world.Maps[_worldClear.LastID].Data).Forget();

            // if (_world.Maps.Count == _worldClear.LastID)
            //     Application.Quit();
        }

        if (@event.Type == PlayerEventType.GameOver)
        {
            _soundController.PlayEffect(SFXType.GameOver, 1f, 1f);
        }
    }

    private void SceneChange()
    {
        _worldLoader.UpdateClearDataAsync(_worldLoader.CurrentMapIndex.Item1, _worldLoader.CurrentMapIndex.Item2).Forget();

        switch (_worldLoader.CurrentMapIndex.Item1)
        {
            case 0:
                if (_worldLoader.CurrentMapIndex.Item2 == 7)
                    SceneLoader.Instance.LoadSceneAsync<MainScene>((_worldLoader.CurrentMapIndex.Item1 + 1) * 19).Forget();
                else
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene0>(_worldLoader.CurrentMapIndex.Item2 * 5 + 4).Forget();
                break;
            case 1:
                if (_worldLoader.CurrentMapIndex.Item2 == 4)
                    SceneLoader.Instance.LoadSceneAsync<MainScene>((_worldLoader.CurrentMapIndex.Item1 + 1) * 19).Forget();
                else
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene1>(_worldLoader.CurrentMapIndex.Item2 * 5 + 4).Forget();
                break;
            case 2:
                if (_worldLoader.CurrentMapIndex.Item2 == 2)
                    SceneLoader.Instance.LoadSceneAsync<MainScene>((_worldLoader.CurrentMapIndex.Item1 + 1) * 19).Forget();
                else
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene2>(_worldLoader.CurrentMapIndex.Item2 * 5 + 4).Forget();
                break;
            default:
                break;
        }
    }

    public void ToMain()
    {
        SceneLoader.Instance.LoadSceneAsync<MainScene>(null).Forget();
    }
    private void OnColorEventOccurred(ColorType colorType)
    {
        if (colorType == _myColorType)
            return;
        else
            _myColorType = colorType;
        _moveable = false;
        Invoke("MoveableSetTrue", 1);

        _soundController.PlayEffect(SFXType.Paint, 1f, 1f);
    }

    private void MoveableSetTrue()
    {
        _moveable = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && _moveable)
        {
            // TODO : 게임 클리어 상황에서 뒤로가기 비활성화
            Undo();
        }

        if (Input.GetKeyDown(KeyCode.R) && _moveable)
        {
            ResetMap();
            InitMap();

            var e = new GameEvent(_gameSetting.Id, "puzzle_restart", Application.version, _worldLoader.CurrentMapIndex.Item1 * 1000 + _worldLoader.CurrentMapIndex.Item2);
            NetworkManager.SendToServer(_url, SendType.POST, e.ToJson()).Forget();
        }
    }
}
