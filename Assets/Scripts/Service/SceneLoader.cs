using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    private IScene _current;
    private ProjectScope _projectScope;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _projectScope = GetComponent<ProjectScope>();
        _projectScope?.Init();

        _current = GetISceneInActiveScene();
        _current.Load(null);
    }

    public async UniTaskVoid LoadSceneAsync<TScene>(object param) where TScene : IScene
    {
        try
        {
            _current?.Unload();

            var name = typeof(TScene).FullName.ToString();
            await SceneManager.LoadSceneAsync(name);

            _current = GetISceneInActiveScene();

            if (_current is not TScene)
                throw new InvalidSceneObjectException("Scene object doesn't match Loaded Scene.");

            _current.Load(param);
        }
        catch (Exception e)
        {
            Debug.LogError($"Unexpected error occurred when scene loading: {e.Message}");
        }
    }

    private IScene GetISceneInActiveScene()
    {
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();
        IScene ret = null;

        foreach (var obj in objs)
        {
            if (obj.TryGetComponent<IScene>(out var component))
                ret = component;
        }

        if (ret == null)
            throw new InvalidSceneObjectException("Scene object is not found.");

        return ret;
    }
}

public class InvalidSceneObjectException : Exception
{
    public InvalidSceneObjectException() { }
    public InvalidSceneObjectException(string message) : base(message) { }
}