using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;

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
        if (Input.GetKeyDown(KeyCode.Space) && _worldLoader.IsWorldLoaded && _inPlayer)
        {
            if (_worldLoader.TryLoadMap(World, Chapter, out var data))
                SceneLoader.Instance.LoadSceneAsync<PuzzleScene>(data).Forget();
        }
    }

}
