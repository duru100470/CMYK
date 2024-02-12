using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PortalAnimation : MonoBehaviour
{
    private bool _onAni = false;
    private float _speed = 5.0f;

    public void AnimationOn()
    {
        _onAni = true;
    }

    public void AnimationOff()
    {
        _onAni= false;
    }

    private void Update()
    {
        Vector3 targetPos;
        Vector3 targetScale;
        if (_onAni)
        {
            targetPos = new Vector3(0, 1, 0);
            targetScale = new Vector3(1, 1, 1);
        }
        else
        {
            targetPos = new Vector3(0, 0, 0);
            targetScale = new Vector3(0, 0, 0);
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * _speed);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * _speed);
    }
}
