using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public ObjectInfo Info;
    public ColorType Color;
    public Coordinate Coordinate;
    // TODO: Injection 어캐 할 지 생각해야함
    protected MapModel _mapModel;
}

/// <summary>
/// 퍼즐 오브젝트 저장 데이터 형식
/// </summary>
public struct ObjectInfo
{
    public ObjectType Type;
}

public enum ObjectType
{
    Player,
    Wall,
    Paint,
    Eraser
}