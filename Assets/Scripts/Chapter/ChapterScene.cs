using BasicInjector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterScene : MonoBehaviour
{
    [Inject]
    public GameSetting _gameSetting;
    [Inject]
    public WorldLoader _worldLoader;
    private async void Start()
    {
        await _gameSetting.LoadAsync();
        await _worldLoader.InitWorlds(_gameSetting);
    }
}
