using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainPlayer : MapObject, IInitializable
{
    private Transform _transform;
    private int sceneIndex;
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;
    public int MaxRight;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public override void Initialize()
    {
        base.Initialize();
        _transform = GetComponent<Transform>();
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
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
    }
}
