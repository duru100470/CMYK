
using System;
using System.Linq;
using UnityEngine.Analytics;

namespace BasicInjector
{
    public static class AttributeInjector
    {
        public static void Inject(object obj, Container container)
        {
            var type = obj.GetType();

            var fields = type.GetFields().Where(f => f.IsDefined(typeof(InjectAttribute), false));
            var properties = type.GetProperties().Where(p => p.CanWrite && p.IsDefined(typeof(InjectAttribute), false));

            foreach (var f in fields)
            {
                FieldInjector.Inject(f, obj, container);
            }

            foreach (var p in properties)
            {
                PropertyInjector.Inject(p, obj, container);
            }
        }
    }
}