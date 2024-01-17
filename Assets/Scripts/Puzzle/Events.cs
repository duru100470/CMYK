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

public class PlayerMoveEvent : IEvent
{
    public PlayerMoveEventType Type;
}

public enum PlayerMoveEventType
{
    TrueMove,
    FakeMove
}

