using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class Wall : MapObject
{
    [SerializeField]
    private Sprite[] _sprites;

    public override void Init()
    {
        base.Init();

        SpriteInit();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Init();
    }

    public void SpriteInit()
    {
        if (Info.SpriteIndex == 0)
            return;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _sprites[Info.SpriteIndex];
    }
}
