using System.Linq;
public class CharacterEraser : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjectsByInfo(new ObjectInfo { Type = ObjectType.Player}, true).First();

        if(character is Player)
            (character as Player).playerColor.Value -= Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
