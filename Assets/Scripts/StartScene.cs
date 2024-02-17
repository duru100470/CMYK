using BasicInjector;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : SceneScope, IScene
{
    public SceneScope SceneScope => this;

    [SerializeField]
    private GameObject _inject;

    public override void Load(object param)
    {
        base.Load();
        GameObjectInjector.InjectSingle(_inject, Container);
        SceneLoader.Instance.LoadSceneAsync<MainScene>(null).Forget();
    }

    public override void Unload()
    {
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
    }
}
