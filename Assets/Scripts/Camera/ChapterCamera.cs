using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BuildReportTool;
using UnityEngine.TextCore.Text;

public class ChapterCamera : MonoBehaviour
{

    public IMapModel MapModel;
    public Player _player;
    public int cameraMax = 20;

    private float _speed = 2.0f;

    void FixedUpdate()
    {
        Vector3 targetPos;
        if (_player.Coordinate.X <= -1)
        {
            targetPos = new Vector3(0, 0, -10);
        }
        else if (_player.Coordinate.X >= cameraMax)
        {
            targetPos = new Vector3(cameraMax, 0, -10);
        }
        else
        {
            targetPos = new Vector3(_player.transform.position.x, 0, -10);
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _speed);

    }
}
