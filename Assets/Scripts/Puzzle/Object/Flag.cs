using BasicInjector;
using MessageChannel;
using UnityEngine;

public class Flag : MapObject, IObtainable
{
    [Inject]
    public Channel<PlayerEvent> channel;

    public void Obtain()
    {
        channel.Notify(new PlayerEvent { Type = PlayerEventType.GameClear });
        MapModel.RemoveMapObject(this);
    }
}
