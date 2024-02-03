public class Key : MapObject, IObtainable
{
    public void Obtain()
    {
        foreach(var keyDoor in MapModel.GetObjectsByInfo(new ObjectInfo {Type = ObjectType.KeyDoor , Color = Info.Color}))
        {
            MapModel.RemoveMapObject(keyDoor);
        }

        MapModel.RemoveMapObject(this);
    }
}