using System;
using System.Reflection;
using UnityEngine;

namespace BasicInjector
{
    public static class PropertyInjector
    {
        public static void Inject(PropertyInfo propertyInfo, object obj, Container container)
        {
            try
            {
                propertyInfo.SetValue(obj, container.Resolve(propertyInfo.PropertyType));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
