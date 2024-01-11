using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MapObject, IObtainable
{

    public void Obtain()
    {
        var backgroundColorType = MapModel.BackgroundColor.Value;
        backgroundColorType.RemoveColor(Info.Color);

        MapModel.BackgroundColor.Value = backgroundColorType;

        MapModel.RemoveMapObject(this);
    }
}
