using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorTypeExtensions
{
    public static Color ToColor(this ColorType colorType)
    {
        return colorType.Type switch
        {
            ColorTypeEnum.Cyan => Color.cyan,
            ColorTypeEnum.Magenta => Color.magenta,
            ColorTypeEnum.Yellow => Color.yellow,
            ColorTypeEnum.Red => Color.red,
            ColorTypeEnum.Green => Color.green,
            ColorTypeEnum.Blue => Color.blue,
            ColorTypeEnum.Key => Color.black,
            ColorTypeEnum.None => Color.white,
            _ => throw new System.Exception(),
        };
    }
}
