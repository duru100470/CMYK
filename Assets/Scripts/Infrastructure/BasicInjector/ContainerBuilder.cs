using System;
using System.Collections.Generic;

namespace BasicInjector
{
    public sealed class ContainerBuilder
    {
        private Dictionary<Type, IResolver> _buffer = new();

        public Container Build()
        {
            var resolverDict = new Dictionary<Type, IResolver>();

            foreach (var (k, v) in _buffer)
            {
                resolverDict[k] = v;
            }

            var instance = new Container(resolverDict);
            return instance;
        }

        public ContainerBuilder AddSingleton<T>()
        {
            _buffer[typeof(T)] = new SingletonResolver(typeof(T));
            return this;
        }

        public ContainerBuilder AddSingleton<T>(object instance)
        {
            _buffer[typeof(T)] = new SingletonValueResolver(instance);
            return this;
        }

        public ContainerBuilder AddTransient<T>()
        {
            _buffer[typeof(T)] = new TransientResolver(typeof(T));
            return this;
        }
    }
}