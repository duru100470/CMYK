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
        string path = Application.persistentDataPath + "/mapdata.json";

        if (!File.Exists(path))
        {
            return;
        }

        json = File.ReadAllText(path);
        MapData mapData = JsonUtility.FromJson<MapData>(json);
    }

    public string ExportData()
    {
        string json = JsonUtility.ToJson(this);

        File.WriteAllText(Application.persistentDataPath + "/mapdata.json", json);

        return json;
    }
}