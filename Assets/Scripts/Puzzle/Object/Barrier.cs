using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class Barrier : MapObject
{
    public override void Init()
    {
        base.Init();

        GetComponent<SpriteRenderer>().color = Color.clear;
    }
}