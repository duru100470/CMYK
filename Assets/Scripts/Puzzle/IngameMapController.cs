using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class IngameMapController : MapController, IInitializable
{
    [Inject]
    public Channel<PlayerEvent> channel;

    // TODO: 임시로 월드 1개만 처리하게 짜놓음
    [Inject]
    public WorldScriptableObject _world;

    public void Initialize()
    {
        InitMap();
        channel.Subscribe(OnPlayerEventOccurred);
    }

    private void OnDestroy()
    {
        channel.Unsubscribe(OnPlayerEventOccurred);
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
            SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(null).Forget();
        }

        if (@event.Type == PlayerEventType.GameOver)
        {
            StartCoroutine(GameOverCoroutine());
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(3f);

        SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(mapData).Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // TODO : 게임 클리어 상황에서 뒤로가기 비활성화
            Undo();
        }
    }
}
