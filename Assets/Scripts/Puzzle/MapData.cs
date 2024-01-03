using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    private List<MapObject> _objectList = new();

    public bool TryGetObjectAtCoordinate(Coordinate dir, out MapObject obj)
    {
        throw new NotImplementedException();
    }
}