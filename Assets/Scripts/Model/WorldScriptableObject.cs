using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new World", menuName = "Models/World Data")]
public class WorldScriptableObject : ScriptableObject
{
    [HideInInspector]
    [Inject]
    public GameSetting _setting;
    private bool _isLoaded = false;
    public bool IsLoaded => _isLoaded;

    public int Index;
    public List<MapInfo> Maps;
    public List<int> Requirements;

    public async UniTaskVoid LoadAsync()
    {
        await LoadClearDataAsync();
        await LoadMapDataAsync();

        _isLoaded = true;
    }

    public async UniTask LoadClearDataAsync()
    {
        await UniTask.WaitUntil(() => _setting.IsAvailable == true);

        var clearString = "";

        if (_setting.WorldClearData.Count - 1 < Index)
        {
            for (int i = 0; i < Maps.Count; i++)
                clearString += "0";

            _setting.WorldClearData.Add(clearString);
            _setting.Save();
        }
        else
        {
            clearString = _setting.WorldClearData[Index];
        }

        foreach (var map in Maps)
        {
            map.IsClear = clearString[map.Index - 1] == 1;

            var isAvailable = false;

            foreach (var r in map.Requirements)
                isAvailable &= clearString[r - 1] == 1;

            map.IsAvailable = isAvailable;
        }
    }

    public async UniTask LoadMapDataAsync()
    {
        foreach (var map in Maps)
        {
            var asset = await Resources.LoadAsync<TextAsset>($"Worlds/{Index}/{map.Index}") as TextAsset;

            var data = new MapData();
            data.ImportData(asset.text);
            map.Data = data;
        }
    }
}

[Serializable]
public class MapInfo
{
    public int Index;
    public MapData Data;
    public List<int> Requirements;
    [ReadOnly]
    public bool IsClear;
    [ReadOnly]
    public bool IsAvailable;
}