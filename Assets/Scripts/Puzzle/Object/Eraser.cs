using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using BasicInjector;

public class Eraser : MapObject, IObtainable
{
    public void Obtain()
    {
        _mapModel.BackgroundColor.Value -= Info.Color;

        _mapModel.RemoveMapObject(this);
    }
}
