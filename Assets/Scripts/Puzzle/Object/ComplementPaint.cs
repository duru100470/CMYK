using BasicInjector;

public class ComplementPaint : MapObject, IObtainable
{
    public void Obtain()
    {
        MapModel.BackgroundColor.Value = MapModel.BackgroundColor.Value.GetComplementColor();

        MapModel.RemoveMapObject(this);
    }
}
