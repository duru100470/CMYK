using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    private GameSetting _gameSetting;

    [SerializeField]
    private List<WorldScriptableObject> _worldScriptableObjects;

    private bool _isWorldLoaded = false;
    public bool IsWorldLoaded => _isWorldLoaded;
    private int _curWorldIdx = -1, _curMapIdx = -1;
    public (int, int) CurrentMapIndex => (_curWorldIdx, _curMapIdx);

    public async UniTask UpdateClearDataAsync(int worldIdx, int mapIdx, bool isClear = true)
    {
        if (!_isWorldLoaded)
            return;

        _isWorldLoaded = false;

        await _worldScriptableObjects[worldIdx].UpdateClearDataAsync(mapIdx, isClear);

        var tasks = new List<UniTask>();

        for (int i = 0; i < _worldScriptableObjects.Count; i++)
        {
            tasks.Add(_worldScriptableObjects[i].LoadClearDataAsync());
        }

        await UniTask.WhenAll(tasks.ToArray());

        _isWorldLoaded = true;
    }

    public bool TryLoadMap(int worldIdx, int mapIdx, out MapData data)
    {
        data = null;
        var targetWorld = _worldScriptableObjects[worldIdx];

        if (_isWorldLoaded && targetWorld.IsAvailable && targetWorld.TryGetMapData(mapIdx, out data))
        {
            _curMapIdx = -1;
            _curWorldIdx = -1;
            return true;
        }

        _curMapIdx = -1;
        _curWorldIdx = -1;
        return false;
    }

    public MapStatus GetStatus(int worldIdx, int mapIdx)
    {
        if (_isWorldLoaded)
            return _worldScriptableObjects[worldIdx].GetStatus(mapIdx);
        else
            return MapStatus.Locked;
    }

    public int GetWorldLength(int worldIdx)
        => _worldScriptableObjects[worldIdx].MapLength;

    public async UniTask InitWorlds(GameSetting gameSetting)
    {
        _gameSetting = gameSetting;

        var tasks = new List<UniTask>();

        foreach (var world in _worldScriptableObjects)
        {
            tasks.Add(world.LoadAsync(_gameSetting));
        }

        await UniTask.WhenAll(tasks.ToArray());

        _isWorldLoaded = true;
        Debug.Log("World loading is finished!");
    }
}

public enum MapStatus
{
    Cleared,
    Available,
    Locked
}