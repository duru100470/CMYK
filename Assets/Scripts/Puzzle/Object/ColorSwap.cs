using System;
using System.Linq;
using BasicInjector;
public class ColorSwap : MapObject, IObtainable
{
    public void Obtain()
    {
        var character = MapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player);

        (character as Player).SwapColorWithBackground();

        MapModel.RemoveMapObject(this);
    }
}
