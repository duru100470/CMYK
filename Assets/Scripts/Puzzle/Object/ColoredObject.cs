using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColoredObject : MonoBehaviour
{
    public ObjectType Type;
    public ColorType Color;
    public Coordinate Coordinate;
}

public enum ObjectType
{
    Player,
    Wall,
    Paint,
    Eraser
}