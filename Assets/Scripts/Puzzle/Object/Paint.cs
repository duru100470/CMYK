using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using Cysharp.Threading.Tasks;

public class Paint : MapObject, IObtainable
{
    [Inject]
    public MessageChannel.Channel<ColorChangeEvent> colorChannel;
    public void Obtain()
    {
        colorChannel.Notify(new ColorChangeEvent { ChangColor = (MapModel.BackgroundColor.Value + Info.Color) });
        MapModel.BackgroundColor.Value += Info.Color;

        MapModel.RemoveMapObject(this);
    }
}
