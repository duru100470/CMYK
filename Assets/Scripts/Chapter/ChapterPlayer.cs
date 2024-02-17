using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChapterPlayer : MapObject, IInitializable
{
    [Inject]
    public Channel<PlayerEvent> channel;
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    [Inject]
    public ISoundController _soundController;

    private ReactiveProperty<ColorType> _playerColor = new();
    public ReactiveProperty<ColorType> PlayerColor => _playerColor;

    private Transform _transform;

    public int MaxRight;

    public override void Initialize()
    {
        base.Initialize();
        _transform = GetComponent<Transform>();
    }

    void OnDestroy()
    {

    }

    private void OnMoveUp()
        => Move(new Coordinate(0, 1));

    private void OnMoveDown()
        => Move(new Coordinate(0, -1));

    private void OnMoveLeft()
        => Move(new Coordinate(-1, 0));

    private void OnMoveRight()
        => Move(new Coordinate(1, 0));

    private void Move(Coordinate dir)
    {
        moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.TrueMove });

        var target = Coordinate + dir;
        if (MapModel.TryGetObject(target, out var obj))
        {
            if (obj.Info.Type == ObjectType.Wall || obj.Info.Type == ObjectType.KeyDoor)
            {
                moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.FakeMove });
                return;
            }
            if (obj is IObtainable)
            {
                var obtainableObj = obj as IObtainable;
                obtainableObj.Obtain();
            }
        }
        if (target.X < -9 || target.Y > 4 || target.Y < -4 || target.X > MaxRight)
        {
            return;
        }
        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);

        _soundController.PlayEffect(SFXType.PlayerMove, 1f, 1f);
    }
}
