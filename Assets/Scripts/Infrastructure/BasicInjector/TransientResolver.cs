using System;

namespace BasicInjector
{
    public class TransientResolver : IResolver
    {
        private readonly Type _type;
        public Lifetime Lifetime => Lifetime.Transient;

        public TransientResolver(Type type)
        {
            _type = type;
        }

        public object Resolve(Container container)
        {
            var instance = container.Construct(_type);
            return instance;
        }

        public void Dispose()
        {
        }
    }
}