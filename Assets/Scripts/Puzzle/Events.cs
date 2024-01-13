using MessageChannel;

public class PlayerEvent : IEvent
{
    public PlayerEventType Type;
}

public enum PlayerEventType
{
    GameOver,
    GameClear
}