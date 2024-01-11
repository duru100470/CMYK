using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MapObject, IMoveable
{
    private Transform _transform;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    public void Move(Coordinate dir)
    {
        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
    }
    public bool TryMove(Coordinate dir)
    {
        var target = Coordinate + dir;

        if(MapModel.TryGetObject(target, out var obj))
        {
            switch(obj.Info.Type)
            {
                case ObjectType.Wall:
                case ObjectType.Movable:
                    return false;
            }
        }

        Move(dir);

        return true;
    }
}
