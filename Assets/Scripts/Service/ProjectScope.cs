using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public abstract class ProjectScope : MonoBehaviour, IInstaller
{
    public static ProjectScope Instance { get; private set; }

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Multiple ProjectScope were detected! Please fix it");
    }

    public abstract void InitializeContainer(ContainerBuilder containerBuilder);
}