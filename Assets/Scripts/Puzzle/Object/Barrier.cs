using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class Barrier : MapObject, IInitializable
{
    public override void Initialize()
    {
        base.Initialize();

        GetComponent<SpriteRenderer>().color = Color.clear;
    }
}