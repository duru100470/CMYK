using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MapObject, IInitializable
{
    [Inject]
    public Channel<PlayerEvent> channel;
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    
    public ReactiveProperty<ColorType> playerColor = new();


    private Transform _transform;

    public override void Initialize()
    {
        base.Initialize();
        _transform = GetComponent<Transform>();

        playerColor.Value = Info.Color;
        playerColor.OnValueChanged += OnPlayerColorChanged;
    }
    void OnDestroy()
    {
        playerColor.OnValueChanged -= OnPlayerColorChanged;
    }
    private void OnPlayerColorChanged(ColorType color)
    {
        Info.Color = color;
        GetComponent<SpriteRenderer>().color = color.ToColor();

        if (Info.Color == color)
        {
            channel.Notify(new PlayerEvent { Type = PlayerEventType.GameOver });
            MapModel.RemoveMapObject(this);
        }
    }

    protected override void OnBackgroundColorChanged(ColorType color)
    {
        base.OnBackgroundColorChanged(color);

        if (Info.Color == color)
        {
            channel.Notify(new PlayerEvent { Type = PlayerEventType.GameOver });
            MapModel.RemoveMapObject(this);
        }
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

            if (obj is IMoveable)
            {
                var movableObj = obj as IMoveable;

                if (!movableObj.TryMove(dir))
                {
                    moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.FakeMove });
                    return;
                }
            }
            if (obj is IObtainable)
            {
                var obtainableObj = obj as IObtainable;
                obtainableObj.Obtain();
            }
        }

        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);

    }
}
