using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.TextCore.Text;

public class MainCamera : MonoBehaviour
{

    public MainPlayer _mainPlayer;
    public int CameraMax = 90;

    private float _speed = 2.0f;

    void FixedUpdate()
    {
        Vector3 targetPos;
        if (_mainPlayer.Coordinate.X <= -1)
        {
            targetPos = new Vector3(0.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= CameraMax)
        {
            targetPos = new Vector3(76.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 10 && _mainPlayer.Coordinate.X < 29)
        {
            targetPos = new Vector3(19.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 29 && _mainPlayer.Coordinate.X < 48)
        {
            targetPos = new Vector3(38.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 48 && _mainPlayer.Coordinate.X < 67)
        {
            targetPos = new Vector3(57.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 67)
        {
            targetPos = new Vector3(76.5f, 0.5f, -10);
        }
        else
        {
            targetPos = new Vector3(0.5f, 0.5f, -10);
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _speed);

    }
}
