using System.Collections.Generic;

public interface IMapModel
{
    ReactiveProperty<ColorType> BackgroundColor { get; }
    void AddMapObject(MapObject mapObject);
    void RemoveMapObject(MapObject mapObject);
    bool TryGetObject(Coordinate dir, out MapObject mapObject, bool ignoreColor = false);
    IEnumerable<MapObject> GetObjects(bool ignoreColor = false);
    // FIXME: BackgroundColor.OnValueChanged 이벤트가 있는데??
    public void OnColorEventOccurred(ColorChangeEvent colorChangeEvent);
}