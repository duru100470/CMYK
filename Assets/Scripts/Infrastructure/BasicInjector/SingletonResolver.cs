using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicInjector
{
    public class SingletonResolver : IResolver
    {
        private object _instance;
        private readonly Type _type;
        public Lifetime Lifetime => Lifetime.Singleton;

        public SingletonResolver(Type type)
        {
            _type = type;
        }

        public object Resolve(Container container)
        {
            if (_instance == null)
            {
                _instance = container.Construct(_type);
            }

            return _instance;
        }

        public void Dispose()
        {
        }
    }
}