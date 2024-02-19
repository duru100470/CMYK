using System.Collections;
using System.Collections.Generic;
using BasicInjector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainProjectScope : ProjectScope
{
    [SerializeField]
    private WorldLoader _worldLoader;
    [SerializeField]
    private SoundController _soundController;
    [SerializeField]
    private GameSetting _gameSetting = new();

    private string _url = "http://duruchigy.ddns.net/cmyk/events";

    private void Awake()
    {
        SendSessionEvent().Forget();
    }

    private async UniTask SendSessionEvent()
    {
        await UniTask.WaitUntil(() => _gameSetting.IsAvailable);

        var e = new GameEvent(_gameSetting.Id, "session_start", Application.version);
        await NetworkManager.SendToServer(_url, SendType.POST, e.ToJson());

        Debug.Log(e.ToJson());
    }

    private void OnApplicationQuit()
    {
        var e = new GameEvent(_gameSetting.Id, "session_finish", Application.version);

        NetworkManager.SendToServer(_url, SendType.POST, e.ToJson()).Forget();
    }

    public override void InitializeContainer(ContainerBuilder builder)
    {
        Debug.Log("Project Scope!!");
        builder.AddSingleton<GameSetting>(_gameSetting);
        builder.AddTransient<AssetLoader>();
        builder.AddSingleton<WorldLoader>(_worldLoader);
        builder.AddSingleton<ISoundController>(_soundController);
    }

    [ContextMenu("Reset Settings")]
    public void ResetSetting()
    {
        PlayerPrefs.DeleteKey("GameSettings");
    }
}
