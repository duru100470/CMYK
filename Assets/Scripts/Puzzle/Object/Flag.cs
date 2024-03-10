using System.Linq;
using BasicInjector;
using MessageChannel;
using UnityEngine;

public class Flag : MapObject, IObtainable
{
    [Inject]
    public Channel<PlayerEvent> channel;

    [SerializeField]
    private GameObject _effect;
    private Flicker _flicker;

    private void Awake()
    {
        _flicker = GetComponent<Flicker>();
        _flicker.enabled = true;
        _flicker._isFlag = true;
    }

    public void Obtain()
    {
        var character = _mapModel.GetObjects()
            .First(obj => obj.Info.Type == ObjectType.Player) as Player;

        character.IsMoveable = false;

        var go = SceneLoader.Instance.CurrentSceneScope.Instantiate(_effect, transform.position, Quaternion.identity);
        go.GetComponent<ClearEffect>().Emit(character.PlayerColor.Value);

        channel.Notify(new PlayerEvent { Type = PlayerEventType.GameClear });
        _mapModel.RemoveMapObject(this);
    }
}
