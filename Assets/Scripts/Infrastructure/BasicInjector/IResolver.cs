using System;

namespace BasicInjector
{
    public interface IResolver : IDisposable
    {
        Lifetime Lifetime { get; }
        object Resolve(Container container);
    }

    public enum Lifetime
    {
        Singleton,
        Transient
    }
}