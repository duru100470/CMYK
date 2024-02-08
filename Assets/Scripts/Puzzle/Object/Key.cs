public class Key : MapObject, IObtainable
{
    public void Obtain()
    {
        foreach (var keyDoor in MapModel.GetObjects(new ObjectInfo { Type = ObjectType.KeyDoor, Color = Info.Color }))
        {
            MapModel.RemoveMapObject(keyDoor);
        }

        MapModel.RemoveMapObject(this);
    }
}