using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool _hasHoldInput = false;
    private ButtonType _buttonType = ButtonType.None;
    [SerializeField]
    private float _holdTime = 1.5f;
    [SerializeField]
    private float _moveDelay = .2f;
    private IEnumerator _holdCoroutine;

    public event Action<Coordinate> OnPlayerMove;

    private void OnMoveUp()
    {
        Move(new Coordinate(0, 1));

        if (_buttonType == ButtonType.None && _hasHoldInput)
        {
            _buttonType = ButtonType.Up;
            _holdCoroutine = InputHoldAction();
            StartCoroutine(_holdCoroutine);
        }
    }

    private void OnMoveDown()
    {
        Move(new Coordinate(0, -1));

        if (_buttonType == ButtonType.None && _hasHoldInput)
        {
            _buttonType = ButtonType.Down;
            _holdCoroutine = InputHoldAction();
            StartCoroutine(_holdCoroutine);
        }
    }

    private void OnMoveLeft()
    {
        Move(new Coordinate(-1, 0));

        if (_buttonType == ButtonType.None && _hasHoldInput)
        {
            _buttonType = ButtonType.Left;
            _holdCoroutine = InputHoldAction();
            StartCoroutine(_holdCoroutine);
        }
    }

    private void OnMoveRight()
    {
        Move(new Coordinate(1, 0));

        if (_buttonType == ButtonType.None && _hasHoldInput)
        {
            _buttonType = ButtonType.Right;
            _holdCoroutine = InputHoldAction();
            StartCoroutine(_holdCoroutine);
        }
    }

    private void Move(Coordinate dir)
    {
        OnPlayerMove?.Invoke(dir);
    }

    private IEnumerator InputHoldAction()
    {
        int timer = 0;
        float cnt = _holdTime / _moveDelay;

        while (_buttonType != ButtonType.None)
        {
            if (timer >= cnt)
            {
                var coor = _buttonType switch
                {
                    ButtonType.Up => new Coordinate(0, 1),
                    ButtonType.Down => new Coordinate(0, -1),
                    ButtonType.Left => new Coordinate(-1, 0),
                    ButtonType.Right => new Coordinate(1, 0),
                    _ => throw new Exception()
                };

                Move(coor);
            }

            yield return new WaitForSeconds(_moveDelay);
            timer++;
        }
    }

    private void OnMoveUpRelease()
    {
        _buttonType = ButtonType.None;

        if (_hasHoldInput)
            StopCoroutine(_holdCoroutine);
    }

    private void OnMoveDownRelease()
    {
        _buttonType = ButtonType.None;

        if (_hasHoldInput)
            StopCoroutine(_holdCoroutine);
    }

    private void OnMoveLeftRelease()
    {
        _buttonType = ButtonType.None;

        if (_hasHoldInput)
            StopCoroutine(_holdCoroutine);
    }

    private void OnMoveRightRelease()
    {
        _buttonType = ButtonType.None;

        if (_hasHoldInput)
            StopCoroutine(_holdCoroutine);
    }

    private enum ButtonType
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}
