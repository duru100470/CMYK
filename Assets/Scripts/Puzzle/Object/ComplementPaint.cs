using BasicInjector;

public class ComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        _mapModel.BackgroundColor.Value = _mapModel.BackgroundColor.Value.GetComplementColor();

        _mapModel.RemoveMapObject(this);
    }
}
