using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicInjector
{
    public class SingletonValueResolver : IResolver
    {
        private object _instance;
        public Lifetime Lifetime => Lifetime.Singleton;

        public SingletonValueResolver(object instance)
        {
            _instance = instance;
        }

        public object Resolve(Container container)
        {
            return _instance;
        }

        public void Dispose()
        {
        }
    }
}