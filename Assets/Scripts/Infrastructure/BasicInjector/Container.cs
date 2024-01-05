using System;
using System.Collections.Generic;

namespace BasicInjector
{
    public class Container : IDisposable
    {
        internal Dictionary<Type, IResolver> ResolverDict { get; }

        public Container(Dictionary<Type, IResolver> resolverDict)
        {
            ResolverDict = resolverDict;
            OverrideSelfInjection();
        }

        public bool HasBinding<T>()
            => HasBinding(typeof(T));

        public bool HasBinding(Type type)
            => ResolverDict.ContainsKey(type);

        public T Resolve<T>()
            => (T)Resolve(typeof(T));

        public object Resolve(Type type)
        {
            var resolver = GetResolver(type);
            var instance = resolver.Resolve(this);
            return instance;
        }

        public IResolver GetResolver(Type type)
        {
            return ResolverDict.GetValueOrDefault(type);
        }

        public object Construct(Type type)
        {
            var instance = ConstructorInjector.Inject(type, this);
            AttributeInjector.Inject(instance, this);
            return instance;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OverrideSelfInjection()
        {
            // ResolverDict[typeof(Container)] = new SingletonValueResolver(this);
        }
    }
}