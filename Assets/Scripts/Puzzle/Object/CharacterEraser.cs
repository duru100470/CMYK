using System;
using System.Linq;
public class CharacterEraser : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player);

        (character as Player).PlayerColor.Value -= Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
