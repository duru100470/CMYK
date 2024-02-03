public class CharacterComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var characters = MapModel.GetObjectsByInfo(new ObjectInfo { Type = ObjectType.Player}, true);

        foreach(var character in characters)
        {
            if(character is Player)
            {
                var player = character as Player; 
                player.playerColor.Value = player.playerColor.Value.GetComplementColor();
            }
        }

        MapModel.RemoveMapObject(this);
    }
}
