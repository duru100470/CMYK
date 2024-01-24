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

        if (MapModel.TryGetObject(target, out var obj))
        {
            if (obj.Info.Type == ObjectType.Wall)
                return false;
            if (obj is IMoveable)
                return false;
            if (obj is IObtainable)
                MapModel.RemoveMapObject(obj);
        }

        Move(dir);

        return true;
    }
}
