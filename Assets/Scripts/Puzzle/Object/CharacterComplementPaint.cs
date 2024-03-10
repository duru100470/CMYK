using System;
using System.Linq;

public class CharacterComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = _mapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player);

        var player = character as Player;
        player.PlayerColor.Value = player.PlayerColor.Value.GetComplementColor();

        _mapModel.RemoveMapObject(this);
    }
}
