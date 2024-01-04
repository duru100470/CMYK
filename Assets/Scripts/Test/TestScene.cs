using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour, IScene
{
    public void Load(object param)
    {
        Debug.Log("Test scene is loaded!");
    }

    public void Unload()
    {
        Debug.Log("Test scene is unloaded!");
    }
}
