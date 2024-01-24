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

public class ColorChangeEvent : IEvent
{
    public ColorType ChangColor; // 어떤 색깔로 바뀌는지에 관한 정보
}

