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

    private bool _inPlayer = false;
    

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

}
