using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainPlayer : MapObject, IInitializable
{
    private Transform _transform;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
    }

    void OnBecameInvisible()    //각 world마다 WorldScene을 만들고 그 스크립트에 넣어야 할 부분 + 추가로 전 씬으로 되돌아오는 기능 구현, 위아래로 못 나가게 구현, world 선택 좌표와 상호작용 구현
    {
        SceneManager.LoadScene("World1");
        Coordinate = new Coordinate(-11, -2);
        _transform.position = Coordinate.CoordinateToWorldPoint(Coordinate);
    }
}
