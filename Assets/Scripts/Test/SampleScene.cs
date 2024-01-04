using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : MonoBehaviour, IScene
{
    public void Load(object param = null)
    {
        Debug.Log("Sample Scene is loaded!");
    }

    public void Unload()
    {
        Debug.Log("Sample Scene is unloaded!");
    }
}
