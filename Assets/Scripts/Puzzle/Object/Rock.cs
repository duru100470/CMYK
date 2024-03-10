using BasicInjector;
using UnityEngine;

public class Rock : MapObject, IMoveable
{
    [Inject]
    public ISoundController _soundController;

    [SerializeField]
    private GameObject _effect;
    private Transform _transform;

    public override void Start()
    {
        base.Start();
        _transform = GetComponent<Transform>();
    }

    public void Move(Coordinate dir)
    {
        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
    }

    public bool TryMove(Coordinate dir)
    {
        var target = Coordinate + dir;

        if (_mapModel.TryGetObject(target, out var obj))
        {
            if (obj.Info.IsSolidType)
                return false;
            if (obj is IMoveable)
                return false;
            if (obj is IObtainable)
            {
                var go = SceneLoader.Instance.CurrentSceneScope.Instantiate(_effect);
                go.transform.position = obj.transform.position;
                go.GetComponent<DestroyEffect>().Emit(obj.Info.Color);

                _mapModel.RemoveMapObject(obj);
                _soundController.PlayEffect(SFXType.DestroyItem, 1f, 1f);
            }
        }

        Move(dir);

        return true;
    }
}
