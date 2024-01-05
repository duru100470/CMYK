using System;
using System.Collections;
using System.Collections.Generic;

public class MapData
{
    public List<(Coordinate, ObjectInfo)> MapObjects = new();
    public ColorType InitColor;

    public void ImportData(string json)
    {
        throw new NotImplementedException();
    }

    public string ExportData()
    {
        throw new NotImplementedException();
    }
}