using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapData
{
    public List<(Coordinate, ObjectInfo)> MapObjects = new();
    public ColorType InitColor;

    public void ImportData(string json)
    {
        MapData mapData = JsonUtility.FromJson<MapData>(json);
    }

    public string ExportData()
    {
        string json = JsonUtility.ToJson(this);

        return json;
    }
}