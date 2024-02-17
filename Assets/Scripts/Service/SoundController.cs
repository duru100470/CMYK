using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundController : MonoBehaviour, ISoundController
{
    [HideInInspector]
    [Inject]
    public GameSetting _setting;

    private readonly List<AudioSource> _audioSources = new List<AudioSource>();
    private readonly Dictionary<SFXType, AudioClipSetting> _audioDict = new();
    [SerializeField]
    private AudioSource _bgmAudioSource;
    private readonly HashSet<int> _usingIndex = new HashSet<int>();

    [SerializeField]
    private bool _debugMode = false;

    private float MasterVolume => _setting.MasterVolume;
    private float SFXVolume => _setting.SFXVolume;
    private float MusicVolume => _setting.MusicVolume;

    public void PlayEffect(SFXType soundName, float volume = 1f, float pitch = 1f)
    {
        int emptyAudioIndex = -1;

        for (int i = 0; i < _audioSources.Count; ++i)
        {
            if (!_usingIndex.Contains(i) && !_audioSources[i].isPlaying)
            {
                emptyAudioIndex = i;
                _usingIndex.Add(emptyAudioIndex);
                break;
            }
        }

        // 만일 모든 AudioSource가 사용중일때
        if (emptyAudioIndex < 0)
        {
            _audioSources.Add(gameObject.AddComponent<AudioSource>());
            emptyAudioIndex = _audioSources.Count - 1;
        }

        var audioSourceToUse = _audioSources[emptyAudioIndex];

        LoadAndPlaySound(audioSourceToUse, soundName, volume)
            .ContinueWith(() =>
            {
                _usingIndex.Remove(emptyAudioIndex);
            })
            .Forget();

        if (_debugMode)
            Debug.Log($"Playing Sound: {soundName}");
    }

    private async UniTask LoadAndPlaySound(AudioSource audioSource, SFXType key, float volume)
    {
        var clip = await LoadSoundAsync(key);

        audioSource.clip = clip.Clip;
        audioSource.volume = clip.Volume * volume * SFXVolume * MasterVolume;
        audioSource.Play();
    }

    private async UniTask<AudioClipSetting> LoadSoundAsync(SFXType key)
    {
        if (_audioDict.TryGetValue(key, out var clip))
            return clip;

        var loaded = await Resources.LoadAsync($"SoundClips/{key}");

        if (loaded == null)
            throw new NullReferenceException($"Cannot find the sound file: {key}");

        _audioDict[key] = loaded as AudioClipSetting;

        return loaded as AudioClipSetting;
    }

    public void PlayBGM(BGMType soundType)
    {
        _bgmAudioSource.Play();
    }

    public void PauseBGM()
    {
        _bgmAudioSource.Pause();
    }

    public void ChangeBGMVolume()
    {
        Debug.Log(MasterVolume);

        _bgmAudioSource.volume = MasterVolume * MusicVolume;
    }

    public void PauseAll()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying)
            {
                _audioSources[i].Pause();
            }
        }
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    // FOR DEBUG
    [ContextMenu("Reload Audio Clips")]
    public void ResetAudio()
    {
        _audioDict.Clear();
    }

    [ContextMenu("Play Test Sound")]
    public void Test()
        => PlayEffect(SFXType.Test);
}
