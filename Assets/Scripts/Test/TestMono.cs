using BasicInjector;
using MessageChannel;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    [Inject]
    public Channel<TestMessage> channel;

    private void SendMessage(TestMessage testMessage)
    {
        Debug.Log(testMessage.Message);
    }
}

public class TestMessage
{
    public string Message;
}