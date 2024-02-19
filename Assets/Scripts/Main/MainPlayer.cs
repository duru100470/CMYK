using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class MainPlayer : MapObject, IInitializable
{
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    [Inject]
    public ISoundController _soundController;

    [SerializeField]
    private PlayerController _playerController;
    public int MaxRight;

    public override void Initialize()
    {
        base.Initialize();
        _playerController.OnPlayerMove += Move;
    }

    private void Move(Coordinate dir)
    {
        moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.TrueMove });

        var target = Coordinate + dir;
        if (MapModel.TryGetObject(target, out var obj)) 
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
        if (MapModel.TryGetObject(Coordinate, out var obj))
        {
            Debug.Log(obj);
        }
        
    }
}
