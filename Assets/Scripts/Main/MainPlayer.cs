using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainPlayer : MapObject, IInitializable
{
    private Transform _transform;
    private int sceneIndex;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
                Coordinate = new Coordinate(11, -2);
                _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
                if (sceneIndex == 1)
                {
                    Destroy(gameObject);
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
                Coordinate = new Coordinate(-11, -2);
                _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
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
//world 선택 좌표와 상호작용 구현(MapPortal 사용)
}
