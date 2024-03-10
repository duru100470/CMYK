using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using Cysharp.Threading.Tasks;

public class Paint : MapObject, IObtainable
{
    public void Obtain()
    {
        _mapModel.BackgroundColor.Value += Info.Color;

        _mapModel.RemoveMapObject(this);
    }
}
