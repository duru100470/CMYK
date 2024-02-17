using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [HideInInspector]
    [Inject]
    public GameSetting _setting;
    [Inject]
    public ISoundController _soundController;

    [SerializeField]
    private Slider _masterVolume;
    [SerializeField]
    private Slider _musicVolume;
    [SerializeField]
    private Slider _sfxVolume;

    private void OnEnable()
    {
        _masterVolume.value = _setting.MasterVolume;
        _musicVolume.value = _setting.MusicVolume;
        _sfxVolume.value = _setting.SFXVolume;
    }

    public void DisablePanel()
        => gameObject.SetActive(false);

    public void ChangeMasterVolume(float value)
    {
        _setting.MasterVolume = value;
        _setting.Save();

        _soundController.ChangeBGMVolume();
    }

    public void ChangeMusicVolume(float value)
    {
        _setting.MusicVolume = value;
        _setting.Save();

        _soundController.ChangeBGMVolume();
    }

    public void ChangeSFXVolume(float value)
    {
        _setting.SFXVolume = value;
        _setting.Save();
    }
}
