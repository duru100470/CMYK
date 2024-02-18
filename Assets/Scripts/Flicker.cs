using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    public float delay = 0.5f;
    private IEnumerator enumerator;
    public bool _isMoving = false;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        StartMethod();
    }

    void StartMethod()
    {
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