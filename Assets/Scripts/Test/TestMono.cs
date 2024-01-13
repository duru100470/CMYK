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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            channel.Subscribe(SendMessage);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            channel.Unsubscribe(SendMessage);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            channel.Notify(new TestMessage() { Message = "test" });
        }
    }

    private void SendMessage(TestMessage testMessage)
    {
        Debug.Log(testMessage.Message);
    }
}

public class TestMessage : IEvent
{
    public string Message;
}

public class PlayerDeathEvent : IEvent
{
    public string Reason;
}