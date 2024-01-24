using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class IngameMapController : MapController, IInitializable
{
    [Inject]
    public Channel<PlayerEvent> _playerEventChannel;
    [Inject]
    public Channel<PlayerMoveEvent> _playerMoveEventChannel;
    // TODO: 임시로 월드 1개만 처리하게 짜놓음
    [Inject]
    public WorldScriptableObject _world;
    [Inject]
    public WorldClearData _worldClear;

    public void Initialize()
    {
        InitMap();
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
            _worldClear.LastID++;

            if (_world.Maps.Count > _worldClear.LastID)
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(_world.Maps[_worldClear.LastID].Data).Forget();
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
