using System;

public struct ColorType : IEquatable<ColorType>
{
    private ColorTypeEnum _type;
    public ColorTypeEnum Type => _type;

    public ColorType(ColorTypeEnum type)
    {
        _type = type;
    }

    public ColorType(ColorType color)
    {
        _type = color.Type;
    }

    public void SetColor(ColorType color)
    {
        _type = color.Type;
    }

    public void SetColor(ColorTypeEnum type)
    {
        _type = type;
    }

    public void AddColor(ColorTypeEnum type)
    {
        var (cc, cm, cy) = ColorTypeToFlag(_type);
        var (nc, nm, ny) = ColorTypeToFlag(type);
        var ret = (cc | nc, cm | nm, cy | ny);
        _type = FlagToColorType(ret);
    }

    public void AddColor(ColorType color)
        => AddColor(color.Type);

    public void RemoveColor(ColorTypeEnum type)
    {
        var (cc, cm, cy) = ColorTypeToFlag(_type);
        var (nc, nm, ny) = ColorTypeToFlag(type);
        var ret = ((cc ^ nc) & (!nc), (cm ^ nm) & (!nm), (cy ^ ny) & (!ny));
        _type = FlagToColorType(ret);
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
            _ => throw new Exception(),
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

    public bool Equals(ColorType other) => other._type == _type;

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
    Cyan,
    Magenta,
    Yellow,
    Red,
    Green,
    Blue,
    Key
}