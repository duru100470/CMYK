using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapModel : IMapModel
{
    private List<MapObject> _objectList = new();
    private ReactiveProperty<ColorType> _colorType = new();

    public ReactiveProperty<ColorType> BackgroundColor => _colorType;

    public void AddMapObject(MapObject mapObject)
    {
        _objectList.Add(mapObject);
    }

    public void RemoveMapObject(MapObject mapObject)
    {
        _objectList.Remove(mapObject);
        mapObject.DestroyObject();

    }

    /// <summary>
    /// 맵 특정 좌표의 오브젝트 탐색
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    public bool TryGetObject(Coordinate dir, out MapObject obj, bool ignoreColor = false)
    {
        var candidates = _objectList.Where(obj => obj.Coordinate == dir);
        obj = null;

        foreach (var target in candidates)
        {
            if (target.Info.Color != BackgroundColor.Value && !ignoreColor)
            {
                obj = target;
                return true;
            }
        }

        return false;
    }
}