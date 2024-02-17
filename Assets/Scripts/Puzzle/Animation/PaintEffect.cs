using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintEffect : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer[] _spriteRenderers;

    public void Play(ColorType color)
    {
        foreach (var sr in _spriteRenderers)
        {
            sr.color = color.ToColor();
        }

        _animator.Play("Paint");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    [ContextMenu("Test")]
    public void Test()
        => Play(ColorType.Cyan);
}