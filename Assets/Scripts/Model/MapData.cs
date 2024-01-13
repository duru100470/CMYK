using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapData
{
    public List<(Coordinate, ObjectInfo)> MapObjects = new();
    public ColorType InitColor;

    public void ImportData(string json)
    {
        MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

        MapObjects.Clear();

        MapObjects.AddRange(mapData.MapObjects);
        InitColor = mapData.InitColor;
    }

    public string ExportData()
    {
        string json = JsonConvert.SerializeObject(this);

        return json;
    }
}