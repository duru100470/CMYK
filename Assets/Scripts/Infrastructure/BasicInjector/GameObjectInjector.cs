using System.Collections.Generic;
using UnityEngine;

namespace BasicInjector
{
    public static class GameObjectInjector
    {
        public static void InjectSingle(GameObject gameObject, Container container)
        {
            var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

            for (int j = 0; j < monoBehaviours.Length; j++)
            {
                var monoBehaviour = monoBehaviours[j];

                if (monoBehaviour != null)
                {
                    AttributeInjector.Inject(monoBehaviour, container);
                }

                (monoBehaviour as IInitializable)?.Initialize();
            }
        }

        public static void InjectRecursiveMany(List<GameObject> gameObject, Container container)
        {
            var monoBehaviours = new List<MonoBehaviour>();

            for (int i = 0; i < gameObject.Count; i++)
            {
                gameObject[i].GetComponentsInChildren(true, monoBehaviours);

                for (int j = 0; j < monoBehaviours.Count; j++)
                {
                    var monoBehaviour = monoBehaviours[j];

                    if (monoBehaviour != null)
                    {
                        AttributeInjector.Inject(monoBehaviour, container);
                    }

                    (monoBehaviour as IInitializable)?.Initialize();
                }
            }
        }
    }
}