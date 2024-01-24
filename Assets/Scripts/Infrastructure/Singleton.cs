using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; } = null;

    /// <summary>
    /// 싱글톤 상속 후 Awake() 작성 시 반드시 제일 처음에 base.Awake()를 실행해 주어야 함.
    /// </summary>
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// 싱글톤 상속 후 OnDestroy() 작성 시 반드시 제일 처음에 base.OnDestroy()를 실행해 주어야 함.
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

public abstract class Singleton<T> where T : new()
{
    private static T s_instance;
    public static T Instance => s_instance ??= new T();
}
