using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    public float delay = 0.5f;
    public int repeat = 4; //반복 횟수
    int value = 0;
    private IEnumerator enumerator;
    public bool _isMoving = false;
    public bool _isFlag = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartMethod();
    }

    void StartMethod()
    {
        value = repeat;
        enumerator = FlickerCoroution();
        StartCoroutine(enumerator);
    }

    void StopMethod()
    {
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
        }
    }

    IEnumerator FlickerCoroution()
    {
        if (_isFlag)
        {
            if(value > 0)
            {
                value -= 1;
            }
            else
            {
                yield break;
            }
        }

        if(_isMoving)
        {
            yield break;
        }

        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);

        yield return new WaitForSeconds(delay);

        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);

        yield return new WaitForSeconds(delay);

        StartCoroutine(FlickerCoroution());
    }
}