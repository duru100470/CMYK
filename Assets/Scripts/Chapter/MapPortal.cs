using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using UnityEngine.SceneManagement;

public class MapPortal : MapObject, IObtainable
{
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    [SerializeField]
    private PortalAnimation _dataEffect;
    [SerializeField]
    private Sprite[] _sprites;
    [Inject]
    public WorldLoader _worldLoader;

    private bool _inPlayer = false;
    private bool _lock = false;
    private bool _init = false;
    private SpriteRenderer _spriteRenderer;
    private MapStatus _mapStatus;
    public int Chapter;
    public int World;
    private bool _isMainScene = false;

    public override void Initialize()
    {
        base.Initialize();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        moveChannel.Subscribe(OnPlayeMoveEventOccurred);

        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene")
            _isMainScene = true;
    }

    public void Obtain()
    {
        Debug.Log("checkin");
        if (!_lock)
        {
            _dataEffect.AnimationOn();
            _inPlayer = true;
        }
    }

    private void OnPlayeMoveEventOccurred(PlayerMoveEvent moveEvent)
    {
        if (_inPlayer)
        {
            _dataEffect.AnimationOff();
            _inPlayer = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _worldLoader.IsWorldLoaded && _inPlayer && !_isMainScene)
        {
            if (_worldLoader.TryLoadMap(World, Chapter, out var data))
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(data).Forget();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isMainScene && _inPlayer)
        {
            switch (World)
            {
                case 0:
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene0>(null).Forget();
                    break;
                case 1:
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene1>(null).Forget();
                    break;
                case 2:
                    SceneLoader.Instance.LoadSceneAsync<ChapterScene2>(null).Forget();
                    break;
                default:
                    break;
            }
        }

        if (!_init && _worldLoader != null)
        {
            _mapStatus = _worldLoader.GetStatus(World, Chapter);

            switch (_mapStatus)
            {
                case MapStatus.Cleared:
                    Debug.Log("clear");
                    _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
                    _spriteRenderer.sprite = _sprites[0];
                    _lock = false;
                    _init = true;
                    break;
                case MapStatus.Available:
                    Debug.Log("available");
                    _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
                    _spriteRenderer.sprite = _sprites[1];
                    _lock = false;
                    _init = true;
                    break;
                case MapStatus.Locked:
                    _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
                    _spriteRenderer.sprite = _sprites[1];
                    _lock = true;
                    break;
                default:
                    break;
            }
        }
    }

}
