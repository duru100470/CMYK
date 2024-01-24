using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AssetLoader
{
    private readonly Dictionary<string, UnityEngine.Object> _loadedDict =
        new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// Assets/Prefab 아래의 프리팹을 로드하는 루틴을 리턴한다.
    /// </summary>
    public async UniTask<T> LoadPrefabAsync<T>(string prefabAddress) where T : UnityEngine.Object
    {
        if (_loadedDict.TryGetValue(prefabAddress, out var v))
        {
            return (T)v;
        }

        var op = await Resources.LoadAsync<T>("Prefabs/" + prefabAddress);

        _loadedDict[prefabAddress] = (T)op;
        return (T)op;
    }

    /// <summary>
    /// Assets/Prefab 아래의 프리팹을 로드하고 로드가 끝날 때까지 기다린다. (여러 개를 연속으로 로드할 때 쓰면 심각한 퍼포먼스 저하가 있을 수도 있음)
    /// </summary>
    public T LoadPrefab<T>(string prefabAddress) where T : UnityEngine.Object
    {
        if (_loadedDict.TryGetValue(prefabAddress, out var v))
        {
            return (T)v;
        }

        var op = Resources.Load<T>("Prefabs/" + prefabAddress);

        if (op == null)
        {
            Debug.LogWarning($"No prefab loaded (Prefabs/{prefabAddress}.prefab)");
            return null;
        }

        _loadedDict[prefabAddress] = op;
        return op;
    }
}