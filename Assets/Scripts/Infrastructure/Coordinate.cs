using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Coordinate : IEquatable<Coordinate>
{
    [SerializeField]
    private int _x;
    [SerializeField]
    private int _y;
    public readonly int X => _x;
    public readonly int Y => _y;

    public static Coordinate Zero = new(0, 0);

    public Coordinate(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public override int GetHashCode() => _x ^ _y;

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        Coordinate point = (Coordinate)obj;

        if (_x != point._x)
            return false;

        return _y == point._y;
    }

    public static bool operator ==(Coordinate c1, Coordinate c2) => c1.Equals(c2);

    public static bool operator !=(Coordinate c1, Coordinate c2) => !c1.Equals(c2);

    public static Coordinate operator +(Coordinate c1, Coordinate c2) => new(c1.X + c2.X, c1.Y + c2.Y);

    public static Coordinate operator -(Coordinate c1, Coordinate c2) => new(c1.X - c2.X, c1.Y - c2.Y);

    public static int Distance(Coordinate c1, Coordinate c2) => Mathf.Abs(c1.X - c2.X) + Mathf.Abs(c1.Y - c2.Y);

    /// <summary>
    /// Change transition.position to Coordinate
    /// </summary>
    public static Coordinate WorldPointToCoordinate(Vector3 point)
    {
        return new Coordinate(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
    }

    /// <summary>
    /// Change Coordinate to transition.position
    /// </summary>
    public static Vector2 CoordinateToWorldPoint(Coordinate coor)
    {
        return new Vector2(coor.X + 0.5f, coor.Y + 0.5f);
    }

    public bool Equals(Coordinate other)
        => _x == other.X && _y == other.Y;
}