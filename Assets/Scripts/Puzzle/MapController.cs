using UnityEngine;

public abstract class MapController : MonoBehaviour
{
    public abstract void InitMap(MapData mapData);
    public abstract void ResetMap();
}