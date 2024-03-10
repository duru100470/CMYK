using System;
using System.Linq;

public class CharacterPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = _mapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player);

        if (character is Player)
            (character as Player).PlayerColor.Value += Info.Color;

        _mapModel.RemoveMapObject(this);
    }
}
