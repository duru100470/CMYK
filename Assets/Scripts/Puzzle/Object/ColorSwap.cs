using System.Linq;
using BasicInjector;
public class ColorSwap : MapObject, IObtainable
{
    [Inject]
    public MessageChannel.Channel<ColorChangeEvent> colorChannel;
    public void Obtain()
    {
        var character = MapModel.GetObjects(new ObjectInfo { Type = ObjectType.Player }, true).First();

        colorChannel.Notify(new ColorChangeEvent { ChangColor = character.Info.Color });

        if (character is Player)
            (character as Player).SwapColorWithBackground();

        MapModel.RemoveMapObject(this);
    }
}
