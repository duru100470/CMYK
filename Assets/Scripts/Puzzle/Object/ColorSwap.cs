using System;
using System.Linq;

public class ColorSwap : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = _mapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player);

        (character as Player).SwapColorWithBackground();

        _mapModel.RemoveMapObject(this);
    }
}
