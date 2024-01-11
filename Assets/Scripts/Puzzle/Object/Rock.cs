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
                case ObjectType.Rock:
                    return false;
                case ObjectType.Flag:
                case ObjectType.Paint:
                case ObjectType.Eraser:
                    MapModel.RemoveMapObject(obj);
                    break;
            }
        }

        Move(dir);

        return true;
    }
}
