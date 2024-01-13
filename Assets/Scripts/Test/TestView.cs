using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class TestView : MonoBehaviour, IInitializable
{
    [Inject]
    public IMapModel mapModel;
    [Inject]
    public Channel<PlayerEvent> channel;

    public void Initialize()
    {
        mapModel.BackgroundColor.OnValueChanged += ChangeBackgroundColor;
        channel.Subscribe(OnPlayerEventOccurred);
    }

    private void OnDestroy()
    {
        mapModel.BackgroundColor.OnValueChanged -= ChangeBackgroundColor;
        channel.Unsubscribe(OnPlayerEventOccurred);
    }

    public void ChangeBackgroundColor(ColorType colorType)
    {
        Camera.main.backgroundColor = colorType.ToColor();
    }

    public void OnPlayerEventOccurred(PlayerEvent playerEvent)
    {
        switch (playerEvent.Type)
        {
            case PlayerEventType.GameClear:
                Debug.Log("Game Clear!!");
                break;
            case PlayerEventType.GameOver:
                Debug.Log("Game Over!!");
                break;
        }
    }
}