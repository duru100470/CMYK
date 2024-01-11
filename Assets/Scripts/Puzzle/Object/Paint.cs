using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MapObject, IObtainable
{

    public void Obtain()
    {
        var backgroundColorType = MapModel.BackgroundColor.Value;
        backgroundColorType.AddColor(Info.Color);

        MapModel.BackgroundColor.Value = backgroundColorType;

        MapModel.RemoveMapObject(this);
    }
}
