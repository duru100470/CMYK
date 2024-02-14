using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using MessageChannel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainPlayer : MapObject, IInitializable
{
    private Transform _transform;
    private int sceneIndex;
    [Inject]
    public Channel<PlayerMoveEvent> moveChannel;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public override void Initialize()
    {
        base.Initialize();
        _transform = GetComponent<Transform>();
    }

    private void OnMoveUp()
        => Move(new Coordinate(0, 1));

    private void OnMoveDown()
        => Move(new Coordinate(0, -1));

    private void OnMoveLeft()
        => Move(new Coordinate(-1, 0));

    private void OnMoveRight()
        => Move(new Coordinate(1, 0));

    private void Move(Coordinate dir)
    {
        moveChannel.Notify(new PlayerMoveEvent { Type = PlayerMoveEventType.TrueMove });

        var target = Coordinate + dir;
        if (MapModel.TryGetObject(target, out var obj))
        {
            if (obj is IObtainable)
            {
                var obtainableObj = obj as IObtainable;
                obtainableObj.Obtain();
            }
        }

        Coordinate += dir;
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);

        Vector3 pos = Camera.main.WorldToViewportPoint(_transform.position);

        if (pos.x < 0f)
        {
            sceneIndex = sceneIndex - 1;
            if (sceneIndex < 1)
            {
                sceneIndex = 1;
                Coordinate += new Coordinate(1, 0);
                _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                if (sceneIndex == 1)
                {
                    GameObject _sceneLoader = GameObject.Find("SceneLoader");
                    Destroy(_sceneLoader);
                }
            }
            
        }
        if (pos.x > 1f)
        {
            sceneIndex = sceneIndex + 1;
            if (sceneIndex > 5)
            {
                sceneIndex = 5;
                Coordinate += new Coordinate(-1, 0);
                _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
            }
        }
        if (pos.y < 0f)
        {
            Coordinate += new Coordinate(0, 1);
            _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
        }
        if (pos.y > 1f)
        {
            Coordinate += new Coordinate(0, -1);
            _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
        }
    }
}
