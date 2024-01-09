using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GameSetting
{
    public float MasterVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float SFXVolume = 1.0f;

    public GameSetting()
    {
        LoadAsync().Forget();
    }

    private const string SaveKey = "GameSettings";

    /// <summary>
    /// Convert this object to json string
    /// </summary>
    /// <returns>json string</returns>
    private string ToJSON()
    {
        return JsonUtility.ToJson(this, false); ;
    }

    /// <summary>
    /// Apply this object from settingJson
    /// </summary>
    /// <param name="settingJson">json string</param>
    private void FromJSON(string settingJson)
    {
        JsonUtility.FromJsonOverwrite(settingJson, this);
    }

    private async UniTask<string> LoadSettingsAsync()
    {
        var json = "";

        // check setting saved
        await UniTask.SwitchToMainThread();

        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return json;
        }

        // get string from playerprefs
        json = PlayerPrefs.GetString(SaveKey);
        return json;
    }

    public void Save()
    {
        var json = ToJSON();

        // save to playerprefs
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public async UniTaskVoid LoadAsync()
    {
        // try load
        var json = await LoadSettingsAsync();

        // if no settings saved, save default settings
        if (string.IsNullOrEmpty(json))
        {
            Save();
            return;
        }

        // if saved, apply to this object
        FromJSON(json);
    }
}