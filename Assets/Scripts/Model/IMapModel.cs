public interface IMapModel
{
    ReactiveProperty<ColorType> BackgroundColor { get; }
    void AddMapObject(MapObject mapObject);
    void RemoveMapObject(MapObject mapObject);
    bool TryGetObject(Coordinate dir, out MapObject mapObject, bool ignoreColor = false);
    public void OnColorEventOccurred(ColorChangeEvent colorChangeEvent);
}

// TODO: Map 관련 코드 Models 폴더로 옮기기
