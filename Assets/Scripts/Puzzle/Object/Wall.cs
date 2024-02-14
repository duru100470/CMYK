using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class Wall : MapObject, IInitializable
{
    [SerializeField]
    private Sprite[] _sprites;

    public override void Initialize()
    {
        base.Initialize();

        if (Info.SpriteIndex == 0)
            return;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _sprites[Info.SpriteIndex];
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Initialize();
    }
}