public class CharacterPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        var characters = MapModel.GetObjectsByInfo(new ObjectInfo { Type = ObjectType.Player}, true);

        foreach(var character in characters)
        {
            if(character is Player)
                (character as Player).playerColor.Value += Info.Color;
        }

        MapModel.RemoveMapObject(this);
    }
}
