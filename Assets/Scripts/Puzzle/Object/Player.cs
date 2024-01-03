using System.Collections;
using System.Collections.Generic;
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
        // TODO: 움직이기 전 MapData에서 움직일 수 있는지 판별해야함
        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
    }
}