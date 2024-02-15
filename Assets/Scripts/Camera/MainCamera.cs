using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BuildReportTool;
using UnityEngine.TextCore.Text;

public class MainCamera : MonoBehaviour
{

    public IMapModel MapModel;
    public MainPlayer _mainPlayer;
    public int cameraMax = 90;

    private float _speed = 2.0f;

    void FixedUpdate()
    {
        Vector3 targetPos;
        if (_mainPlayer.Coordinate.X <= -1)
        {
            targetPos = new Vector3(0.5f, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= cameraMax)
        {
            targetPos = new Vector3(80, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 11 && _mainPlayer.Coordinate.X < 31)
        {
            targetPos = new Vector3(20, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 31 && _mainPlayer.Coordinate.X < 51)
        {
            targetPos = new Vector3(40, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 51 && _mainPlayer.Coordinate.X < 71)
        {
            targetPos = new Vector3(60, 0.5f, -10);
        }
        else if (_mainPlayer.Coordinate.X >= 71)
        {
            targetPos = new Vector3(80, 0.5f, -10);
        }
        else
        {
            targetPos = new Vector3(0.5f, 0.5f, -10);
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _speed);

    }
}
