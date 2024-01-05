public interface IMapModel
{
    ReactiveProperty<ColorType> BackgroundColor { get; }
    void AddMapObject(MapObject mapObject);
    void RemoveMapObject(MapObject mapObject);
    bool TryGetObject(Coordinate dir, out MapObject mapObject, bool ignoreColor = false);
}