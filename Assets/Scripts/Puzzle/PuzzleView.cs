using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleView : MonoBehaviour
{
    [Inject]
    public ISoundController _soundController;

    [SerializeField]
    private Button _info;
    [SerializeField]
    private GameObject _infoImage;

    private bool _isInfoEnable = false;

    private void Awake()
    {
        _info.onClick.AddListener(ToggleInfoButton);
    }

    private void ToggleInfoButton()
    {
        _isInfoEnable = !_isInfoEnable;
        _infoImage.SetActive(_isInfoEnable);

        _soundController.PlayEffect(SFXType.PlayerInteract, 1f, 1f);
    }
}