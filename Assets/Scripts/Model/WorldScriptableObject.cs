using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BasicInjector;
using BuildReportTool.Window;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new World", menuName = "Models/World Data")]
public class WorldScriptableObject : ScriptableObject
{
    [HideInInspector]
    private GameSetting _setting;
    private bool _isLoaded = false;
    public bool IsLoaded => _isLoaded;

    // TODO: 월드 해금 조건 만들어야 함
    public bool IsAvailable => true;

    [SerializeField]
    private int _index;
    [SerializeField]
    private List<Map> _maps;
    [SerializeField]
    private List<int> _requirements;

    public int MapLength => _maps.Count;

    public async UniTask LoadAsync(GameSetting setting)
    {
        _setting = setting;

        await LoadClearDataAsync();
        await LoadMapDataAsync();

        _isLoaded = true;
    }

    public bool TryGetMapData(int index, out MapData mapData)
    {
        if (_isLoaded && _maps[index].IsAvailable)
        {
            mapData = _maps[index].Data;
            return true;
        }

        mapData = null;
        return false;
    }

    public async UniTask UpdateClearDataAsync(int index, bool isClear = true)
    {
        _isLoaded = false;

        var clearString = _setting.WorldClearData[_index];
        var nextState = isClear ? '1' : '0';

        clearString = new StringBuilder(clearString)
            .Remove(index, 1)
            .Insert(index, nextState)
            .ToString();

        _setting.WorldClearData[_index] = clearString;
        _setting.Save();

        await LoadClearDataAsync();

        _isLoaded = true;
    }

    public MapStatus GetStatus(int index)
    {
        if (_maps[index].IsClear)
            return MapStatus.Cleared;
        else if (_maps[index].IsAvailable)
            return MapStatus.Available;
        else
            return MapStatus.Locked;
    }

    public async UniTask LoadClearDataAsync()
    {
        await UniTask.WaitUntil(() => _setting.IsAvailable == true);

        var clearString = "";

        if (_setting.WorldClearData.Count - 1 < _index)
        {
            for (int i = 0; i < _maps.Count; i++)
                clearString += "0";

            _setting.WorldClearData.Add(clearString);
            _setting.Save();
        }
        else
        {
            clearString = _setting.WorldClearData[_index];
        }

        foreach (var map in _maps)
        {
            map.IsClear = clearString[map.Index] == '1';

            var isAvailable = true;

            foreach (var r in map.Requirements)
                isAvailable &= clearString[r] == '1';

            map.IsAvailable = isAvailable;
        }
    }

    private async UniTask LoadMapDataAsync()
    {
        foreach (var map in _maps)
        {
            var asset = await Resources.LoadAsync<TextAsset>($"Worlds/{_index + 1}/{map.Index + 1}") as TextAsset;

            var data = new MapData();
            data.ImportData(asset.text);
            map.Data = data;
        }
    }

    [Serializable]
    private class Map
    {
        public int Index;
        public MapData Data;
        public List<int> Requirements;
        [HideInInspector]
        public bool IsClear;
        [HideInInspector]
        public bool IsAvailable;
    }
}
