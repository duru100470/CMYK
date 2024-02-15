using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class Wall : MapObject, IInitializable
{
    [SerializeField]
    private Sprite[] _sprites;

    private bool _spriteInit = false;

    public override void Initialize()
    {
        base.Initialize();

        SpriteInit();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Initialize();
    }

   public void SpriteInit()
    {
        if (Info.SpriteIndex == 0)
            return;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _sprites[Info.SpriteIndex];
    }
}
