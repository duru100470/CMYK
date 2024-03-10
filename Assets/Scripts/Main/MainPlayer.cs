using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class MainPlayer : MapObject
{
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    [Inject]
    public ISoundController _soundController;

    [SerializeField]
    private PlayerController _playerController;
    public int MaxRight;

    public override void Start()
    {
        base.Start();
        _playerController.OnPlayerMove += Move;
    }

    private void Move(Coordinate dir)
    {
        moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.TrueMove });

        var target = Coordinate + dir;
        if (_mapModel.TryGetObject(target, out var obj))
        {
            if (obj is IObtainable)
            {
                var obtainableObj = obj as IObtainable;
                obtainableObj.Obtain();
            }
        }

        if (target.X < -8 || target.Y > 4 || target.Y < -4 || target.X > MaxRight)
        {
            return;
        }

        Coordinate += dir;
        transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);

        _soundController.PlayEffect(SFXType.PlayerMove, 1f, 1f);
    }


    public void PosInit()
    {
        if (_mapModel.TryGetObject(Coordinate, out var obj))
        {
            Debug.Log(obj);
        }

    }
}
