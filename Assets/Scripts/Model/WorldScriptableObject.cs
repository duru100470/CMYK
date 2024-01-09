using System;
using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "new World", menuName = "Models/World Data")]
public class WorldScriptableObject : ScriptableObject
{
    [HideInInspector]
    [Inject]
    public GameSetting _setting;

    public int Index;
    public List<MapInfo> Maps;
    public List<int> Requires;

    public async UniTaskVoid LoadAsync()
    {
        await UniTask.WaitUntil(() => _setting.IsAvailable == true);

        foreach (var map in Maps)
        {
            // TODO: Load ClearData from GameSetting
        }
    }
}

[Serializable]
public class MapInfo
{
    public int Index;
    public MapData Data;
    public List<int> Requires;
    public bool IsClear;
}