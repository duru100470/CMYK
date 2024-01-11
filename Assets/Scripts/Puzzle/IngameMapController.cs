using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class IngameMapController : MapController
{
    public void Initialize()
    {
        InitMap();
    }

    public override void InitMap()
    {
        GenerateMapFromData();
    }

    public override void ResetMap()
    {
        int children = _puzzle.transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            var go = _puzzle.GetChild(i).gameObject;
            mapModel.RemoveMapObject(go.GetComponent<MapObject>());
            Destroy(go);
        }
    }
}
