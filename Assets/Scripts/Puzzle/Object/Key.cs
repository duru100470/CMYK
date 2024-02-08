using System.Linq;
using BuildReportTool;

public class Key : MapObject, IObtainable
{
    public void Obtain()
    {
        var doors = MapModel.GetObjects()
            .Where(obj => obj.Info.Type == ObjectType.KeyDoor && obj.Info.Color == Info.Color);

        foreach (var keyDoor in doors)
        {
            MapModel.RemoveMapObject(keyDoor);
        }

        MapModel.RemoveMapObject(this);
    }
}