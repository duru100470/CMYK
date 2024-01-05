using BasicInjector;
using MessageChannel;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    [Inject]
    public Channel<TestMessage> channel;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            channel.Subscribe(SendMessage);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            channel.Unsubscribe(SendMessage);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            channel.Notify(new TestMessage() { Message = "TEST!!!" });
        }
    }

    private void SendMessage(TestMessage testMessage)
    {
        Debug.Log(testMessage.Message);
    }
}

public class TestMessage
{
    public string Message;
}