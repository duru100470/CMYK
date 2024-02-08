using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using BasicInjector;

public class Eraser : MapObject, IObtainable
{
    public void Obtain()
    {
        MapModel.BackgroundColor.Value -= Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
