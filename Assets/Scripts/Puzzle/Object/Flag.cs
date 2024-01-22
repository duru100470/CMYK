using BasicInjector;
using MessageChannel;
using UnityEngine;

public class Flag : MapObject, IObtainable
{
    [Inject]
    public ISoundController _soundController;
    [Inject]
    public Channel<PlayerEvent> channel;

    public void Obtain()
    {
        _soundController.PlayEffect(SFXType.GameClear, 1.0f, 1.0f);

        channel.Notify(new PlayerEvent { Type = PlayerEventType.GameClear });
        MapModel.RemoveMapObject(this);
    }
}
