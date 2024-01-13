using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MapObject
{
    private Transform _transform;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
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