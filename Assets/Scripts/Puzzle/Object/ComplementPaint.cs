using BasicInjector;

public class ComplementPaint : MapObject, IObtainable
{
    [Inject]
    public MessageChannel.Channel<ColorChangeEvent> colorChannel;
    public void Obtain()
    {
        colorChannel.Notify(new ColorChangeEvent { ChangColor = (MapModel.BackgroundColor.Value.GetComplementColor()) });
        MapModel.BackgroundColor.Value = MapModel.BackgroundColor.Value.GetComplementColor();

        MapModel.RemoveMapObject(this);
    }
}
