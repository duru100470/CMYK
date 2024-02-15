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
    [Inject]
    public WorldLoader _worldLoader;

    private bool _inPlayer = false;
    public int Chapter;
    public int World;
    private string sceneName;
    private bool _isMainScene = false;
    
    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainScene")
        {
            _isMainScene = true;
        }
    }
    public override void Initialize()
    {
        base.Initialize();
        moveChannel.Subscribe(OnPlayeMoveEventOccurred);
    }
    public void Obtain()
    {
        _dataEffect.AnimationOn();
        _inPlayer = true;
    }
    private void OnPlayeMoveEventOccurred(PlayerMoveEvent moveEvent)
    {
        if(_inPlayer)
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
        if (Input.GetKeyDown(KeyCode.Space) && _isMainScene && _inPlayer)
        {
            if(World == 0)
            {
                SceneManager.LoadScene("World1");
            }
            if(World == 1)
            {
                SceneManager.LoadScene("World2");
            }
            if(World == 2)
            {
                SceneManager.LoadScene("World3");
            }
            if(World == 3)
            {
                SceneManager.LoadScene("World4");
            }
        }
    }

}
