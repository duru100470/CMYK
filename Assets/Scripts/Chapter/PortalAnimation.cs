using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    private bool _onAni = false;
    private bool _init = false;
    private float _speed = 5.0f;

    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
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
            _spriteRenderer.enabled = true;
            targetPos = new Vector3(0, 1, 0);
            targetScale = new Vector3(1, 1, 1);
            if(!_init)
            {
                _init = true;
                transform.localPosition = new Vector3(0, 0.5f, 0);
            }
        }
        else
        {
            targetPos = new Vector3(0, 0, 0);
            targetScale = new Vector3(0, 0, 0);
        }
        if(transform.localPosition.y < 0.5)
        {
            _init = false;
            _spriteRenderer.enabled = false;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * _speed);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * _speed);
        }
    }
}
