using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class TestView : MonoBehaviour, IInitializable
{
    [Inject]
    public IMapModel mapModel;

    public void Initialize()
    {
        mapModel.BackgroundColor.OnValueChanged += ChangeBackgroundColor;
    }

    private void OnDestroy()
    {
        mapModel.BackgroundColor.OnValueChanged -= ChangeBackgroundColor;
    }

    public void ChangeBackgroundColor(ColorType colorType)
    {
        Camera.main.backgroundColor = colorType.ToColor();
    }
}