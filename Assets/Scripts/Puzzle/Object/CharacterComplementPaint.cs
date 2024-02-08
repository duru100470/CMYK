using System.Linq;

public class CharacterComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjects(new ObjectInfo { Type = ObjectType.Player }, true).First();

        if (character is Player)
        {
            var player = character as Player;
            player.PlayerColor.Value = player.PlayerColor.Value.GetComplementColor();
        }

        MapModel.RemoveMapObject(this);
    }
}
