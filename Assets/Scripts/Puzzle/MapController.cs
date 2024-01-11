using BasicInjector;
using UnityEngine;

public abstract class MapController : MonoBehaviour
{
    [Inject]
    public IMapModel mapModel;
    [Inject]
    public MapData mapData;
    [Inject]
    public AssetLoader assetLoader;

    [SerializeField]
    protected Transform _puzzle;

    public abstract void InitMap();
    public abstract void ResetMap();

    protected void GenerateMapFromData()
    {
        foreach (var (coor, info) in mapData.MapObjects)
        {
            Debug.Log($"MapObjects/{info.Type}");

            var go =
                SceneLoader.Instance.CurrentSceneScope.Instantiate(assetLoader.LoadPrefab<GameObject>($"MapObjects/{info.Type}"), _puzzle);

            var mo = go.GetComponent<MapObject>();
            mo.Coordinate = coor;
            mo.Info = info;
            mo.Init();

            mapModel.AddMapObject(mo);
            Debug.Log($"Create MapObject! [{mo.Coordinate}, {mo.Info.Type}]");
        }

        mapModel.BackgroundColor.Value = mapData.InitColor;
    }
}