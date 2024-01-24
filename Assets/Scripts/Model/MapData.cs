using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapData
{
    public List<(Coordinate, ObjectInfo)> MapObjects = new();
    public List<(Coordinate, string)> DecorationObjects = new();
    public ColorType InitColor;

    public void ImportData(string json)
    {
        MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

        MapObjects.Clear();
        MapObjects.AddRange(mapData.MapObjects);

        DecorationObjects.Clear();
        DecorationObjects.AddRange(mapData.DecorationObjects);

        InitColor = mapData.InitColor;
    }

    public string ExportData()
    {
        string json = JsonConvert.SerializeObject(this);

        return json;
    }
}