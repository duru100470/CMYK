using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MapObject, IObtainable
{
    public void Obtain()
    {
        _mapModel.BackgroundColor.Value -= Info.Color;

        _mapModel.RemoveMapObject(this);
    }
}
