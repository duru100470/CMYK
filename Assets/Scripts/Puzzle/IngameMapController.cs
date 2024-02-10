using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using MessageChannel;
using UnityEngine;

public class IngameMapController : MapController, IInitializable
{
    [Inject]
    public MessageChannel.Channel<PlayerEvent> _playerEventChannel;
    [Inject]
    public MessageChannel.Channel<PlayerMoveEvent> _playerMoveEventChannel;
    [Inject]
    public WorldLoader _worldLoader;

    public void Initialize()
    {
        _playerEventChannel.Subscribe(OnPlayerEventOccurred);
        _playerMoveEventChannel.Subscribe(OnPlayerMoveEventOccurred);
    }

    private void OnDestroy()
    {
        _playerEventChannel.Unsubscribe(OnPlayerEventOccurred);
        _playerMoveEventChannel.Unsubscribe(OnPlayerMoveEventOccurred);
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
            _worldLoader.UpdateClearDataAsync(0, 0).Forget();

            // if (_world.Maps.Count > _worldClear.LastID)
            //     SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(_world.Maps[_worldClear.LastID].Data).Forget();

            // if (_world.Maps.Count == _worldClear.LastID)
            //     Application.Quit();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // TODO : 게임 클리어 상황에서 뒤로가기 비활성화
            Undo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
            InitMap();
        }
    }
}
