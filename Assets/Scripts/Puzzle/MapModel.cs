using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModel
{
    private List<MapObject> _objectList = new();
    private ReactiveProperty<ColorType> _colorType;

    public ReactiveProperty<ColorType> BackgroundColor => _colorType;

    /// <summary>
    /// 맵 특정 좌표의 오브젝트 탐색
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    public bool TryGetObjectAtCoordinate(Coordinate dir, out MapObject obj, bool ignoreColor = false)
    {
        throw new NotImplementedException();
    }
}