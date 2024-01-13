using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MapObject, IInitializable
{
    [Inject]
    public IMapModel mapModel;
    [Inject]
    public Channel<PlayerEvent> channel;

    private Transform _transform;

    public void Initialize()
    {
        _transform = GetComponent<Transform>();
        mapModel.BackgroundColor.OnValueChanged += OnBackgroundColorChanged;
    }

    private void OnDestroy()
    {
        mapModel.BackgroundColor.OnValueChanged -= OnBackgroundColorChanged;
    }

    private void OnBackgroundColorChanged(ColorType color)
    {
        if (Info.Color == color)
        {
            channel.Notify(new PlayerEvent { Type = PlayerEventType.GameOver });
            mapModel.RemoveMapObject(this);
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
        var target = Coordinate + dir;

        if (MapModel.TryGetObject(target, out var obj))
        {
            if (obj.Info.Type == ObjectType.Wall)
                return;

            if (obj is IMoveable)
            {
                var movableObj = obj as IMoveable;

                if (!movableObj.TryMove(dir))
                    return;
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