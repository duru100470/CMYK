using System.Linq;

public class CharacterComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjectsByInfo(new ObjectInfo { Type = ObjectType.Player}, true).First();

            if(character is Player)
            {
                var player = character as Player; 
                player.playerColor.Value = player.playerColor.Value.GetComplementColor();
            }

        MapModel.RemoveMapObject(this);
    }
}
