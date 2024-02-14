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
    private int sceneIndex;
    private bool _isWorldScene = false;
    
    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex >= 2 && sceneIndex <= 5)
        {
            _isWorldScene = true;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_inPlayer);
            Debug.Log(_isWorldScene);
        }
        if (Input.GetKeyDown(KeyCode.Space) && _worldLoader.IsWorldLoaded && _inPlayer && !_isWorldScene)
        {
            if (_worldLoader.TryLoadMap(World, Chapter, out var data))
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(data).Forget();
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isWorldScene && _inPlayer)
        {
            SceneManager.LoadScene(sceneIndex + 4, LoadSceneMode.Single);
        }
    }

}
