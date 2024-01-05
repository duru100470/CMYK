using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Unity.VisualScripting;
using UnityEngine;

public class TestProjectScope : ProjectScope
{
    [SerializeField]
    private TestMono _testMono;

    public override void InitializeContainer(ContainerBuilder builder)
    {
        Debug.Log("Project Scope!!");
        builder.AddSingleton<TestMono>(_testMono);
    }
}
