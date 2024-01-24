using System;

public class ReactiveProperty<T>
{
    private T _value = default;
    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    /// <summary>
    /// 값 변경 시 호출되는 이벤트
    /// </summary>
    public event Action<T> OnValueChanged;

    public ReactiveProperty() { }
    public ReactiveProperty(T value) => _value = value;

    /// <summary>
    /// Reference 타입일 경우 값 변경 시 Notify()를 통해 이벤트 수동 호출해야 함
    /// </summary>
    public void Notify()
        => OnValueChanged?.Invoke(_value);
}