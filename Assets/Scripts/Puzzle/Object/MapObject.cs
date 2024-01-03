using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObject : MonoBehaviour
{
    public ObjectType Type;
    public ColorType Color;
    public Coordinate Coordinate;
    protected MapData _mapData;
}

public enum ObjectType
{
    Player,
    Wall,
    Paint,
    Eraser
}