using System.Linq;

public class Key : MapObject, IObtainable
{
    public void Obtain()
    {
        var doors = _mapModel.GetObjects()
            .Where(obj => obj.Info.Type == ObjectType.KeyDoor && obj.Info.Color == Info.Color)
            .ToList();

        foreach (var keyDoor in doors)
        {
            _mapModel.RemoveMapObject(keyDoor);
        }

        _mapModel.RemoveMapObject(this);
    }
}