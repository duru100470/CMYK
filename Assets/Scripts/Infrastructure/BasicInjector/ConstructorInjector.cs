
using System;
using System.Collections.Generic;

namespace BasicInjector
{
    public static class ConstructorInjector
    {
        public static object Inject(Type type, Container container)
        {
            var ctors = type.GetConstructors();
            var @params = ctors[0].GetParameters();

            if (@params.Length > 0)
            {
                var args = new List<object>();

                foreach (var p in @params)
                {
                    args.Add(container.Resolve(p.ParameterType));
                }

                var instance = Activator.CreateInstance(type, args);

                return instance;
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}