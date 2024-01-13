using System;
using System.Collections.Generic;

namespace BasicInjector
{
    public sealed class ContainerBuilder
    {
        private Dictionary<Type, IResolver> _buffer = new();
        private Container _container = null;

        public Container Build()
        {
            var resolverDict = new Dictionary<Type, IResolver>();

            foreach (var (k, v) in _buffer)
            {
                resolverDict[k] = v;
            }

            var instance = new Container(resolverDict, _container);
            return instance;
        }

        public ContainerBuilder AddSingleton<T>()
        {
            _buffer[typeof(T)] = new SingletonResolver(typeof(T));
            return this;
        }

        /// <typeparam name="T">실제 타입</typeparam>
        /// <typeparam name="U">바인딩 될 타입</typeparam>
        public ContainerBuilder AddSingletonAs<T, U>() where T : U
        {
            _buffer[typeof(U)] = new SingletonResolver(typeof(T));
            return this;
        }

        public ContainerBuilder AddSingleton<T>(object instance)
        {
            _buffer[typeof(T)] = new SingletonValueResolver(instance);
            return this;
        }

        /// <typeparam name="T">실제 타입</typeparam>
        /// <typeparam name="U">바인딩 될 타입</typeparam>
        public ContainerBuilder AddSingletonAs<T, U>(object instance) where T : U
        {
            _buffer[typeof(U)] = new SingletonValueResolver(instance);
            return this;
        }

        public ContainerBuilder AddTransient<T>()
        {
            _buffer[typeof(T)] = new TransientResolver(typeof(T));
            return this;
        }

        /// <typeparam name="T">실제 타입</typeparam>
        /// <typeparam name="U">바인딩 될 타입</typeparam>
        public ContainerBuilder AddTransientAs<T, U>() where T : U
        {
            _buffer[typeof(U)] = new TransientResolver(typeof(T));
            return this;
        }

        public ContainerBuilder SetParent(Container container)
        {
            _container = container;
            return this;
        }
    }
}