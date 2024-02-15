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

    [SerializeField]
    private GameObject _effect;
    private ColorType _myColorType;

    public void Initialize()
    {
        mapModel.BackgroundColor.OnValueChanged += ChangeBackgroundColor;
        _myColorType = mapModel.BackgroundColor.Value;
        channel.Subscribe(OnPlayerEventOccurred);
    }

    private void OnDestroy()
    {
        mapModel.BackgroundColor.OnValueChanged -= ChangeBackgroundColor;
        channel.Unsubscribe(OnPlayerEventOccurred);
    }

    public void ChangeBackgroundColor(ColorType colorType)
    {
        if (colorType == _myColorType)
            return;
        else
            _myColorType = colorType;
        StartCoroutine(ChangeColorAnimation(colorType));
    }

    private IEnumerator ChangeColorAnimation(ColorType color)
    {
        var go = SceneLoader.Instance.CurrentSceneScope.Instantiate(_effect, transform);
        go.GetComponent<PaintEffect>().Play(color);

        yield return new WaitForSeconds(1f);

        Camera.main.backgroundColor = color.ToColor();
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
