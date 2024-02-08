using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

public class MapObject : MonoBehaviour, IInitializable
{
    [Inject]
    public IMapModel MapModel;
    public ObjectInfo Info;
    public Coordinate Coordinate;

    public virtual void Init()
    {
        GetComponent<Transform>().position = Coordinate.CoordinateToWorldPoint(Coordinate);
        GetComponent<SpriteRenderer>().color = Info.Color.ToColor();
    }

    public virtual void Initialize()
    {
        Debug.Log(Info.Type);

        MapModel.BackgroundColor.OnValueChanged += OnBackgroundColorChanged;
    }

    protected virtual void OnBackgroundColorChanged(ColorType color)
    {
        GetComponent<SpriteRenderer>().sortingOrder =
            color == Info.Color ? 0 : 1;
    }

    public void DestroyObject()
    {
        if (MapModel != null)
        {
            MapModel.BackgroundColor.OnValueChanged -= OnBackgroundColorChanged;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    private void OnValidate()
    {
        Init();
    }
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
    Rock,
    Flag,
    Paint,
    Eraser
}