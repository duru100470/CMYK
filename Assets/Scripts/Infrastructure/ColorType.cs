using System;

[Serializable]
public struct ColorType : IEquatable<ColorType>
{
    public ColorTypeEnum Type;

    public ColorType(ColorTypeEnum type)
    {
        Type = type;
    }

    public ColorType(ColorType color)
    {
        Type = color.Type;
    }

    public void SetColor(ColorType color)
    {
        Type = color.Type;
    }

    public void SetColor(ColorTypeEnum type)
    {
        Type = type;
    }

    public void AddColor(ColorTypeEnum type)
    {
        var (cc, cm, cy) = ColorTypeToFlag(Type);
        var (nc, nm, ny) = ColorTypeToFlag(type);
        var ret = (cc | nc, cm | nm, cy | ny);
        Type = FlagToColorType(ret);
    }

    public void AddColor(ColorType color)
        => AddColor(color.Type);

    public void RemoveColor(ColorTypeEnum type)
    {
        var (cc, cm, cy) = ColorTypeToFlag(Type);
        var (nc, nm, ny) = ColorTypeToFlag(type);
        var ret = ((cc ^ nc) & (!nc), (cm ^ nm) & (!nm), (cy ^ ny) & (!ny));
        Type = FlagToColorType(ret);
    }

    public void RemoveColor(ColorType color)
        => RemoveColor(color.Type);

    private (bool, bool, bool) ColorTypeToFlag(ColorTypeEnum type)
    {
        return type switch
        {
            ColorTypeEnum.Cyan => (true, false, false),
            ColorTypeEnum.Magenta => (false, true, false),
            ColorTypeEnum.Yellow => (false, false, true),
            ColorTypeEnum.Red => (false, true, true),
            ColorTypeEnum.Green => (true, false, true),
            ColorTypeEnum.Blue => (true, true, false),
            ColorTypeEnum.Key => (true, true, true),
            ColorTypeEnum.None => (false, false, false),
            _ => throw new Exception(),
        };
    }

    private ColorTypeEnum FlagToColorType((bool, bool, bool) flag)
    {
        return flag switch
        {
            (true, false, false) => ColorTypeEnum.Cyan,
            (false, true, false) => ColorTypeEnum.Magenta,
            (false, false, true) => ColorTypeEnum.Yellow,
            (false, true, true) => ColorTypeEnum.Red,
            (true, false, true) => ColorTypeEnum.Green,
            (true, true, false) => ColorTypeEnum.Blue,
            (true, true, true) => ColorTypeEnum.Key,
            (false, false, false) => ColorTypeEnum.None,
        };
    }

    public override bool Equals(object other)
    {
        if (other == null)
            return false;

        if (other is not ColorType)
            return false;

        return this.Equals(other);
    }

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(ColorType other) => other.Type == Type;

    public static bool operator ==(ColorType c1, ColorType c2) => c1.Equals(c2);

    public static bool operator !=(ColorType c1, ColorType c2) => !c1.Equals(c2);

    public static ColorType operator +(ColorType c1, ColorType c2)
    {
        var ret = new ColorType(c1);
        ret.AddColor(c2);
        return ret;
    }

    public static ColorType operator -(ColorType c1, ColorType c2)
    {
        var ret = new ColorType(c1);
        ret.RemoveColor(c2);
        return ret;
    }

    public static implicit operator ColorTypeEnum(ColorType color) => color.Type;

    public override string ToString() => Type.ToString();
}

public enum ColorTypeEnum
{
    None,
    Cyan,
    Magenta,
    Yellow,
    Red,
    Green,
    Blue,
    Key
}