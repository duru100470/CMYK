using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using BasicInjector;

public class Eraser : MapObject, IObtainable
{
    [Inject]
    public ISoundController _soundController;

    public void Obtain()
    {
        _soundController.PlayEffect(SFXType.ObtainEraser, 1.0f, 1.0f);

        MapModel.BackgroundColor.Value -= Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
