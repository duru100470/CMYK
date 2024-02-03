using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageChannel;
using Cysharp.Threading.Tasks;

public class Key : MapObject, IObtainable
{
    [Inject]
    public MessageChannel.Channel<ColorChangeEvent> colorChannel;
    public void Obtain()
    {
        foreach(var keyDoor in MapModel.GetObjectsByInfo(new ObjectInfo {Type = ObjectType.KeyDoor , Color = Info.Color}))
        {
            MapModel.RemoveMapObject(keyDoor);
        }

        MapModel.RemoveMapObject(this);
    }
}