using System.Linq;

public class CharacterPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjects(new ObjectInfo { Type = ObjectType.Player }, true).First();

        if (character is Player)
            (character as Player).PlayerColor.Value += Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
