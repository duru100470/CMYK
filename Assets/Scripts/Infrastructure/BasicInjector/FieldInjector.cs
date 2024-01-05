using System;
using System.Reflection;
using UnityEngine;

namespace BasicInjector
{
    public static class FieldInjector
    {
        public static void Inject(FieldInfo fieldInfo, object obj, Container container)
        {
            try
            {
                fieldInfo.SetValue(obj, container.Resolve(fieldInfo.FieldType));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
