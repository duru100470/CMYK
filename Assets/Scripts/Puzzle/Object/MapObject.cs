using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Inject]
    public IMapModel MapModel;
    public ObjectInfo Info;
    public Coordinate Coordinate;
}

/// <summary>
/// 퍼즐 오브젝트 저장 데이터 형식
/// </summary>
[Serializable]
public struct ObjectInfo
{
    public ObjectType Type;
    public ColorType Color;
}

public enum ObjectType
{
    Player,
    Wall,
    Movable,
    Paint,
    Eraser
}