using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NetworkManager
{
    private static double s_timeout = 5;

    public static async UniTask<string> SendToServer(string url, SendType sendType, string jsonBody = null)
    {
        var networkReachability = await CheckNetwork();

        if (!networkReachability)
            return default;

        var cts = new CancellationTokenSource();
        cts.CancelAfterSlim(TimeSpan.FromSeconds(s_timeout));

        var request = new UnityWebRequest(url, sendType.ToString())
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        if (!string.IsNullOrEmpty(jsonBody))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        SetHeaders(request);

        try
        {
            var res = await request.SendWebRequest().WithCancellation(cts.Token);
            return res.downloadHandler.text;
        }
        catch (OperationCanceledException ex)
        {
            if (ex.CancellationToken == cts.Token)
            {
                Debug.Log("Sending failed: Timeout");
            }
            return default;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return default;
        }
    }

    public static async UniTask<T> SendToServer<T>(string url, SendType sendType, string jsonBody = null)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(await SendToServer(url, sendType, jsonBody));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return default;
        }
    }

    private static async UniTask<bool> CheckNetwork()
    {
        var cts = new CancellationTokenSource();

        cts.CancelAfterSlim(TimeSpan.FromSeconds(s_timeout));

        try
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("The network is not connected");
                await UniTask.WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable, PlayerLoopTiming.Update, cts.Token);
                Debug.Log("The network is connected");
                return true;
            }
        }
        catch (OperationCanceledException ex)
        {
            if (ex.CancellationToken == cts.Token)
            {
                Debug.Log("Network connecting failed: Timeout");
            }
            return false;
        }

        return true;
    }

    private static void SetHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("Content-Type", "application/json");
    }
}

public enum SendType
{
    GET,
    POST,
    PUT,
    DELETE
}

public struct GameEvent
{
    public string uid;
    public string type;
    public string version;
    public int? data;
    public DateTime time;

    public GameEvent(string uid, string type, string version, int? data = null)
    {
        this.uid = uid;
        this.type = type;
        this.version = version;
        this.data = data;
        time = DateTime.Now;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}