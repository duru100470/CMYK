using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;

// TODO: VContainer로 바꿔야함

public abstract class ProjectScope : MonoBehaviour, IInstaller
{
    public static ProjectScope Instance { get; private set; }
    private Container _container;

    public Container Container => _container;

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;

            var containerBuilder = new ContainerBuilder();
            InitializeContainer(containerBuilder);
            _container = containerBuilder.Build();
        }
        else
            Debug.LogError("Multiple ProjectScope were detected! Please fix it");
    }

    public abstract void InitializeContainer(ContainerBuilder containerBuilder);
}