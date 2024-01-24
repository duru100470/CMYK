using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace BasicInjector
{
    public class Container : IDisposable
    {
        internal Dictionary<Type, IResolver> ResolverDict { get; }
        private Container _parentContainer;

        public Container(Dictionary<Type, IResolver> resolverDict, Container container)
        {
            ResolverDict = resolverDict;
            _parentContainer = container;
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
            var resolver = GetResolver(type) ?? throw new ResolverNotFoundException(type.ToString());
            var instance = resolver.Resolve(this);
            return instance;
        }

        public IResolver GetResolver(Type type)
        {
            if (_parentContainer != null && _parentContainer.HasBinding(type))
            {
                return _parentContainer.GetResolver(type);
            }
            else
            {
                return ResolverDict.GetValueOrDefault(type);
            }
        }

        public object Construct(Type type)
        {
            var instance = ConstructorInjector.Inject(type, this);
            AttributeInjector.Inject(instance, this);

            (instance as IInitializable)?.Initialize();

            return instance;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OverrideSelfInjection()
        {
            ResolverDict[typeof(Container)] = new SingletonValueResolver(this);
        }
    }
}