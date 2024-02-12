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
        Debug.Log(sceneIndex);
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
                pos.x = 0f;
                _transform.position = Camera.main.ViewportToWorldPoint(pos);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                Coordinate = new Coordinate(11, -2);
                _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
                if (sceneIndex == 1)
                {
                    Destroy(gameObject);
                }
            }
            
        }
        if (pos.x > 1f)
        {
            sceneIndex = sceneIndex + 1;
            if (sceneIndex > 5)
            {
                sceneIndex = 5;
                pos.x = 1f;
                _transform.position = Camera.main.ViewportToWorldPoint(pos);
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
            pos.y = 0f;
            _transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        if (pos.y > 1f)
        {
            pos.y = 1f;
            _transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
    }
//world 선택 좌표와 상호작용 구현(MapPortal 사용)
//화면 가장자리까지 간 뒤 해당 방향으로 몇 번 이동 입력하면 반대 방향 움직임이 그 횟수만큼 씹힘
//MainScene으로 돌아갈 때 ProjectScope 2개임
}
