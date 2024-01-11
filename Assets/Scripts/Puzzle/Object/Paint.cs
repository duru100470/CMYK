using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MapObject, IObtainable
{

    public void Obtain()
    {
        MapModel.BackgroundColor.Value += Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
