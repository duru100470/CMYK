using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MapObject, IInitializable
{
    [Inject]
    public ISoundController _soundController;
    [Inject]
    public Channel<PlayerEvent> channel;
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;

    private ReactiveProperty<ColorType> _playerColor = new();
    public ReactiveProperty<ColorType> PlayerColor => _playerColor;

    private Transform _transform;
    private ColorType _myColorType;
    public bool IsMoveable { get; set; } = true;

    public override void Initialize()
    {
        base.Initialize();
        _transform = GetComponent<Transform>();

        PlayerColor.Value = Info.Color;
        PlayerColor.OnValueChanged += OnPlayerColorChanged;
        _myColorType = MapModel.BackgroundColor.Value;
    }

    void OnDestroy()
    {
        PlayerColor.OnValueChanged -= OnPlayerColorChanged;
    }

    private void OnPlayerColorChanged(ColorType color)
    {

        Info.Color = color;
        GetComponent<SpriteRenderer>().color = color.ToColor();

        if (MapModel.BackgroundColor.Value == color)
        {
            channel.Notify(new PlayerEvent { Type = PlayerEventType.GameOver });
            MapModel.RemoveMapObject(this);
        }
    }

    // playerColor를 직접 변경하여 색상을 교환하는 과정에서 둘의 색상이 같아져 게임오버가 되는것을 방지하기 위해 구현 
    public void SwapColorWithBackground()
    {
        ColorType playerColor = Info.Color, backgroundColor = MapModel.BackgroundColor.Value;

        Info.Color = backgroundColor;
        GetComponent<SpriteRenderer>().color = backgroundColor.ToColor();

        MapModel.BackgroundColor.Value = playerColor;
    }

    protected override void OnBackgroundColorChanged(ColorType color)
    {
        base.OnBackgroundColorChanged(color);

        if (color == _myColorType)
            return;
        else
            _myColorType = color;

        if (Info.Color == color)
        {
            _soundController.PlayEffect(SFXType.GameOver, 1.0f, 1.0f);

            channel.Notify(new PlayerEvent { Type = PlayerEventType.GameOver });
            MapModel.RemoveMapObject(this);
        }
        IsMoveable = false;
        Invoke("MoveableSetTrue", 1);
    }

    private void MoveableSetTrue()
    {
        IsMoveable = true;
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
        if (!IsMoveable)
            return;

        moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.TrueMove });

        var target = Coordinate + dir;
        if (MapModel.TryGetObject(target, out var obj))
        {
            if (obj.Info.IsSolidType)
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

    public void OnMoveRightInit()
    => Move(new Coordinate(1, 0));
}
