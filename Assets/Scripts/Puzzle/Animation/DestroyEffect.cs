using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void Emit(ColorType color)
    {
        var col = _particleSystem.colorOverLifetime;
        col.enabled = true;

        Gradient grad = new Gradient();
        var c = color.ToColor();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(c, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        col.color = grad;

        _particleSystem.Emit(20);
    }

    [ContextMenu("Test")]
    public void Test()
        => Emit(ColorType.Blue);
}